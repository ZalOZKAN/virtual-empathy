using System.Collections;
using System.Text;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Networking;
using Unity.Cinemachine;
using TMPro;

public class GameManager : MonoBehaviour
{
    // ─── CANVAS'LAR ───────────────────────────────────────────
    [Header("Canvas'lar")]
    public GameObject canvas1Guide;
    public GameObject canvas2IlkIzlenim;
    public GameObject canvas3Gecis;
    public GameObject canvas4A;
    public GameObject canvas5B;
    public GameObject canvas6C;
    public GameObject canvas7Soru;
    public GameObject canvas8Sonder;
    public GameObject canvasFade;
    public GameObject canvasExit;

    // ─── BUTONLAR ─────────────────────────────────────────────
    [Header("1-Guide Butonları")]
    public Button btnA;
    public Button btnB;
    public Button btnC;

    [Header("2-İlk İzlenim Butonları")]
    public Button btnOpt1;
    public Button btnOpt2;
    public Button btnOpt3;

    [Header("3-Geçiş Butonu")]
    public Button btnDevam3;

    [Header("4/5/6 Devam Butonları")]
    public Button btnDevam4;
    public Button btnDevam5;
    public Button btnDevam6;

    [Header("7-Soru")]
    public Button btnGonder;
    public TMP_InputField inputField;

    [Header("Exit Butonları")]
    public Button btnExit;
    public Button btnRestart;

    // ─── METINLER ─────────────────────────────────────────────
    [Header("Metin Alanları")]
    public TextMeshProUGUI text1Soru;
    public TextMeshProUGUI text2Soru;
    public TextMeshProUGUI text3Yanit;
    public TextMeshProUGUI text4Icsesi;
    public TextMeshProUGUI text5Icsesi;
    public TextMeshProUGUI text6Icsesi;
    public TextMeshProUGUI text7Bot;
    public TextMeshProUGUI text7Cevap;
    public TextMeshProUGUI text8Sonder;

    // ─── SES ──────────────────────────────────────────────────
    [Header("Audio Source")]
    public AudioSource audioSource;

    [Header("Ses Klipleri")]
    public AudioClip clipMasadaki;
    public AudioClip clipIlkIzlenim;
    public AudioClip clipGecis;
    public AudioClip clipA;
    public AudioClip clipB;
    public AudioClip clipC;
    public AudioClip clipSonder;
    public AudioClip clipWhoosh;

    // ─── CİNEMACHINE ──────────────────────────────────────────
    [Header("Cinemachine Kameralar")]
    public CinemachineCamera camObserver;
    public CinemachineCamera camA;
    public CinemachineCamera camB;
    public CinemachineCamera camC;
    public CinemachineCamera camSonder;

    // ─── BANNER CANVASGROUP'LAR ───────────────────────────────
    [Header("Banner Badge'leri (her canvas için A/B/C)")]
    public CanvasGroup[] badgesA; // size 6: Guide,2,3,4,5,6,7
    public CanvasGroup[] badgesB;
    public CanvasGroup[] badgesC;

    // ─── FADE ─────────────────────────────────────────────────
    [Header("Fade")]
    public CanvasGroup fadeGroup;
    public float fadeDuration = 1f;

    // ─── GEMİNİ ───────────────────────────────────────────────
    [Header("Gemini API")]
    public string geminiApiKey = "BURAYA_KEY_YAZ";

    // ─── INTERNAL STATE ───────────────────────────────────────
    private int selectedChar = -1;
    private bool[] visited = new bool[3];
    private int visitedCount = 0;
    private bool isWaiting = false;

