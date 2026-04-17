using UnityEngine;
using System.Collections;

public class MissionManager : MonoBehaviour
{
    [Header("Iþýnlanma Ayarlarý")]
    public Transform sinif1Noktasi; // Seni ýþýnlayacaðýmýz hedef nokta
    public GameObject player;       // XR Origin (VR) objen

    [Header("Disleksi Efekti Ayarlarý")]
    public GameObject ekstraHarflerGrubu; // Kapalý olan "EkstraHarf" objen
    public float efektGecikmesi = 2f;      // Iþýnlandýktan kaç saniye sonra harfler çýksýn?

    // Butona basýldýðýnda çalýþan ana fonksiyon
    public void StartComputerMission()
    {
        if (player != null && sinif1Noktasi != null)
        {
            // 1. Adým: Oyuncuyu sýnýfa ýþýnla
            player.transform.position = sinif1Noktasi.position;
            player.transform.rotation = sinif1Noktasi.rotation;

            // 2. Adým: Harf animasyonlarýný baþlat (Gecikmeli)
            StartCoroutine(DisleksiSürpriziniBaslat());
        }
        else
        {
            Debug.LogError("Remle! Inspector'dan Player veya Sinif Noktasini sürüklemeyi unutma!");
        }
    }

    IEnumerator DisleksiSürpriziniBaslat()
    {
        // Iþýnlandýktan sonra kýsa bir süre bekle (Göz alýþmasý için)
        yield return new WaitForSeconds(efektGecikmesi);

        if (ekstraHarflerGrubu != null)
        {
            // Harfleri görünür yap!
            ekstraHarflerGrubu.SetActive(true);

            // Harflerin içindeki "HarfDansi" scriptleri Start() ile otomatik baþlayacak.
            Debug.Log("Disleksi harfleri uçuþmaya baþladý!");
        }
    }
}