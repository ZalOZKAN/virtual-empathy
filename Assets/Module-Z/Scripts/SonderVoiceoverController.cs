using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class SonderVoiceoverController : MonoBehaviour
{
    [Header("Ses")]
    public AudioSource sonderSource;
    public AudioClip sonderClip;

    [Header("Sonder Canvas")]
    public GameObject sonderCanvas;
    public TextMeshProUGUI sonderText;
    public ScrollRect scrollRect;

    [Header("Fade")]
    public CanvasGroup fadeCanvasGroup;
    public float fadeDuration = 1f;

    [Header("Sonraki Panel")]
    public GameObject chatBotCanvas;

    [Header("Referanslar")]
    public CanvasFollower canvasFollower;

    private const string SonderBody =
        "Sonder...\n\n" +
        "Her insanın, senin hiç farkında olmadığın\n" +
        "derin ve karmaşık bir iç dünyası olduğunu\n" +
        "fark etme anı.\n\n" +
        "Bugün masada gördüklerin — sadece birer\n" +
        "davranış değildi. Birer pencereydi.";

    public void PlaySonder() { StartCoroutine(SonderSequence()); }

    IEnumerator SonderSequence()
    {
        yield return StartCoroutine(Fade(0f, 1f));
        sonderCanvas.SetActive(true);
        canvasFollower?.SnapToCamera();
        if (sonderText != null) sonderText.text = "";
        yield return StartCoroutine(Fade(1f, 0f));

        if (sonderSource != null && sonderClip != null)
        { sonderSource.clip = sonderClip; sonderSource.Play(); }

        if (sonderText != null)
            yield return StartCoroutine(TypeText(SonderBody, 0.04f));

        float remaining = sonderClip != null
            ? sonderClip.length - SonderBody.Length * 0.04f : 0f;
        yield return new WaitForSeconds(remaining > 0f ? remaining : 1.5f);

        yield return StartCoroutine(Fade(0f, 1f));
        sonderCanvas.SetActive(false);
        if (chatBotCanvas != null) chatBotCanvas.SetActive(true);
        yield return StartCoroutine(Fade(1f, 0f));
    }

    IEnumerator TypeText(string fullText, float charDelay)
    {
        sonderText.text = "";
        foreach (char c in fullText)
        {
            sonderText.text += c;
            if (scrollRect != null)
            { Canvas.ForceUpdateCanvases(); scrollRect.verticalNormalizedPosition = 0f; }
            yield return new WaitForSeconds(charDelay);
        }
    }

    IEnumerator Fade(float from, float to)
    {
        if (fadeCanvasGroup == null) yield break;
        fadeCanvasGroup.gameObject.SetActive(true);
        float elapsed = 0f;
        while (elapsed < fadeDuration)
        {
            elapsed += Time.deltaTime;
            fadeCanvasGroup.alpha = Mathf.Lerp(from, to, elapsed / fadeDuration);
            yield return null;
        }
        fadeCanvasGroup.alpha = to;
        if (to == 0f) fadeCanvasGroup.gameObject.SetActive(false);
    }
}
