using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// Her canvas'taki Banner'ı yönetir.
/// Aktif karakter → alpha 1
/// Diğerleri → alpha 0.3
/// Biten karakter → alpha 0.3 + tick aktif
/// </summary>
public class BannerController : MonoBehaviour
{
    [Header("Karakter İmge/Logo Objeleri")]
    public CanvasGroup charA;
    public CanvasGroup charB;
    public CanvasGroup charC;

    [Header("Tick Objeleri")]
    public GameObject tickA;
    public GameObject tickB;
    public GameObject tickC;

    [Header("Aktif Alpha")]
    public float activeAlpha = 1f;
    public float inactiveAlpha = 0.3f;
    public float doneAlpha = 0.3f;

    /// <summary>
    /// Her canvas açılırken çağır.
    /// activeIndex: 0=A, 1=B, 2=C, -1=hepsi eşit
    /// visited: hangi karakterler tamamlandı
    /// </summary>
    public void Refresh(int activeIndex, bool[] visited)
    {
        CanvasGroup[] chars = { charA, charB, charC };
        GameObject[] ticks = { tickA, tickB, tickC };

        for (int i = 0; i < 3; i++)
        {
            if (chars[i] == null) continue;

            if (visited != null && visited[i])
            {
                // Tamamlandı
                chars[i].alpha = doneAlpha;
                if (ticks[i] != null) ticks[i].SetActive(true);
            }
            else if (i == activeIndex)
            {
                // Şu an aktif
                chars[i].alpha = activeAlpha;
                if (ticks[i] != null) ticks[i].SetActive(false);
            }
            else
            {
                // Henüz ziyaret edilmedi
                chars[i].alpha = inactiveAlpha;
                if (ticks[i] != null) ticks[i].SetActive(false);
            }
        }
    }
}