    private readonly string[] icSesMetni = {
        // A - Sosyal Kaygı
        "Bana söz gelecek mi acaba. Umarım gelmez.\n" +
        "Bilgisayarla meşgul görünüyorum, bu daha güvenli.\n" +
        "Dün gece bir şeyler hazırlamıştım ama şu an net değil.\n" +
        "Yanlış bir şey söylersem nasıl görünürüm diye düşünüyorum.\n" +
        "Gelirse kısa tutayım. Fazla konuşmadan geçeyim.",

        // B - Zihinsel Yük
        "Az önce ne dedi, kaçırdım.\n" +
        "Odaklanmaya çalışıyorum ama olmuyor.\n" +
        "O konu hâlâ aklımda dönüyor.\n" +
        "Karar vermem gerekiyor ama veremiyorum.\n" +
        "Buradayım. Ama zihnim burada değil.",

        // C - Duygusal Yük
        "Bugüne zaten böyle başladım.\n" +
        "Konuşulanları duyuyorum ama içime girmiyor.\n" +
        "Normal görünmeye çalışıyorum.\n" +
        "Parmaklarımı durduramıyorum. Fark ediyorum ama olmuyor.\n" +
        "İçimde bir şey var. Ne olduğunu bilmiyorum ama ağır.\n" +
        "Kimse fark etmeden bu gün geçse yeter."
    };

    private const string SonderMetni =
        "Sonder...\n\n" +
        "Her insanın, senin hiç farkında olmadığın\n" +
        "derin ve karmaşık bir iç dünyası olduğunu\n" +
        "fark etme anı.\n\n" +
        "Bugün masada gördüklerin — sadece birer\n" +
        "davranış değildi. Birer pencereydi.";

    private const string SystemContext =
        "Sen bir empati simülasyonu rehberisin. Kullanıcı az önce sosyal kaygı, " +
        "zihinsel yük ve duygusal yük yaşayan üç farklı kişinin iç sesini deneyimledi. " +
        "Sorulara kısa, içten ve psikolojik açıdan bilgili yanıtlar ver. " +
        "Türkçe konuş. Maksimum 3 cümle.";

    // ══════════════════════════════════════════════════════════
    void Start()
    {
        BaglantilariKur();
        TumCanvaslariKapat();
        canvas1Guide.SetActive(true);
        ResetCams();
        camObserver.Priority = 10;
        PlayClip(clipMasadaki);
        if (text1Soru) text1Soru.text =
            "Masadaki kişilerden hangisi dikkatini çekti?\n" +
            "Seni görmediğin bir tarafla karşı karşıya bırakabilir.";
        BadgeGuncelle(-1);
    }

    // ── BUTON BAĞLANTILARI ────────────────────────────────────
    void BaglantilariKur()
    {
        if (btnA) btnA.onClick.AddListener(() => KarakterSec(0));
        if (btnB) btnB.onClick.AddListener(() => KarakterSec(1));
        if (btnC) btnC.onClick.AddListener(() => KarakterSec(2));

        if (btnOpt1) btnOpt1.onClick.AddListener(() => SecenekSecildi());
        if (btnOpt2) btnOpt2.onClick.AddListener(() => SecenekSecildi());
        if (btnOpt3) btnOpt3.onClick.AddListener(() => SecenekSecildi());

        if (btnDevam3) btnDevam3.onClick.AddListener(() => IcSesBaslat());

        if (btnDevam4) btnDevam4.onClick.AddListener(() => KarakterCik());
        if (btnDevam5) btnDevam5.onClick.AddListener(() => KarakterCik());
        if (btnDevam6) btnDevam6.onClick.AddListener(() => KarakterCik());

        if (btnGonder) btnGonder.onClick.AddListener(() => SoruGonder());

        if (btnExit)    btnExit.onClick.AddListener(() => CikisYap());
        if (btnRestart) btnRestart.onClick.AddListener(() => UnityEngine.SceneManagement.SceneManager.LoadScene(0));
    }

    // ══════════════════════════════════════════════════════════
    // AKIŞ: 1 → Karakter Seç
    void KarakterSec(int index)
    {
        selectedChar = index;
        PlayWhoosh();
        BadgeGuncelle(index);
        ResetCams();
        CinemachineCamera[] cams = { camA, camB, camC };
        cams[index].Priority = 20;

        canvas1Guide.SetActive(false);
        Invoke(nameof(IlkIzlenimAc), 1.8f);
    }

    // AKIŞ: 2 → İlk İzlenim
    void IlkIzlenimAc()
    {
        canvas2IlkIzlenim.SetActive(true);
        PlayClip(clipIlkIzlenim);
        if (text2Soru) text2Soru.text = "Bu kişi hakkında ilk izlenimin nedir?";
    }

