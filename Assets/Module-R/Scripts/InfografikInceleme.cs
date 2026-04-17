using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;
using UnityEngine.XR.Interaction.Toolkit.Interactables;

public class InfografikInceleme : MonoBehaviour
{
    public Transform incelemeNoktasi; // Kameranýn tam önünde boþ bir Transform (Inspector'dan sürükle)
    public float buyumeHizi = 1.5f;   // Kullanýcý tuttuðunda ne kadar büyüsün?

    private Vector3 originalScale;
    private Vector3 originalPosition;
    private Quaternion originalRotation;
    private XRGrabInteractable grabInteractable;

    void Start()
    {
        originalScale = transform.localScale;
        originalPosition = transform.position;
        originalRotation = transform.rotation;
        grabInteractable = GetComponent<XRGrabInteractable>();

        // Týklama olaylarýný baðla
        grabInteractable.selectEntered.AddListener(Tuttugunda);
        grabInteractable.selectExited.AddListener(Biraktigunda);
    }

    void Tuttugunda(SelectEnterEventArgs args)
    {
        // Ýnceleme moduna geç: Büyüt ve Öne Getir
        LeanTween.scale(gameObject, originalScale * buyumeHizi, 0.4f).setEaseOutBack();
        LeanTween.move(gameObject, incelemeNoktasi.position, 0.4f).setEaseOutBack();
        LeanTween.rotate(gameObject, incelemeNoktasi.rotation.eulerAngles, 0.4f).setEaseOutBack();

        // Yerçekimini kapat ki havada kalsýn (Eðer Rigidbody varsa)
        if (GetComponent<Rigidbody>() != null)
        {
            GetComponent<Rigidbody>().useGravity = false;
            GetComponent<Rigidbody>().isKinematic = true;
        }
    }

    void Biraktigunda(SelectExitEventArgs args)
    {
        // Eski haline dön
        LeanTween.scale(gameObject, originalScale, 0.4f).setEaseInBack();
        LeanTween.move(gameObject, originalPosition, 0.4f).setEaseInBack();
        LeanTween.rotate(gameObject, originalRotation.eulerAngles, 0.4f).setEaseInBack();

        // Yerçekimini aç (Opsiyonel: Eðer yere düþmesini istiyorsan)
        if (GetComponent<Rigidbody>() != null)
        {
            GetComponent<Rigidbody>().useGravity = true;
            GetComponent<Rigidbody>().isKinematic = false;
        }
    }

    // UPDATE: Kullanýcýnýn infografiði elinde döndürmesini saðlar
    void Update()
    {
        if (grabInteractable.isSelected)
        {
            // Sað elin joystick'ini kullanarak döndürme (Action-based Input ile)
            // Bu kýsma Input Action baðlantýsý yapman gerekebilir ama þu an
            // en basit haliyle kodla döndürelim:
            transform.Rotate(Vector3.up * Time.deltaTime * 30f);
        }
    }
}