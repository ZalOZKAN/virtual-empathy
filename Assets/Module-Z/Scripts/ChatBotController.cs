using System.Collections;
using System.Text;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Networking;
using TMPro;

/// <summary>
/// HTML S8 — AI Rehber Soru Modu.
/// Preset sorular veya serbest yazı → Gemini API yanıtı.
/// </summary>
public class ChatBotController : MonoBehaviour
{
    [Header("UI")]
    public TextMeshProUGUI botMessageText;      // Üstteki açıklama metni
    public TextMeshProUGUI answerText;          // Yanıt alanı
    public TMP_InputField inputField;           // Kullanıcı yazı alanı
    public Button sendButton;
    public ScrollRect answerScroll;

    [Header("Preset Butonlar (4 adet)")]
    public Button[] presetButtons;

    [Header("Sonraki Panel")]
    public GameObject exitCanvas;

    [Header("Gemini API")]
    [Tooltip("Google AI Studio'dan alınan API key")]
    public string geminiApiKey = "BURAYA_API_KEY_YAZ";

    // Preset sorular
    private readonly string[] presetQuestions =
    {
        "Bu kişi neden 'iyiyim' dedi?",
        "Toplantıda neden odaklanamadı?",
        "Böyle birine nasıl yaklaşmak gerekir?",
        "Bu davranışların altında ne olabilir?"
    };

    // Gemini sistem bağlamı
    private const string SystemContext =
        "Sen bir empati simülasyonu rehberisin. Kullanıcı az önce sosyal kaygı, " +
        "zihinsel yük ve duygusal yük yaşayan üç farklı kişinin iç sesini deneyimledi. " +
        "Sorulara kısa, içten ve psikolojik açıdan bilgili yanıtlar ver. " +
        "Türkçe konuş. Maksimum 3 cümle.";

    private bool isWaiting = false;

    void Awake()
    {
        sendButton.onClick.AddListener(OnSendPressed);

        for (int i = 0; i < presetButtons.Length; i++)
        {
            int capture = i;
            presetButtons[i].onClick.AddListener(() => AskQuestion(presetQuestions[capture]));
        }
    }

    void OnEnable()
    {
        botMessageText.text =
            "Dışarıdan görülen her davranış, o kişinin iç dünyasını tam olarak yansıtmaz.\n" +
            "Kısa cevaplar her zaman kabalık değildir.\n" +
            "Sessizlik her zaman ilgisizlik anlamına gelmez.";

        answerText.text = "";
        inputField.text = "";
    }

    public void OnSendPressed()
    {
        string q = inputField.text.Trim();
        if (string.IsNullOrEmpty(q) || isWaiting) return;
        inputField.text = "";
        AskQuestion(q);
    }

    void AskQuestion(string question)
    {
        if (isWaiting) return;
        answerText.text = "...";
        StartCoroutine(CallGemini(question));
    }

    IEnumerator CallGemini(string question)
    {
        isWaiting = true;
        sendButton.interactable = false;

        string url = $"https://generativelanguage.googleapis.com/v1beta/models/" +
                     $"gemini-2.0-flash:generateContent?key={geminiApiKey}";

        // Gemini request body
        string fullPrompt = SystemContext + "\n\nKullanıcı sorusu: " + question;

        string jsonBody = "{\"contents\":[{\"parts\":[{\"text\":" +
                          JsonUtility.ToJson(fullPrompt) + "}]}]}";

        byte[] bodyRaw = Encoding.UTF8.GetBytes(jsonBody);

        using UnityWebRequest req = new UnityWebRequest(url, "POST");
        req.uploadHandler   = new UploadHandlerRaw(bodyRaw);
        req.downloadHandler = new DownloadHandlerBuffer();
        req.SetRequestHeader("Content-Type", "application/json");

        yield return req.SendWebRequest();

        if (req.result == UnityWebRequest.Result.Success)
        {
            string responseText = ParseGeminiResponse(req.downloadHandler.text);
            answerText.text = responseText;

            // Scroll en üste
            if (answerScroll != null)
            {
                Canvas.ForceUpdateCanvases();
                answerScroll.verticalNormalizedPosition = 1f;
            }
        }
        else
        {
            answerText.text = "Bağlantı hatası. Lütfen tekrar deneyin.";
            Debug.LogWarning("[ChatBot] Gemini API hatası: " + req.error);
        }

        isWaiting = false;
        sendButton.interactable = true;
    }

    string ParseGeminiResponse(string json)
    {
        // Gemini yanıt formatı: candidates[0].content.parts[0].text
        // Unity'nin JsonUtility bunu desteklemez, basit string parse yapıyoruz.
        try
        {
            int textIdx = json.IndexOf("\"text\":");
            if (textIdx < 0) return "Yanıt alınamadı.";

            int start = json.IndexOf("\"", textIdx + 7) + 1;
            int end   = json.IndexOf("\"", start);
            string raw = json.Substring(start, end - start);

            // Escape karakterleri düzelt
            raw = raw.Replace("\\n", "\n").Replace("\\\"", "\"");
            return raw;
        }
        catch
        {
            return "Yanıt işlenemedi.";
        }
    }

    // "Devam Et" / çıkış butonu
    public void GoToExit()
    {
        gameObject.SetActive(false);
        if (exitCanvas != null)
            exitCanvas.SetActive(true);
    }
}