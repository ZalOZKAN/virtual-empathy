using UnityEngine;

public class BannerController : MonoBehaviour
{
    [Header("Karakter Badge Objeleri (CanvasGroup ekli)")]
    public CanvasGroup charA;
    public CanvasGroup charB;
    public CanvasGroup charC;

    [Header("Tick Objeleri")]
    public GameObject tickA;
    public GameObject tickB;
    public GameObject tickC;

    [Header("Alpha Değerleri")]
    public float activeAlpha = 1f;
    public float inactiveAlpha = 0.3f;
    public float doneAlpha = 0.3f;

    public void Refresh(int activeIndex, bool[] visited)
    {
        CanvasGroup[] chars = { charA, charB, charC };
        GameObject[] ticks = { tickA, tickB, tickC };

        for (int i = 0; i < 3; i++)
        {
            if (chars[i] == null) continue;

            if (visited != null && visited[i])
            {
                chars[i].alpha = doneAlpha;
                if (ticks[i] != null) ticks[i].SetActive(true);
            }
            else if (i == activeIndex)
            {
                chars[i].alpha = activeAlpha;
                if (ticks[i] != null) ticks[i].SetActive(false);
            }
            else
            {
                chars[i].alpha = inactiveAlpha;
                if (ticks[i] != null) ticks[i].SetActive(false);
            }
        }
    }
}
