using System.Collections;
using UnityEngine;
using Unity.Cinemachine;

public class SonderVoiceoverController : MonoBehaviour
{
    [Header("Kamera")]
    public CinemachineCamera camSonder;

    [Header("Ses")]
    public AudioSource sonderSource;
    public AudioClip sonderClip;

    [Header("Görsel")]
    public CanvasGroup fadeCanvas;

    [Header("Sonraki Panel")]
    public GameObject chatBotPanel;

    public float fadeDuration = 1.5f;

    public void PlaySonder()
    {
        StartCoroutine(SonderSequence());
    }

    IEnumerator SonderSequence()
    {
        // Sonder kamerasina gec
        camSonder.Priority = 25;

        // Hafif loslas (alpha 0 → 0.4)
        yield return StartCoroutine(FadeCanvas(0f, 0.4f, fadeDuration));

        // Ses baslat
        float clipLength = 10f; // Gercek ses gelince guncellenir
        if (sonderClip != null)
        {
            sonderSource.clip = sonderClip;
            sonderSource.Play();
            clipLength = sonderClip.length;
        }

        // Ses bitene kadar bekle
        yield return new WaitForSeconds(clipLength);

        // Losluğu kaldir
        yield return StartCoroutine(FadeCanvas(0.4f, 0f, fadeDuration));

        // Sonder kamerasini kapat
        camSonder.Priority = 0;

        // 1 saniye bekle, chatbot'u ac
        yield return new WaitForSeconds(1f);
        if (chatBotPanel != null)
            chatBotPanel.SetActive(true);
    }

    IEnumerator FadeCanvas(float from, float to, float dur)
    {
        float t = 0f;
        fadeCanvas.alpha = from;
        while (t < dur)
        {
            t += Time.deltaTime;
            fadeCanvas.alpha = Mathf.Lerp(from, to, t / dur);
            yield return null;
        }
        fadeCanvas.alpha = to;
    }
}
