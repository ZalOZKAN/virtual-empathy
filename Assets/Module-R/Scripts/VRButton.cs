using UnityEngine;
using UnityEngine.Events;
using UnityEngine.XR.Interaction.Toolkit;

public class VRButton : MonoBehaviour
{
    public UnityEvent onButtonClick; // Butona basýlýnca olacaklar
    private UnityEngine.XR.Interaction.Toolkit.Interactables.XRSimpleInteractable interactable;

    void Awake()
    {
        // VR Etkileþim bileþenini otomatik ekle veya bul
        interactable = GetComponent<UnityEngine.XR.Interaction.Toolkit.Interactables.XRSimpleInteractable>();
        if (interactable == null)
            interactable = gameObject.AddComponent<UnityEngine.XR.Interaction.Toolkit.Interactables.XRSimpleInteractable>();

        // Lazerle butona týklandýðýnda (Trigger tuþu) çalýþacak olay
        interactable.selectEntered.AddListener(x => OnClicked());
    }

    private void OnClicked()
    {
        Debug.Log("VR Butonuna Lazerle Basýldý!");
        onButtonClick.Invoke();
    }
}