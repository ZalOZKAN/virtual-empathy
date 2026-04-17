using UnityEngine;
using UnityEngine.SceneManagement;

/// <summary>
/// Son panel — kullanıcı sahneyi kapatır veya ana menüye döner.
/// </summary>
public class ExitController : MonoBehaviour
{
    [Header("Sahne Ayarları")]
    [Tooltip("Ana menü sahnesinin adı (boş bırakırsan uygulamayı kapatır)")]
    public string mainMenuSceneName = "";

    public void OnExitPressed()
    {
        if (!string.IsNullOrEmpty(mainMenuSceneName))
            SceneManager.LoadScene(mainMenuSceneName);
        else
            Application.Quit();

#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#endif
    }

    public void OnRestartPressed()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
}