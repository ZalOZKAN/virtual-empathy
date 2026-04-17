using UnityEngine;

public class HeartTrigger : MonoBehaviour
{
    public ScenarioManager manager;
    public AudioClip heartEffectClip;
    [Range(0f, 1f)] public float heartEffectVolume = 1f;

    private void OnTriggerEnter(Collider other)
    {
        GameObject hitObject = other.attachedRigidbody != null
            ? other.attachedRigidbody.gameObject
            : other.gameObject;

        if (hitObject.CompareTag("IyiSoz") && hitObject.activeSelf)
        {
            if (heartEffectClip != null)
            {
                AudioSource.PlayClipAtPoint(
                    heartEffectClip,
                    hitObject.transform.position,
                    heartEffectVolume
                );
            }

            if (manager != null)
            {
                manager.GoodWordCompleted();
            }

            hitObject.SetActive(false);
        }
    }
}