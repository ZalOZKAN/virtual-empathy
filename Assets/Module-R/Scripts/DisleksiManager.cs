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
    public Transform okObjesi;
    public Transform[] hedefNoktalari;

    void Start()
    {
        GorevYazisiniGuncelle();
        if (labSoruSeti2 != null) labSoruSeti2.SetActive(false);
    }

    void Update()
    {
        if (okObjesi != null && hedefNoktalari != null && tamamlananGorev < hedefNoktalari.Length)
        {
            Transform suAnkiHedef = hedefNoktalari[tamamlananGorev];
            if (suAnkiHedef != null)
            {
                okObjesi.LookAt(suAnkiHedef);
            }
        }
    }

    // --- 1. GÖREV: BAÞLAT BUTONU ---
    public void EgitimiBaslatButonu()
    {
        if (egitimBasladi) return;
        egitimBasladi = true;

        // Tahtaya bakarak doðmasý için Isinla çaðrýlýyor
        Isinla(hedefNoktalari[0], new Vector3(0, 0.8f, 0));

        if (ekstraHarflerGrubu != null)
        {
            ekstraHarflerGrubu.SetActive(true);
            if (dogruSes != null) dogruSes.Play();
        }
    }

    // --- 2. GÖREV: TAHTADAKÝ "TAMAM" VEYA "LABA GÝT" BUTONU ---
    public void TahtaBittiLabaGit()
    {
        // Eðer buton çalýþmýyorsa bu fonksiyona týklandýðýndan emin ol
        if (tahtaSoruPaneli != null) tahtaSoruPaneli.SetActive(false);

        if (tamamlananGorev == 0)
        {
            GoreviTamamla();
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
            GoreviTamamla();
        }
    }

    // --- IÞINLANMA VE BAKIÞ YÖNÜ AYARI ---
    void Isinla(Transform hedef, Vector3 yukseklikOffset)
    {
        if (xrOrigin != null && hedef != null)
        {
            CharacterController cc = xrOrigin.GetComponent<CharacterController>();
            if (cc != null) cc.enabled = false;

            // POZÝSYON: Hedefin olduðu yere git
            xrOrigin.transform.position = hedef.position + yukseklikOffset;

            // ROTASYON: Yan yatmayý ve tersliði tamamen bitiren ayar
            // Sadece Y ekseninde (sað-sol) hedefin baktýðý yöne bak, asla X ve Z'ye dokunma.
            xrOrigin.transform.rotation = Quaternion.Euler(0, hedef.eulerAngles.y, 0);

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
                           t2 + " Sorularý Doðru Cevabýný Ýþaretle\n" +
                           t3 + " Ýnfografikleri Ýncele ve Soruyu Cevapla";
    }
}