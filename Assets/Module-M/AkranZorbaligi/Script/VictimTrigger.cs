using UnityEngine;

public class VictimTrigger : MonoBehaviour
{
    public ScenarioManager manager;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            manager.ActivateVictimCamera();
            gameObject.SetActive(false);
        }
    }
}