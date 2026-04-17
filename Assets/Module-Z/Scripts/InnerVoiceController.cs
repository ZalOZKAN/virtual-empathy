using UnityEngine;
using TMPro;

public class InnerVoiceController : MonoBehaviour
{
    [Header("Ses")]
    public AudioSource innerVoiceSource;
    public AudioClip clipA;
    public AudioClip clipB;
    public AudioClip clipC;

    [Header("İç Ses Canvas")]
    public GameObject innerVoiceCanvas;
    public TextMeshProUGUI characterLabel;
    public TextMeshProUGUI innerVoiceText;

    [Header("Return Butonu")]
    public GameObject returnButton;

    [Header("Referanslar")]
    public CanvasFollower canvasFollower;
    public CharacterSelectManager charSelectManager;

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

        if (characterLabel != null) characterLabel.text = labels[index];
        if (returnButton   != null) returnButton.SetActive(false);

        innerVoiceCanvas.SetActive(true);
        canvasFollower?.SnapToCamera();

        innerVoiceSource.clip = clip;
        innerVoiceSource.Play();
        Invoke(nameof(OnVoiceEnd), clip.length + 0.8f);
    }

    void OnVoiceEnd() { charSelectManager.OnInnerVoiceFinished(); }

    public void OnReturnPressed()
    {
        innerVoiceCanvas.SetActive(false);
        charSelectManager.ExitCharacter();
    }
}
