using UnityEngine;
using UnityEngine.UI;

public class PresentationController : MonoBehaviour
{
    public Sprite[] pages;
    public Image displayImage;
    private int currentPage = 0;    

    void OnEnable()
    {
        currentPage = 0;
        ShowPage(0);
    }

    public void NextPage()
    {
        if (currentPage < pages.Length - 1)
            ShowPage(++currentPage);
    }

    public void PrevPage()
    {
        if (currentPage > 0)
            ShowPage(--currentPage);
    }

    public void Close()
    {
        // if (FirebaseManager.Instance != null)
        //     FirebaseManager.Instance.LogPresentationClosed();
        // gameObject.SetActive(false);
    }

    void ShowPage(int index)
    {
        if (index < pages.Length)
            displayImage.sprite = pages[index];
    }
}
