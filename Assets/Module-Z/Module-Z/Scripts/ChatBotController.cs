using UnityEngine;
using TMPro;
using UnityEngine.UI;

[System.Serializable]
public class QA
{
    public string question;
    [TextArea(2, 4)]
    public string answer;
}

public class ChatBotController : MonoBehaviour
{
    [Header("UI Referanslar")]
    public TextMeshProUGUI botMessageText;
    public TextMeshProUGUI answerText;
    public TMP_InputField inputField;
    public GameObject moduleMenuPanel;

    [Header("Ses")]
    public AudioSource botVoice;
    public AudioClip botMessageClip;

    [Header("Soru-Cevap")]
    public QA[] presetQAs;

    [Header("Preset Butonlar")]
    public Button[] presetButtons;

    void Awake()
    {
        // Butonlari kod ile bagla
        for (int i = 0; i < presetButtons.Length; i++)
        {
            int capture = i;
            presetButtons[i].onClick.AddListener(
                () => OnPresetSelected(capture));
        }
    }

    void OnEnable()
    {
        botMessageText.text =
            "Dışarıdan görülen her davranış, o kişinin iç dünyasını tam olarak yansıtmaz. " +
            "Kısa cevaplar her zaman kabalık değildir. Dalgınlık her zaman " +
            "umursamazlık değildir. Sessizlik her zaman ilgisizlik anlamına gelmez. " +
            "Bazen bir insanın davranışlarının arkasında görünmeyen bir yorgunluk, " +
            "bastırılmış bir duygu ya da açıklayamadığı bir zihinsel yük olabilir.";

        if (botVoice != null && botMessageClip != null)
        {
            botVoice.clip = botMessageClip;
            botVoice.Play();
        }

        if (answerText != null)
            answerText.text = "";
    }

    public void OnPresetSelected(int index)
    {
        if (index < presetQAs.Length)
            answerText.text = presetQAs[index].answer;
    }

    public void OnSendPressed()
    {
        string q = inputField.text.Trim().ToLower();
        if (string.IsNullOrEmpty(q)) return;
        answerText.text = FindAnswer(q);
        inputField.text = "";
    }

    string FindAnswer(string q)
    {
        foreach (var qa in presetQAs)
        {
            string keyword = qa.question.Length >= 5
                ? qa.question.Substring(0, 5).ToLower()
                : qa.question.ToLower();
            if (q.Contains(keyword))
                return qa.answer;
        }
        return "Her davranışın arkasında görünmeyen bir neden olabilir. " +
               "Empatiyle yaklaşmak her zaman iyi bir başlangıçtır.";
    }

    public void GoToModuleMenu()
    {
        gameObject.SetActive(false);
        if (moduleMenuPanel != null)
            moduleMenuPanel.SetActive(true);
    }
}   