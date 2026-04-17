/*using System.Collections.Generic;
using UnityEngine;
using Firebase.Firestore;

public class FirebaseManager : MonoBehaviour
{
    public static FirebaseManager Instance;

    private FirebaseFirestore db;
    private string sessionId;
    private float presentationOpenTime;
    private int quizCorrect = 0;
    private int quizWrong = 0;
    private string selectedCharacter = "";

    void Awake()
    {
        if (Instance == null) { Instance = this; DontDestroyOnLoad(gameObject); }
        else { Destroy(gameObject); return; }

        sessionId = System.Guid.NewGuid().ToString();
        db = FirebaseFirestore.DefaultInstance;
    }

    public void LogCharacterSelected(string character)
    {
        selectedCharacter = character;
        Debug.Log("[Firebase] Karakter secildi: " + character);
    }

    public void LogPresentationOpened()
    {
        presentationOpenTime = Time.time;
    }

    public void LogPresentationClosed()
    {
        float duration = Time.time - presentationOpenTime;
        UpdateField("presentationDuration", duration);
    }

    public void LogQuizAnswer(bool correct)
    {
        if (correct) quizCorrect++; else quizWrong++;
    }

    public void SaveSession()
    {
        var data = new Dictionary<string, object>
        {
            { "sessionId", sessionId },
            { "character", selectedCharacter },
            { "quizCorrect", quizCorrect },
            { "quizWrong", quizWrong },
            { "timestamp", FieldValue.ServerTimestamp }
        };
        db.Collection("sonder_sessions").AddAsync(data).ContinueWith(task =>
        {
            if (task.IsCompleted) Debug.Log("[Firebase] Session kaydedildi.");
            else Debug.LogError("[Firebase] Hata: " + task.Exception);
        });
    }

    void UpdateField(string key, object value)
    {
        db.Collection("sonder_sessions").Document(sessionId)
          .SetAsync(new Dictionary<string, object> {{ key, value }},
                    SetOptions.MergeAll);
    }
}

*/