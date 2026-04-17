using UnityEngine;

public class InnerVoiceController : MonoBehaviour
{
    public AudioSource innerVoiceSource;
    public AudioClip[] clips; // 0=A, 1=B, 2=C
    public GameObject returnButton;

    public void PlayInnerVoice(int index)
    {
        if (index < 0 || index >= clips.Length) return;
        if (clips[index] == null) return;

        innerVoiceSource.clip = clips[index];
        innerVoiceSource.Play();

        float duration = clips[index].length;
        Invoke(nameof(OnVoiceEnd), duration + 0.8f);
    }

    void OnVoiceEnd()
    {
        returnButton.SetActive(true);
    }
}