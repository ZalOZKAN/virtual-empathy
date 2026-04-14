using UnityEngine;

public class TrashTrigger : MonoBehaviour
{
    public ScenarioManager manager;
    public AudioClip trashEffectClip;
    [Range(0f, 1f)] public float trashEffectVolume = 1f;

    private void OnTriggerEnter(Collider other)
    {
        GameObject hitObject = other.attachedRigidbody != null
            ? other.attachedRigidbody.gameObject
            : other.gameObject;

        if (hitObject.CompareTag("KotuSoz") && hitObject.activeSelf)
        {
            if (trashEffectClip != null)
            {
                AudioSource.PlayClipAtPoint(
                    trashEffectClip,
                    hitObject.transform.position,
                    trashEffectVolume
                );
            }

            if (manager != null)
            {
                manager.BadWordDestroyed();
            }

            hitObject.SetActive(false);
        }
    }
}