using UnityEngine;
using UnityEngine.UI;
using TMPro;

/// <summary>
/// İç ses panelini yönetir (HTML S4/S5/S6).
/// Ses çalar → panel açılır → ses biter → ReturnButton görünür.
/// </summary>
public class InnerVoiceController : MonoBehaviour
{
    [Header("Ses")]
    public AudioSource innerVoiceSource;
    public AudioClip clipA;
    public AudioClip clipB;
    public AudioClip clipC;

    [Header("İç Ses Canvas")]
    public GameObject innerVoiceCanvas;         // InnerVoiceCanvas objesi
    public TextMeshProUGUI characterLabel;       // "A · Sosyal Kaygı" gibi
    public TextMeshProUGUI innerVoiceText;       // İç ses satırları (isteğe bağlı, ses varsa boş bırak)

    [Header("Return Butonu")]
    public GameObject returnButton;

    [Header("Referanslar")]
    public CanvasFollower canvasFollower;
    public CharacterSelectManager charSelectManager;

    // Karakter bilgileri
    private readonly string[] labels =
    {
        "A  ·  Sosyal Kaygı",
        "B  ·  Zihinsel Yük",
        "C  ·  Duygusal Yük"
    };

    public void PlayInnerVoice(int index)
    {
        AudioClip[] clips = { clipA, clipB, clipC };
        if (index < 0 || index >= clips.Length) return;

        AudioClip clip = clips[index];
        if (clip == null) return;

        // Panel aç
        if (characterLabel != null)
            characterLabel.text = labels[index];

        if (returnButton != null)
            returnButton.SetActive(false);

        innerVoiceCanvas.SetActive(true);
        canvasFollower?.SnapToCamera();

        // Ses çal
        innerVoiceSource.clip = clip;
        innerVoiceSource.Play();

        // Ses bitince ReturnButton'ı göster
        Invoke(nameof(OnVoiceEnd), clip.length + 0.8f);
    }

    void OnVoiceEnd()
    {
        charSelectManager.OnInnerVoiceFinished();
    }

    // ReturnButton OnClick'e bağla
    public void OnReturnPressed()
    {
        innerVoiceCanvas.SetActive(false);
        charSelectManager.ExitCharacter();
    }
}