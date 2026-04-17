using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class LabGorevKontrol : MonoBehaviour
{
    private TextMeshProUGUI gorevYazisi;
    private Image ekranArkaplan;
    private GameObject soru1Paketi;
    private GameObject soru2Paketi;

    void Awake()
    {
        // SENÝN YERÝNE BEN BULUYORUM:
        // Hierarchy'deki isimlere göre otomatik eþleþme yapar.
        gorevYazisi = GetComponentInChildren<TextMeshProUGUI>();
        ekranArkaplan = GetComponent<Image>();

        // Bu isimlerin Hierarchy'deki ile birebir ayný olmasý lazým:
        soru1Paketi = transform.Find("Soru_1_Paketi")?.gameObject;
        soru2Paketi = transform.Find("Soru_2_Paketi")?.gameObject;
    }

    public void Soru1DogruSecildi()
    {
        if (gorevYazisi != null) gorevYazisi.text = "TEBRÝKLER! DOÐRU KELÝMEYÝ BULDUN.";
        if (ekranArkaplan != null) ekranArkaplan.color = Color.green;
        Invoke("IkinciSoruyaGec", 2f);
    }

    private void IkinciSoruyaGec()
    {
        if (ekranArkaplan != null) ekranArkaplan.color = Color.white;
        if (gorevYazisi != null) gorevYazisi.text = "Þimdi doðru 'E' harfini bul!";

        if (soru1Paketi != null) soru1Paketi.SetActive(false);
        if (soru2Paketi != null) soru2Paketi.SetActive(true);
    }

    public void Soru2DogruSecildi()
    {
        if (gorevYazisi != null) gorevYazisi.text = "HARÝKA! Görevler bitti.";
        if (ekranArkaplan != null) ekranArkaplan.color = Color.blue;
    }

    public void YanlisSecim()
    {
        if (gorevYazisi != null) gorevYazisi.text = "TEKRAR DENE!";
        if (ekranArkaplan != null) ekranArkaplan.color = Color.red;
        Invoke("RengiNormalYap", 1f);
    }

    private void RengiNormalYap()
    {
        if (ekranArkaplan != null) ekranArkaplan.color = Color.white;
    }
}