    // AKIŞ: 3 → Seçenek seçildi → Geçiş
    void SecenekSecildi()
    {
        canvas2IlkIzlenim.SetActive(false);
        canvas3Gecis.SetActive(true);
        PlayClip(clipGecis);
        if (text3Yanit) text3Yanit.text =
            "Öyle mi? Hadi bir bakalım.\n" +
            "Şimdi kendini onun yerine koy ve\n" +
            "aynı anı onun gözlerinden deneyimle.";
        if (btnDevam3) btnDevam3.gameObject.SetActive(false);
        float delay = clipGecis != null ? clipGecis.length + 0.5f : 2f;
        Invoke(nameof(DevamButonuGoster), delay);
    }

    void DevamButonuGoster()
    {
        if (btnDevam3) btnDevam3.gameObject.SetActive(true);
    }

    // AKIŞ: 4/5/6 → İç Ses
    void IcSesBaslat()
    {
        canvas3Gecis.SetActive(false);
        GameObject[] icSesCanvaslari = { canvas4A, canvas5B, canvas6C };
        TextMeshProUGUI[] icSesMetinleri = { text4Icsesi, text5Icsesi, text6Icsesi };
        AudioClip[] clips = { clipA, clipB, clipC };
        Button[] devamButonlari = { btnDevam4, btnDevam5, btnDevam6 };

        GameObject hedef = icSesCanvaslari[selectedChar];
        hedef.SetActive(true);
        if (icSesMetinleri[selectedChar])
            icSesMetinleri[selectedChar].text = icSesMetni[selectedChar];
        if (devamButonlari[selectedChar])
            devamButonlari[selectedChar].gameObject.SetActive(false);

        PlayClip(clips[selectedChar]);
        float sure = clips[selectedChar] != null ? clips[selectedChar].length + 0.8f : 3f;
        Invoke(nameof(IcSesDevamGoster), sure);
    }

    void IcSesDevamGoster()
    {
        Button[] devamButonlari = { btnDevam4, btnDevam5, btnDevam6 };
        if (devamButonlari[selectedChar])
            devamButonlari[selectedChar].gameObject.SetActive(true);
    }

    // AKIŞ: Karakter bitti → döngü veya sonder
    void KarakterCik()
    {
        GameObject[] icSesCanvaslari = { canvas4A, canvas5B, canvas6C };
        icSesCanvaslari[selectedChar].SetActive(false);

        if (!visited[selectedChar])
        {
            visited[selectedChar] = true;
            visitedCount++;
        }

        ResetCams();
        camObserver.Priority = 10;
        BadgeGuncelle(-1);

        if (visitedCount >= 3)
            Invoke(nameof(SonderBaslat), 1.5f);
        else
        {
            canvas1Guide.SetActive(true);
            PlayClip(clipMasadaki);
        }
    }

    // AKIŞ: 8 → Sonder
    void SonderBaslat() { StartCoroutine(SonderSequence()); }

    IEnumerator SonderSequence()
    {
        yield return StartCoroutine(Fade(0f, 1f));
        canvas8Sonder.SetActive(true);
        if (text8Sonder) text8Sonder.text = "";
        yield return StartCoroutine(Fade(1f, 0f));

        PlayClip(clipSonder);
        if (text8Sonder) yield return StartCoroutine(YaziYaz(SonderMetni, 0.04f));

        float kalan = clipSonder != null ? clipSonder.length - SonderMetni.Length * 0.04f : 0f;
        yield return new WaitForSeconds(kalan > 0f ? kalan : 1.5f);

        yield return StartCoroutine(Fade(0f, 1f));
        canvas8Sonder.SetActive(false);
        canvas7Soru.SetActive(true);
        if (text7Bot) text7Bot.text =
            "Dışarıdan görülen her davranış, o kişinin iç dünyasını tam olarak yansıtmaz.\n" +
            "Sessizlik her zaman ilgisizlik anlamına gelmez.";
        if (text7Cevap) text7Cevap.text = "";
        yield return StartCoroutine(Fade(1f, 0f));
    }

    // AKIŞ: 7 → Gemini
    void SoruGonder()
    {
        if (inputField == null) return;
        string soru = inputField.text.Trim();
        if (string.IsNullOrEmpty(soru) || isWaiting) return;
        inputField.text = "";
        GeminiSor(soru);
    }

