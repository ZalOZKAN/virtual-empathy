/*using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuSelected : MonoBehaviour
{
    [Header("Radial Link")]
    [Tooltip("Bos birakirsan sahneden otomatik bulunur.")]
    [SerializeField] private RadialMenu radialMenu;

    [Header("Wrist Menu")]
    [Tooltip("Bu part secildiginde astigmat ayar paneli kola acilir.")]
    [SerializeField] private int wristMenuPartIndex = 0;
    [Tooltip("Bos birakirsan RadialMenu.handTransform kullanilir.")]
    [SerializeField] private Transform wristAnchor;
    [Tooltip("Acilip kapanacak menu koku. Bos birakirsan AstigmatismUIController parent'i kullanilir.")]
    [SerializeField] private Transform menuRootOverride;
    [SerializeField] private AstigmatismUIController astigmatismUIController;
    [SerializeField] private Vector3 wristLocalPosition = new Vector3(0.06f, -0.035f, 0.09f);
    [SerializeField] private Vector3 wristLocalEulerAngles = new Vector3(10f, 170f, 80f);
    [SerializeField] private Vector3 wristLocalScale = Vector3.one;
    [SerializeField] private bool hideMenuOnStart = true;

    [Header("Other Parts")]
    [Tooltip("Secilen part wrist menu degilse eski sahne yukleme davranisini kullanir.")]
    [SerializeField] private bool loadSceneForOtherParts = true;

    GameObject wristMenuRoot;

    void Awake()
    {
        ResolveReferences();

        if (hideMenuOnStart && wristMenuRoot != null)
            wristMenuRoot.SetActive(false);
    }

    void OnEnable()
    {
        ResolveReferences();

        if (radialMenu != null)
            radialMenu.OnPartSelected.AddListener(HandlePartSelected);
    }

    void OnDisable()
    {
        if (radialMenu != null)
            radialMenu.OnPartSelected.RemoveListener(HandlePartSelected);
    }

    void ResolveReferences()
    {
        if (radialMenu == null)
            radialMenu = FindFirstObjectByType<RadialMenu>();

        if (wristAnchor == null && radialMenu != null)
            wristAnchor = radialMenu.handTransform;

        if (astigmatismUIController == null)
            astigmatismUIController = FindFirstObjectByType<AstigmatismUIController>();

        if (menuRootOverride == null && astigmatismUIController != null)
            menuRootOverride = astigmatismUIController.transform.parent != null
                ? astigmatismUIController.transform.parent
                : astigmatismUIController.transform;

        if (menuRootOverride != null)
            wristMenuRoot = menuRootOverride.gameObject;
    }

    void HandlePartSelected(int selectedPartIndex)
    {
        if (selectedPartIndex == wristMenuPartIndex)
        {
            ToggleWristMenu();
            return;
        }

        if (loadSceneForOtherParts)
            LoadSceneByIndex(selectedPartIndex);
    }

    public void ToggleWristMenu()
    {
        ResolveReferences();

        if (wristMenuRoot == null)
        {
            Debug.LogWarning("[MenuSelected] Wrist menu root bulunamadi.");
            return;
        }

        bool willOpen = !wristMenuRoot.activeSelf;
        if (willOpen)
            AttachMenuToWrist();

        wristMenuRoot.SetActive(willOpen);
    }

    public void ShowWristMenu()
    {
        ResolveReferences();

        if (wristMenuRoot == null)
            return;

        AttachMenuToWrist();
        wristMenuRoot.SetActive(true);
    }

    public void HideWristMenu()
    {
        ResolveReferences();

        if (wristMenuRoot != null)
            wristMenuRoot.SetActive(false);
    }

    void AttachMenuToWrist()
    {
        if (wristMenuRoot == null || wristAnchor == null)
            return;

        Transform menuTransform = wristMenuRoot.transform;
        if (menuTransform.parent != wristAnchor)
            menuTransform.SetParent(wristAnchor, false);

        menuTransform.localPosition = wristLocalPosition;
        menuTransform.localEulerAngles = wristLocalEulerAngles;
        menuTransform.localScale = wristLocalScale;
    }

    public void LoadSceneByIndex(int index)
    {
        switch (index)
        {
            case 0:
                Debug.Log("0");
                SceneManager.LoadScene("Home");
                break;
            case 1:
                Debug.Log("1");
                string currentSceneName = SceneManager.GetActiveScene().name;
                SceneManager.LoadScene(currentSceneName); // sahneyi yeniden ba�lat
                break;
            case 2:
                Debug.Log("2");
                // Buraya ba�ka bir sahne veya i�lev ekleyebilirsin
                break;
            case 3:
                Debug.Log("��k�� yap�l�yor...");
#if UNITY_EDITOR
                UnityEditor.EditorApplication.isPlaying = false;
#else
                    Application.Quit();
#endif
                break;
            default:
                Debug.Log("Ge�ersiz se�im: " + index);
                break;
        }
    }
}
*/