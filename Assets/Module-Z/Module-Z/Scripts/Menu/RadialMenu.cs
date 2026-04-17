using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using UnityEngine.InputSystem;
using UnityEngine.XR;
using XRInputDevice = UnityEngine.XR.InputDevice;
using XRCommonUsages = UnityEngine.XR.CommonUsages;

public class RadialMenu : MonoBehaviour
{
    [Range(2, 10)]
    public int numberofRadialPart;
    public GameObject radialPart;
    public Transform radialCanvas;
    public float anglebeetwen = 10;
    private List<GameObject> SppawnedParts = new List<GameObject>();
    public Transform handTransform;
    private bool wasPressedLastFrame = false;

    [Header("Input")]
    [SerializeField] private bool usePrimaryButton = true;
    [SerializeField] private bool useMenuButton = true;
    [SerializeField] private bool useSecondaryButton = false;
    [SerializeField] private bool enableKeyboardFallback = true;
    [SerializeField] private Key keyboardFallbackKey = Key.O;

    private int currentSelectedRadialPart = -1;
    public UnityEvent<int> OnPartSelected;

    void Start() { }

    public void GetSelectedRadialPart()
    {
        Vector3 centerToHand = handTransform.position - radialCanvas.position;
        Vector3 centerToHandProjected = Vector3.ProjectOnPlane(centerToHand, radialCanvas.forward);

        float angle = Vector3.SignedAngle(radialCanvas.up, centerToHandProjected, -radialCanvas.forward);
        if (angle < 0) angle += 360;

        currentSelectedRadialPart = (int)(angle * numberofRadialPart / 360);

        for (int i = 0; i < SppawnedParts.Count; i++)
        {
            if (i == currentSelectedRadialPart)
            {
                SppawnedParts[i].GetComponent<Image>().color = Color.yellow;
                SppawnedParts[i].transform.localScale = 1.1f * Vector3.one;
            }
            else
            {
                SppawnedParts[i].GetComponent<Image>().color = Color.white;
                SppawnedParts[i].transform.localScale = 1f * Vector3.one;
            }
        }
    }

    void Update()
    {
        XRInputDevice rightHand = InputDevices.GetDeviceAtXRNode(XRNode.RightHand);
        bool isPressed = IsVrPressingMenu(rightHand);

        if (enableKeyboardFallback && Keyboard.current != null)
        {
            if (Keyboard.current[keyboardFallbackKey].isPressed)
                isPressed = true;
        }

        if (isPressed && !wasPressedLastFrame)
        {
            SpawnRadialPart();
        }

        if (isPressed)
        {
            GetSelectedRadialPart();
        }

        if (!isPressed && wasPressedLastFrame)
        {
            HideAndTriggerSelected();
        }

        wasPressedLastFrame = isPressed;
    }

    bool IsVrPressingMenu(XRInputDevice rightHand)
    {
        if (!rightHand.isValid)
            return false;

        bool isPressed = false;

        if (usePrimaryButton && rightHand.TryGetFeatureValue(XRCommonUsages.primaryButton, out bool primaryPressed) && primaryPressed)
            isPressed = true;

        if (useMenuButton && rightHand.TryGetFeatureValue(XRCommonUsages.menuButton, out bool menuPressed) && menuPressed)
            isPressed = true;

        if (useSecondaryButton && rightHand.TryGetFeatureValue(XRCommonUsages.secondaryButton, out bool secondaryPressed) && secondaryPressed)
            isPressed = true;

        return isPressed;
    }

    public void HideAndTriggerSelected()
    {
        OnPartSelected.Invoke(currentSelectedRadialPart);
        radialCanvas.gameObject.SetActive(false);
    }

    public void SpawnRadialPart()
    {
        radialCanvas.gameObject.SetActive(true);
        radialCanvas.position = handTransform.position;
        radialCanvas.rotation = handTransform.rotation;

        foreach (var part in SppawnedParts)
        {
            Destroy(part);
        }
        SppawnedParts.Clear();

        for (int i = 0; i < numberofRadialPart; i++)
        {
            float angle = -i * 360 / numberofRadialPart + anglebeetwen / 2;
            Vector3 radialPartEualerAngle = new Vector3(0, 0, angle);

            GameObject spawnedRadialPart = Instantiate(radialPart, radialCanvas);
            spawnedRadialPart.transform.position = radialCanvas.position;
            spawnedRadialPart.transform.localEulerAngles = radialPartEualerAngle;
            spawnedRadialPart.GetComponent<Image>().fillAmount =
                (1 / (float)numberofRadialPart) - (anglebeetwen / 360);

            SppawnedParts.Add(spawnedRadialPart);
        }
    }
}
