using System.Collections;
using System.Text;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Networking;
using TMPro;

public class ChatBotController : MonoBehaviour
{
    [Header("UI")]
    public TextMeshProUGUI botMessageText;
    public TextMeshProUGUI answerText;
    public TMP_InputField inputField;
    public Button sendButton;
    public ScrollRect answerScroll;

    [Header("Preset Butonlar")]
    public Button[] presetButtons;

    [Header("Sonraki Panel")]
    public GameObject exitCanvas;

    [Header("Gemini API")]
    public string geminiApiKey = "BURAYA_API_KEY_YAZ";

    private readonly string[] presetQuestions =
    {
        "Bu kişi neden 'iyiyim' dedi?",
        "Toplantıda neden odaklanamadı?",
        "Böyle birine nasıl yaklaşmak gerekir?",
        "Bu davranışların altında ne olabilir?"
    };

    private const string SystemContext =
        "Sen bir empati simülasyonu rehberisin. Kullanıcı az önce sosyal kaygı, " +
        "zihinsel yük ve duygusal yük yaşayan üç farklı kişinin iç sesini deneyimledi. " +
        "Sorulara kısa, içten ve psikolojik açıdan bilgili yanıtlar ver. " +
        "Türkçe konuş. Maksimum 3 cümle.";

    private bool isWaiting = false;

    void Awake()
    {
        if (sendButton != null)
            sendButton.onClick.AddListener(OnSendPressed);

        for (int i = 0; i < presetButtons.Length; i++)
        {
            int capture = i;
            if (presetButtons[i] != null)
                presetButtons[i].onClick.AddListener(() => AskQuestion(presetQuestions[capture]));
        }
    }

    void OnEnable()
    {
        if (botMessageText != null)
            botMessageText.text =
                "Dışarıdan görülen her davranış, o kişinin iç dünyasını tam olarak yansıtmaz.\n" +
                "Kısa cevaplar her zaman kabalık değildir.\n" +
                "Sessizlik her zaman ilgisizlik anlamına gelmez.";
        if (answerText  != null) answerText.text = "";
        if (inputField  != null) inputField.text = "";
    }

    public void OnSendPressed()
    {
        if (inputField == null) return;
        string q = inputField.text.Trim();
        if (string.IsNullOrEmpty(q) || isWaiting) return;
        inputField.text = "";
        AskQuestion(q);
    }

    void AskQuestion(string question)
    {
        if (isWaiting) return;
        if (answerText != null) answerText.text = "...";
        StartCoroutine(CallGemini(question));
    }

    IEnumerator CallGemini(string question)
    {
        isWaiting = true;
        if (sendButton != null) sendButton.interactable = false;

        string url = "https://generativelanguage.googleapis.com/v1beta/models/" +
                     "gemini-2.0-flash:generateContent?key=" + geminiApiKey;

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
            if (answerText != null)
                answerText.text = ParseGeminiResponse(req.downloadHandler.text);
            if (answerScroll != null)
            { Canvas.ForceUpdateCanvases(); answerScroll.verticalNormalizedPosition = 1f; }
        }
        else
        {
            if (answerText != null)
                answerText.text = "Bağlantı hatası. Lütfen tekrar deneyin.";
            Debug.LogWarning("[ChatBot] Gemini API hatası: " + req.error);
        }

        isWaiting = false;
        if (sendButton != null) sendButton.interactable = true;
    }

    string ParseGeminiResponse(string json)
    {
        try
        {
            int textIdx = json.IndexOf("\"text\":");
            if (textIdx < 0) return "Yanıt alınamadı.";
            int start = json.IndexOf("\"", textIdx + 7) + 1;
            int end   = json.IndexOf("\"", start);
            string raw = json.Substring(start, end - start);
            return raw.Replace("\\n", "\n").Replace("\\\"", "\"");
        }
        catch { return "Yanıt işlenemedi."; }
    }

    public void GoToExit()
    {
        gameObject.SetActive(false);
        if (exitCanvas != null) exitCanvas.SetActive(true);
    }
}
