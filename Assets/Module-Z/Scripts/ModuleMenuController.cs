using UnityEngine;
using UnityEngine.SceneManagement;

public class ModuleMenuController : MonoBehaviour
{
    public GameObject presentationPanel;
    public GameObject infographicPanel;
    public GameObject quizPanel;

    // Simulasyon Butonu
    public void RestartSimulation()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    // Sunum Butonu
    public void OpenPresentation()
    {   
        gameObject.SetActive(false);
        presentationPanel.SetActive(true);
        // Firebase: sunum acilma zamanini logla
        // if (FirebaseManager.Instance != null)
        //     FirebaseManager.Instance.LogPresentationOpened();
    }

    // Infografik Butonu
    public void OpenInfographic()
    {
        gameObject.SetActive(false);
        infographicPanel.SetActive(true);
    }

    // Sinav Butonu
    public void OpenQuiz()
    {
        gameObject.SetActive(false);
        quizPanel.SetActive(true);
    }
}
