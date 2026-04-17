using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class BotPanelController : MonoBehaviour
{
    [Header("UI Referanslar")]
    public TextMeshProUGUI questionText;
    public GameObject buttonGroup;
    public TextMeshProUGUI botResponseText;
    public GameObject continueButton;

    [Header("Butonlar")]
    public Button optionA;
    public Button optionB;
    public Button optionC;

    [Header("Buton Metinleri")]
    public TextMeshProUGUI optionAText;
    public TextMeshProUGUI optionBText;
    public TextMeshProUGUI optionCText;

    [Header("Ses")]
    public AudioSource botVoice;
    public AudioClip charSelectClip;
    public AudioClip firstImpressClip;
    public AudioClip responseClip;

    private int currentMode = 0;

    void Awake()
    {
        optionA.onClick.AddListener(() => OnOptionSelected(0));
        optionB.onClick.AddListener(() => OnOptionSelected(1));
        optionC.onClick.AddListener(() => OnOptionSelected(2));
    }

    public void ShowCharacterSelect()
    {
        currentMode = 0;
        questionText.text =
            "Masadaki kişilerden hangisi dikkatini çekti?\n" +
            "Seçimine dikkat et — bilmediğin şeyleri öğrenebilirsin.";
        optionAText.text = "\"Sabah merhaba bile demedi\" dediğin kişi";
        optionBText.text = "\"Toplantıda hiç yoktu\" dediğin kişi";
        optionCText.text = "\"Soğuk biri\" dediğin kişi";

        if (botResponseText != null)
            botResponseText.gameObject.SetActive(false);
        if (continueButton != null)
            continueButton.SetActive(false);
        if (buttonGroup != null)
            buttonGroup.SetActive(true);

        if (botVoice != null && charSelectClip != null)
        {
            botVoice.clip = charSelectClip;
            botVoice.Play();
        }

        gameObject.SetActive(true);
    }

    public void ShowFirstImpression()
    {
        currentMode = 1;
        questionText.text = "Bu kişi hakkında ilk izlenimin nedir?";
        optionAText.text = "Dalgın görünüyor";
        optionBText.text = "Soğuk ve mesafeli görünüyor";
        optionCText.text = "Yorgun olabilir";

        if (botResponseText != null)
            botResponseText.gameObject.SetActive(false);
        if (continueButton != null)
            continueButton.SetActive(false);
        if (buttonGroup != null)
            buttonGroup.SetActive(true);

        if (botVoice != null && firstImpressClip != null)
        {
            botVoice.clip = firstImpressClip;
            botVoice.Play();
        }
    }

    public void OnOptionSelected(int index)
    {
        buttonGroup.SetActive(false);

        if (currentMode == 0)
        {
            FindFirstObjectByType<CharacterSelectManager>()
                .SelectCharacter(index);
            gameObject.SetActive(false);
        }
        else
        {
            botResponseText.text =
                "Öyle mi? Hadi bir bakalım. Şimdi kendini onun " +
                "yerine koy ve aynı anı onun gözlerinden deneyimle.";
            botResponseText.gameObject.SetActive(true);

            if (botVoice != null && responseClip != null)
            {
                botVoice.clip = responseClip;
                botVoice.Play();
            }

            Invoke(nameof(ShowContinue), 2f);
        }
    }

    void ShowContinue()
    {
        if (continueButton != null)
            continueButton.SetActive(true);
    }
}