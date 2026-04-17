using UnityEngine;
using Unity.Cinemachine;

public class CharacterSelectManager : MonoBehaviour
{
    [Header("Cinemachine Kameralar")]
    public CinemachineCamera camObserver;
    public CinemachineCamera camA;
    public CinemachineCamera camB;
    public CinemachineCamera camC;
    public CinemachineCamera camSonder;

    [Header("Ses")]
    public AudioSource transitionAudio;
    public AudioClip whooshClip;

    [Header("UI Referanslar")]
    public GameObject returnButton;
    public BotPanelController botPanelController;

    private CinemachineCamera[] charCams;
    private int selectedIndex = -1;
    private bool[] visitedChars = new bool[3];
    private int visitedCount = 0;

    void Start()
    {
        charCams = new CinemachineCamera[] { camA, camB, camC };
        ResetAllCameras();
        camObserver.Priority = 10;
        Invoke(nameof(ShowBotPanel), 3f);
    }

    void ShowBotPanel()
    {
        botPanelController.ShowCharacterSelect();
    }

    public void SelectCharacter(int index)
    {
        selectedIndex = index;

        if (whooshClip != null)
        {
            transitionAudio.clip = whooshClip;
            transitionAudio.Play();
        }

        charCams[index].Priority = 20;
        Invoke(nameof(TriggerInnerVoice), 1.8f);
    }

    void TriggerInnerVoice()
    {
        FindFirstObjectByType<InnerVoiceController>()
            .PlayInnerVoice(selectedIndex);
    }

    public void ExitCharacter()
    {
        if (!visitedChars[selectedIndex])
        {
            visitedChars[selectedIndex] = true;
            visitedCount++;
        }

        ResetAllCameras();
        camObserver.Priority = 10;

        if (returnButton != null)
            returnButton.SetActive(false);

        if (visitedCount >= 3)
            Invoke(nameof(TriggerSonder), 2f);
        else
            Invoke(nameof(ShowBotPanel), 1.5f);
    }

    void TriggerSonder()
    {
        FindFirstObjectByType<SonderVoiceoverController>().PlaySonder();
    }

    void ResetAllCameras()
    {
        camObserver.Priority = 0;
        foreach (var cam in charCams) cam.Priority = 0;
        if (camSonder != null) camSonder.Priority = 0;
    }
}