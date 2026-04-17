using UnityEngine;
using XRInputDevice = UnityEngine.XR.InputDevice;
using XRCommonUsages = UnityEngine.XR.CommonUsages;
using UnityEngine.XR;

public class denememenu : MonoBehaviour
{
    private bool wasPressedLastFrame = false;

    void Update()
    {
        XRInputDevice rightHand = InputDevices.GetDeviceAtXRNode(XRNode.RightHand);

        if (rightHand.TryGetFeatureValue(XRCommonUsages.primaryButton, out bool isPressed))
        {
            // GetKeyDown benzeri - Ýlk basýldýðýnda
            if (isPressed && !wasPressedLastFrame)
            {
                Debug.Log("2");
            }

            // GetKey benzeri - Basýlý tutulurken
            if (isPressed)
            {
                Debug.Log("3");
            }

            // GetKeyUp benzeri - Tuþ býrakýldýðýnda
            if (!isPressed && wasPressedLastFrame)
            {
                Debug.Log("4");
            }

            // Durumu güncelle
            wasPressedLastFrame = isPressed;
        }
    }
}
