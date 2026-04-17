// BUILD ÖNCESİ SAHNEden KALDIR
using UnityEngine;
using Unity.Cinemachine;

public class EditorTestController : MonoBehaviour
{
    [Header("Canvas'lar")]
    public GameObject guideCanvas;
    public GameObject canvas2IlkIzlenim;
    public GameObject canvas3OyleMi;
    public GameObject canvas4AIcses;
    public GameObject canvas5BIcses;
    public GameObject canvas6CIcses;
    public GameObject canvas7Sorucevap;
    public GameObject canvas8Sonder;

    [Header("Cinemachine Kameralar")]
    public CinemachineCamera camObserver;
    public CinemachineCamera camA;
    public CinemachineCamera camB;
    public CinemachineCamera camC;
    public CinemachineCamera camSonder;

    [Header("Ses")]
    public AudioSource audioSource;
    public AudioClip clipMasadaki;
    public AudioClip clipBuKisi;
    public AudioClip clipOyleMi;
    public AudioClip clipA;
    public AudioClip clipB;
    public AudioClip clipC;
    public AudioClip clipSonder;
    public AudioClip clipWhoosh;

    private GameObject[] allCanvases;

    void Start()
    {
        allCanvases = new GameObject[]
        { guideCanvas, canvas2IlkIzlenim, canvas3OyleMi,
          canvas4AIcses, canvas5BIcses, canvas6CIcses,
          canvas7Sorucevap, canvas8Sonder };
        CloseAll(); ResetCams(); if (camObserver) camObserver.Priority = 10;
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Alpha1)) ShowOnly(guideCanvas,         clipMasadaki);
        if (Input.GetKeyDown(KeyCode.Alpha2)) ShowOnly(canvas2IlkIzlenim,   clipBuKisi);
        if (Input.GetKeyDown(KeyCode.Alpha3)) ShowOnly(canvas3OyleMi,       clipOyleMi);
        if (Input.GetKeyDown(KeyCode.Alpha4)) ShowOnly(canvas4AIcses,       clipA);
        if (Input.GetKeyDown(KeyCode.Alpha5)) ShowOnly(canvas5BIcses,       clipB);
        if (Input.GetKeyDown(KeyCode.Alpha6)) ShowOnly(canvas6CIcses,       clipC);
        if (Input.GetKeyDown(KeyCode.Alpha7)) ShowOnly(canvas7Sorucevap,    null);
        if (Input.GetKeyDown(KeyCode.Alpha8)) ShowOnly(canvas8Sonder,       clipSonder);
        if (Input.GetKeyDown(KeyCode.Alpha0)) { CloseAll(); ResetCams(); if (camObserver) camObserver.Priority = 10; }
        if (Input.GetKeyDown(KeyCode.Q)) SwitchCam(camA);
        if (Input.GetKeyDown(KeyCode.W)) SwitchCam(camB);
        if (Input.GetKeyDown(KeyCode.E)) SwitchCam(camC);
        if (Input.GetKeyDown(KeyCode.R)) SwitchCam(camSonder);
        if (Input.GetKeyDown(KeyCode.T)) { ResetCams(); if (camObserver) camObserver.Priority = 10; }
        if (Input.GetKeyDown(KeyCode.S)) { if (audioSource) audioSource.Stop(); }
        if (Input.GetKeyDown(KeyCode.Space)) PlayClip(clipWhoosh);
    }

    void ShowOnly(GameObject target, AudioClip clip)
    {
        CloseAll();
        if (target != null) target.SetActive(true);
        if (clip   != null) PlayClip(clip);
    }

    void SwitchCam(CinemachineCamera cam)
    {
        ResetCams(); PlayClip(clipWhoosh);
        if (cam != null) cam.Priority = 20;
    }

    void CloseAll()  { foreach (var c in allCanvases) if (c != null) c.SetActive(false); }

    void ResetCams()
    {
        if (camObserver) camObserver.Priority = 0;
        if (camA)        camA.Priority        = 0;
        if (camB)        camB.Priority        = 0;
        if (camC)        camC.Priority        = 0;
        if (camSonder)   camSonder.Priority   = 0;
    }

    void PlayClip(AudioClip clip)
    {
        if (audioSource == null || clip == null) return;
        audioSource.clip = clip; audioSource.Play();
    }
}
