using UnityEngine;
using Unity.Cinemachine;

/// <summary>
/// Ana döngü yöneticisi.
/// Karakter seçimi → Cinemachine geçişi → İç ses → Badge → Döngü → Sonder
/// </summary>
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
    public BotPanelController botPanelController;
    public GameObject returnButton;

    [Header("Badge Objeleri (Banner içindeki tick GO'lar)")]
    public GameObject badgeA;
    public GameObject badgeB;
    public GameObject badgeC;

    [Header("Banner Controller'lar (her canvas'tan)")]
    public BannerController[] bannerControllers;

    // İç durum
    private CinemachineCamera[] charCams;
    private int selectedIndex = -1;
    private bool[] visited = new bool[3];
    private int visitedCount = 0;

    void Start()
    {
        charCams = new CinemachineCamera[] { camA, camB, camC };
        ResetAllCams();
        camObserver.Priority = 10;

        // Badge'leri kapat
        SetBadge(0, false); SetBadge(1, false); SetBadge(2, false);

        Invoke(nameof(ShowBotPanel), 2f);
    }

    void ShowBotPanel()
    {
        botPanelController.ShowCharacterSelect();
    }

    // BotPanelController S1'den çağrılır
    public void SelectCharacter(int index)
    {
        selectedIndex = index;
        PlayWhoosh();
        RefreshBanners(index);

        // Cinemachine: seçilen karaktere geç
        charCams[index].Priority = 20;

        // 1.8sn blend sonrası S2 panelini aç
        Invoke(nameof(ShowFirstImpression), 1.8f);
    }

    void ShowFirstImpression()
    {
        botPanelController.ShowFirstImpression();
    }

    // BotPanelController S3 "Devam Et"ten çağrılır
    public void OnContinueToInnerVoice()
    {
        PlayWhoosh();
        Invoke(nameof(TriggerInnerVoice), 0.5f);
    }

    void TriggerInnerVoice()
    {
        FindFirstObjectByType<InnerVoiceController>()
            .PlayInnerVoice(selectedIndex);
    }

    // InnerVoiceController ses bitince çağırır
    public void OnInnerVoiceFinished()
    {
        if (returnButton != null)
            returnButton.SetActive(true);
    }

    // ReturnButton OnClick'e bağla
    public void ExitCharacter()
    {
        if (!visited[selectedIndex])
        {
            visited[selectedIndex] = true;
            visitedCount++;
            SetBadge(selectedIndex, true);
            RefreshBanners(-1);
        }

        ResetAllCams();
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

    void ResetAllCams()
    {
        camObserver.Priority = 0;
        foreach (var c in charCams) c.Priority = 0;
        if (camSonder != null) camSonder.Priority = 0;
    }

    void PlayWhoosh()
    {
        if (transitionAudio != null && whooshClip != null)
        {
            transitionAudio.clip = whooshClip;
            transitionAudio.Play();
        }
    }

    void SetBadge(int index, bool active)
    {
        GameObject[] badges = { badgeA, badgeB, badgeC };
        if (badges[index] != null)
            badges[index].SetActive(active);
    }

    public void RefreshBanners(int activeIndex)
    {
        foreach (var b in bannerControllers)
            if (b != null) b.Refresh(activeIndex, visited);
    }
}