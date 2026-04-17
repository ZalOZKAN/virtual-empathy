using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

/// <summary>
/// 3 karakter bittikten sonra:
/// Sonder sesi çalar → Canvas'ta akan yazı → ChatBotCanvas açılır.
/// </summary>
public class SonderVoiceoverController : MonoBehaviour
{
    [Header("Ses")]
    public AudioSource sonderSource;
    public AudioClip sonderClip;

    [Header("Sonder Canvas")]
    public GameObject sonderCanvas;
    public TextMeshProUGUI sonderText;          // Akan yazı için
    public ScrollRect scrollRect;               // Otomatik scroll (isteğe bağlı)

    [Header("Fade")]
    public CanvasGroup fadeCanvasGroup;         // FadeCanvas > CanvasGroup
    public float fadeDuration = 1f;

    [Header("Sonraki Panel")]
    public GameObject chatBotCanvas;

    [Header("Referanslar")]
    public CanvasFollower canvasFollower;

    // Sonder metni — sese eşlik eden kısa görünür özet
    private const string SonderBody =
        "Sonder...\n\n" +
        "Her insanın, senin hiç farkında olmadığın\n" +
        "derin ve karmaşık bir iç dünyası olduğunu\n" +
        "fark etme anı.\n\n" +
        "Bugün masada gördüklerin — sadece birer\n" +
        "davranış değildi. Birer pencereydi.";

    public void PlaySonder()
    {
        StartCoroutine(SonderSequence());
    }

    IEnumerator SonderSequence()
    {
        // 1. Fade in (siyah)
        yield return StartCoroutine(Fade(0f, 1f));

        // 2. Sonder canvas aç
        sonderCanvas.SetActive(true);
        canvasFollower?.SnapToCamera();

        if (sonderText != null)
            sonderText.text = "";

        // 3. Fade out (sonder canvas görünsün)
        yield return StartCoroutine(Fade(1f, 0f));

        // 4. Ses çal
        if (sonderSource != null && sonderClip != null)
        {
            sonderSource.clip = sonderClip;
            sonderSource.Play();
        }

        // 5. Yazı akan şekilde gelsin
        if (sonderText != null)
            yield return StartCoroutine(TypeText(SonderBody, 0.04f));

        // 6. Ses bitene kadar bekle
        float remaining = sonderClip != null
            ? sonderClip.length - SonderBody.Length * 0.04f
            : 0f;
        if (remaining > 0f)
            yield return new WaitForSeconds(remaining);
        else
            yield return new WaitForSeconds(1.5f);

        // 7. Fade → ChatBot aç
        yield return StartCoroutine(Fade(0f, 1f));
        sonderCanvas.SetActive(false);

        if (chatBotCanvas != null)
            chatBotCanvas.SetActive(true);

        yield return StartCoroutine(Fade(1f, 0f));
    }

    IEnumerator TypeText(string fullText, float charDelay)
    {
        sonderText.text = "";
        foreach (char c in fullText)
        {
            sonderText.text += c;

            // Otomatik scroll aşağıya
            if (scrollRect != null)
            {
                Canvas.ForceUpdateCanvases();
                scrollRect.verticalNormalizedPosition = 0f;
            }

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

        if (to == 0f)
            fadeCanvasGroup.gameObject.SetActive(false);
    }
}