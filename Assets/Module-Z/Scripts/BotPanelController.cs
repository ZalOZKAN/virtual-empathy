using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class BotPanelController : MonoBehaviour
{
    [Header("Ortak UI")]
    public TextMeshProUGUI questionText;

    [Header("S1 — Karakter Seçim Butonları")]
    public GameObject charSelectGroup;
    public Button btnCharA;
    public Button btnCharB;
    public Button btnCharC;

    [Header("S2 — Metin Seçenekleri")]
    public GameObject optionGroup;
    public Button optionA;
    public Button optionB;
    public Button optionC;
    public TextMeshProUGUI optionAText;
    public TextMeshProUGUI optionBText;
    public TextMeshProUGUI optionCText;

    [Header("S3 — Geçiş Yanıtı")]
    public GameObject responseGroup;
    public TextMeshProUGUI botResponseText;
    public Button continueButton;

    [Header("Ses")]
    public AudioSource botVoice;
    public AudioClip charSelectClip;
    public AudioClip firstImpressClip;
    public AudioClip responseClip;

    [Header("Referanslar")]
    public CanvasFollower canvasFollower;
    public CharacterSelectManager charSelectManager;

    void Awake()
    {
        if (btnCharA) btnCharA.onClick.AddListener(() => OnCharSelected(0));
        if (btnCharB) btnCharB.onClick.AddListener(() => OnCharSelected(1));
        if (btnCharC) btnCharC.onClick.AddListener(() => OnCharSelected(2));
        if (optionA)  optionA.onClick.AddListener(() => OnOptionSelected(0));
        if (optionB)  optionB.onClick.AddListener(() => OnOptionSelected(1));
        if (optionC)  optionC.onClick.AddListener(() => OnOptionSelected(2));
        if (continueButton) continueButton.onClick.AddListener(OnContinuePressed);
    }

    public void ShowCharacterSelect()
    {
        questionText.text =
            "Masadaki kişilerden hangisi dikkatini çekti?\n" +
            "Seni görmediğin bir tarafla karşı karşıya bırakabilir.";
        SetGroups(s1: true, s2: false, s3: false);
        PlayClip(charSelectClip);
        Open();
    }

    public void OnCharSelected(int index)
    {
        Debug.Log($"[BotPanel] Karakter seçildi: {index}");
        gameObject.SetActive(false);
        charSelectManager.SelectCharacter(index);
    }

    public void ShowFirstImpression()
    {
        questionText.text = "Bu kişi hakkında ilk izlenimin nedir?";
        optionAText.text  = "Dalgın görünüyor";
        optionBText.text  = "Soğuk ve mesafeli görünüyor";
        optionCText.text  = "Yorgun olabilir";
        SetGroups(s1: false, s2: true, s3: false);
        PlayClip(firstImpressClip);
        Open();
    }

    public void OnOptionSelected(int index)
    {
        ShowTransition();
    }

    void ShowTransition()
    {
        botResponseText.text =
            "Öyle mi? Hadi bir bakalım.\n" +
            "Şimdi kendini onun yerine koy ve\n" +
            "aynı anı onun gözlerinden deneyimle.";
        continueButton.gameObject.SetActive(false);
        SetGroups(s1: false, s2: false, s3: true);
        PlayClip(responseClip);
        float delay = (botVoice != null && responseClip != null)
            ? responseClip.length + 0.5f : 2f;
        Invoke(nameof(ShowContinueButton), delay);
        Open();
    }

    void ShowContinueButton() { continueButton.gameObject.SetActive(true); }

    public void OnContinuePressed()
    {
        gameObject.SetActive(false);
        charSelectManager.OnContinueToInnerVoice();
    }

    void SetGroups(bool s1, bool s2, bool s3)
    {
        if (charSelectGroup != null) charSelectGroup.SetActive(s1);
        if (optionGroup     != null) optionGroup.SetActive(s2);
        if (responseGroup   != null) responseGroup.SetActive(s3);
    }

    void PlayClip(AudioClip clip)
    {
        if (botVoice == null || clip == null) return;
        botVoice.clip = clip;
        botVoice.Play();
    }

    void Open()
    {
        gameObject.SetActive(true);
        canvasFollower?.SnapToCamera();
    }
}
