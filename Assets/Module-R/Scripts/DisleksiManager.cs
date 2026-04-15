using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class DisleksiManager : MonoBehaviour
{
    [Header("Görsel & Ses Ayarlarý")]
    public GameObject ekstraHarflerGrubu;
    public Transform kamera;
    public GameObject xrOrigin;
    public AudioSource dogruSes;

    [Header("UI & Görev Paneli")]
    public TextMeshProUGUI gorevYazisi;
    public GameObject[] kapilar;
    private int tamamlananGorev = 0;
    private bool egitimBasladi = false;

    [Header("Görev Objeleri (Soru Panelleri)")]
    public GameObject tahtaSoruPaneli;
    public GameObject labSoruSeti1;
    public GameObject labSoruSeti2;
    public GameObject infografikButonlari;

    [Header("Scary Teacher Oku Ayarlarý")]
    public Transform okObjesi;              // Senin sarý "Ok_Gövde" objen
    public Transform[] hedefNoktalari;     // Okun sýrayla bakacaðý yerler

    void Start()
    {
        GorevYazisiniGuncelle();
        if (labSoruSeti2 != null) labSoruSeti2.SetActive(false);
    }

    void Update()
    {
        // --- OKUN HEDEFE BAKMA MANTIÐI ---
        if (okObjesi != null && hedefNoktalari != null && tamamlananGorev < hedefNoktalari.Length)
        {
            Transform suAnkiHedef = hedefNoktalari[tamamlananGorev];
            if (suAnkiHedef != null)
            {
                // Okun hedefe bakmasýný saðlar
                okObjesi.LookAt(suAnkiHedef);

                // NOT: Ok yan bakýyorsa aþaðýdaki satýrýn baþýndaki // iþaretini kaldýr 
                // ve 90 rakamýný (180, -90 gibi) deðiþtirerek dene:
                // okObjesi.Rotate(0, 90, 0); 
            }
        }
    }

    // --- 1. GÖREV: BAÞLAT BUTONU ---
    public void EgitimiBaslatButonu()
    {
        if (egitimBasladi) return;
        egitimBasladi = true;

        // Okun ilk hedefine (Tahta) ýþýnlar
        Isinla(hedefNoktalari[0], new Vector3(0, 0.8f, 0));

        if (ekstraHarflerGrubu != null)
        {
            ekstraHarflerGrubu.SetActive(true);
            if (dogruSes != null) dogruSes.Play();
        }
    }

    // --- 2. GÖREV: LAB IÞINLANMA ---
    public void TahtaBittiLabaGit()
    {
        if (tamamlananGorev == 0)
        {
            if (tahtaSoruPaneli != null) tahtaSoruPaneli.SetActive(false);
            GoreviTamamla(); // Bu iþlem tamamlananGorev'i artýrýr, ok otomatik döner
            Isinla(hedefNoktalari[1], Vector3.zero);
        }
    }

    public void LabSoru1BittiSoru2yeGec()
    {
        if (labSoruSeti1 != null) labSoruSeti1.SetActive(false);
        if (labSoruSeti2 != null) labSoruSeti2.SetActive(true);
    }

    public void LabBittiInfografikKapisiniAc()
    {
        if (tamamlananGorev == 1)
        {
            if (labSoruSeti2 != null) labSoruSeti2.SetActive(false);
            GoreviTamamla(); // Ok otomatik olarak 3. hedefe (Ýnfografik) döner
        }
    }

    // --- GENEL IÞINLANMA ---
    void Isinla(Transform hedef, Vector3 yukseklikOffset)
    {
        if (xrOrigin != null && hedef != null)
        {
            CharacterController cc = xrOrigin.GetComponent<CharacterController>();
            if (cc != null) cc.enabled = false;

            xrOrigin.transform.position = hedef.position + yukseklikOffset;
            xrOrigin.transform.rotation = hedef.rotation;

            if (cc != null) cc.enabled = true;
        }
    }

    public void GoreviTamamla()
    {
        tamamlananGorev++;
        GorevYazisiniGuncelle();

        int kapiIdx = tamamlananGorev - 1;
        if (kapilar != null && kapiIdx < kapilar.Length && kapilar[kapiIdx] != null)
        {
            LeanTween.rotateY(kapilar[kapiIdx], 90f, 2f).setEase(LeanTweenType.easeInOutQuad);
        }

        // Tüm görevler bittiðinde oku kapatýr
        if (tamamlananGorev >= hedefNoktalari.Length && okObjesi != null)
        {
            okObjesi.gameObject.SetActive(false);
        }
    }

    void GorevYazisiniGuncelle()
    {
        if (gorevYazisi == null) return;

        string t1 = (tamamlananGorev >= 1) ? "[X]" : "[ ]";
        string t2 = (tamamlananGorev >= 2) ? "[X]" : "[ ]";
        string t3 = (tamamlananGorev >= 3) ? "[X]" : "[ ]";

        gorevYazisi.text = tamamlananGorev + "/3 Görev Tamamlandý\n\n" +
                           t1 + " Tahtadaki Harfleri Ýncele\n" +
                           t2 + " Laboratuvar Görevini Yap\n" +
                           t3 + " Ýnfografik Odasýný Ýncele";
    }
}