    void GeminiSor(string soru)
    {
        if (isWaiting) return;
        if (text7Cevap) text7Cevap.text = "...";
        StartCoroutine(GeminiCall(soru));
    }

    IEnumerator GeminiCall(string soru)
    {
        isWaiting = true;
        if (btnGonder) btnGonder.interactable = false;

        string url = "https://generativelanguage.googleapis.com/v1beta/models/" +
                     "gemini-2.0-flash:generateContent?key=" + geminiApiKey;
        string prompt = SystemContext + "\n\nKullanıcı sorusu: " + soru;
        string json = "{\"contents\":[{\"parts\":[{\"text\":" + JsonUtility.ToJson(prompt) + "}]}]}";

        using UnityWebRequest req = new UnityWebRequest(url, "POST");
        req.uploadHandler   = new UploadHandlerRaw(Encoding.UTF8.GetBytes(json));
        req.downloadHandler = new DownloadHandlerBuffer();
        req.SetRequestHeader("Content-Type", "application/json");
        yield return req.SendWebRequest();

        if (req.result == UnityWebRequest.Result.Success)
        {
            if (text7Cevap) text7Cevap.text = ParseGemini(req.downloadHandler.text);
        }
        else
        {
            if (text7Cevap) text7Cevap.text = "Bağlantı hatası.";
            Debug.LogWarning("[GameManager] Gemini hata: " + req.error);
        }

        isWaiting = false;
        if (btnGonder) btnGonder.interactable = true;
    }

    string ParseGemini(string json)
    {
        try
        {
            int idx   = json.IndexOf("\"text\":");
            if (idx < 0) return "Yanıt alınamadı.";
            int start = json.IndexOf("\"", idx + 7) + 1;
            int end   = json.IndexOf("\"", start);
            return json.Substring(start, end - start)
                       .Replace("\\n", "\n").Replace("\\\"", "\"");
        }
        catch { return "Yanıt işlenemedi."; }
    }

    // ── YARDIMCI FONKSİYONLAR ─────────────────────────────────
    void TumCanvaslariKapat()
    {
        foreach (var c in new[] {
            canvas1Guide, canvas2IlkIzlenim, canvas3Gecis,
            canvas4A, canvas5B, canvas6C,
            canvas7Soru, canvas8Sonder, canvasExit })
            if (c != null) c.SetActive(false);
    }

    void ResetCams()
    {
        if (camObserver) camObserver.Priority = 0;
        if (camA)        camA.Priority        = 0;
        if (camB)        camB.Priority        = 0;
        if (camC)        camC.Priority        = 0;
        if (camSonder)   camSonder.Priority   = 0;
    }

    void PlayClip(AudioClip clip)
    {
        if (audioSource == null || clip == null) return;
        audioSource.clip = clip;
        audioSource.Play();
    }

    void PlayWhoosh()
    {
        if (audioSource != null && clipWhoosh != null)
        { audioSource.clip = clipWhoosh; audioSource.Play(); }
    }

    void BadgeGuncelle(int aktif)
    {
        CanvasGroup[][] tumBadgeler = { badgesA, badgesB, badgesC };
        for (int i = 0; i < 3; i++)
        {
            if (tumBadgeler[i] == null) continue;
            float alpha = visited[i] ? 0.3f : (i == aktif ? 1f : 0.3f);
            foreach (var cg in tumBadgeler[i])
                if (cg != null) cg.alpha = alpha;
        }
    }

    IEnumerator YaziYaz(string metin, float hiz)
    {
        text8Sonder.text = "";
        foreach (char c in metin)
        {
            text8Sonder.text += c;
            yield return new WaitForSeconds(hiz);
        }
    }

    IEnumerator Fade(float baslangic, float bitis)
    {
        if (fadeGroup == null) yield break;
        fadeGroup.gameObject.SetActive(true);
        float gecen = 0f;
        while (gecen < fadeDuration)
        {
            gecen += Time.deltaTime;
            fadeGroup.alpha = Mathf.Lerp(baslangic, bitis, gecen / fadeDuration);
            yield return null;
        }
        fadeGroup.alpha = bitis;
        if (bitis == 0f) fadeGroup.gameObject.SetActive(false);
    }

    void CikisYap()
    {
        Application.Quit();
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#endif
    }
}