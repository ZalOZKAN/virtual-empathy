using UnityEngine;
using TMPro;
using UnityEngine.UI;

[System.Serializable]
public class QuizQuestion
{
    public string questionText;
    public string[] options; // 3 secenek
    public int correctIndex; // Dogru secenek indexi (0, 1 veya 2)
    [TextArea(1, 3)]
    public string explanation;
}

public class QuizManager : MonoBehaviour
{
        public QuizQuestion[] questions;
    public TextMeshProUGUI questionDisplay;
    public Button[] optionButtons;
    public TextMeshProUGUI feedbackText;

    private int currentIndex = 0;
    private int score = 0;

    void OnEnable() { currentIndex = 0; score = 0; ShowQuestion(0); }

    void ShowQuestion(int idx)
    {
        feedbackText.text = "";
        QuizQuestion q = questions[idx];
        questionDisplay.text = (idx + 1) + ". " + q.questionText;
        for (int i = 0; i < optionButtons.Length; i++)
        {
            int capture = i;
            bool active = i < q.options.Length;
            optionButtons[i].gameObject.SetActive(active);
            if (active)
            {
                optionButtons[i].GetComponentInChildren<TextMeshProUGUI>().text
                    = q.options[i];
                optionButtons[i].onClick.RemoveAllListeners();
                optionButtons[i].onClick.AddListener(() => Answer(capture));
                optionButtons[i].interactable = true;
            }
        }
    }

    void Answer(int idx)
    {
        foreach (var b in optionButtons) b.interactable = false;
        QuizQuestion q = questions[currentIndex];
        bool correct = idx == q.correctIndex;
        if (correct) { score++; feedbackText.text = "✓ Doğru! " + q.explanation; }
        else feedbackText.text = "✗ Yanlış. " + q.explanation;

        // if (FirebaseManager.Instance != null)
        //     FirebaseManager.Instance.LogQuizAnswer(correct);

        currentIndex++;
        if (currentIndex < questions.Length) Invoke(nameof(NextQ), 2.5f);
        else Invoke(nameof(ShowResult), 2.5f);
    }

    void NextQ() { ShowQuestion(currentIndex); }

    void ShowResult()
    {
        // questionDisplay.text = "Tamamlandı! " + score + " / " + questions.Length + " doğru.";
        // foreach (var b in optionButtons) b.gameObject.SetActive(false);
        // if (FirebaseManager.Instance != null)
        //     FirebaseManager.Instance.SaveSession();
    }
}
