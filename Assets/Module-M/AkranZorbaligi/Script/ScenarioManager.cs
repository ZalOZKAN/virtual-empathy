using System.Collections;
using UnityEngine;

public class ScenarioManager : MonoBehaviour
{
    [Header("Kameralar")]
    public GameObject mainCamera;
    public GameObject victimCamera;

    [Header("Butonlar")]
    public GameObject startTask1Button;
    public GameObject stopBullyButton;
    public GameObject startScene2Button;
    public GameObject startScene3Button;

    [Header("Objeler")]
    public GameObject bullies;
    public GameObject confirm1;
    public GameObject confirm2;
    public GameObject confirm3;

    [Header("2. Sahne Objeleri")]
    public GameObject[] badWords;
    public GameObject brokenHeart;

    [Header("3. Sahne Objeleri")]
    public GameObject[] goodWords;
    public GameObject redHeart;

    [Header("Sesler - Zorbalar")]
    public AudioSource bullyMaleAudioSource;
    public AudioSource bullyFemaleAudioSource;

    [Header("Sesler - Maðdur")]
    public AudioSource victimVoiceAudioSource;
    public AudioSource victimHeartbeatAudioSource;

    [Header("Ses Seviyeleri")]
    public float bullyNormalVolume = 0.7f;
    public float bullyLowVolume = 0.2f;
    public float victimVoiceVolume = 1f;
    public float victimHeartbeatVolume = 0.25f;

    private int destroyedBadWords = 0;
    private int destroyedGoodWords = 0;

    private int totalBadWords = 7;
    private int totalGoodWords = 7;

    private bool victimSequenceStarted = false;

    void Start()
    {
        if (victimCamera != null) victimCamera.SetActive(false);
        if (stopBullyButton != null) stopBullyButton.SetActive(false);
        if (startScene2Button != null) startScene2Button.SetActive(false);
        if (startScene3Button != null) startScene3Button.SetActive(false);
        if (confirm1 != null) confirm1.SetActive(false);
        if (confirm2 != null) confirm2.SetActive(false);
        if (confirm3 != null) confirm3.SetActive(false);
        if (redHeart != null) redHeart.SetActive(false);

        if (brokenHeart != null) brokenHeart.SetActive(false);

        foreach (GameObject obj in badWords)
        {
            if (obj != null) obj.SetActive(false);
        }

        foreach (GameObject obj in goodWords)
        {
            if (obj != null) obj.SetActive(false);
        }

        if (victimVoiceAudioSource != null)
        {
            victimVoiceAudioSource.volume = victimVoiceVolume;
            victimVoiceAudioSource.Stop();
        }

        if (victimHeartbeatAudioSource != null)
        {
            victimHeartbeatAudioSource.volume = victimHeartbeatVolume;
            victimHeartbeatAudioSource.Stop();
        }

        StartBullyVoices();
    }

    public void StartTask1()
    {
        if (startTask1Button != null)
            startTask1Button.SetActive(false);
    }

    public void ActivateVictimCamera()
    {
        if (victimSequenceStarted) return;
        victimSequenceStarted = true;

        StartCoroutine(CameraRoutine());
    }

    private IEnumerator CameraRoutine()
    {
        if (mainCamera != null) mainCamera.SetActive(false);
        if (victimCamera != null) victimCamera.SetActive(true);

        LowerBullyVoices();
        StartVictimVoices();

        yield return new WaitForSeconds(30f);

        StopVictimVoices();
        RestoreBullyVoices();

        if (victimCamera != null) victimCamera.SetActive(false);
        if (mainCamera != null) mainCamera.SetActive(true);

        if (stopBullyButton != null)
            stopBullyButton.SetActive(true);
    }

    public void StopBullying()
    {
        StopBullyVoices();

        if (bullies != null) bullies.SetActive(false);
        if (stopBullyButton != null) stopBullyButton.SetActive(false);
        if (confirm1 != null) confirm1.SetActive(true);
        if (startScene2Button != null) startScene2Button.SetActive(true);
    }

    public void StartScene2()
    {
        if (startScene2Button != null)
            startScene2Button.SetActive(false);

        if (brokenHeart != null)
            brokenHeart.SetActive(true);

        foreach (GameObject obj in badWords)
        {
            if (obj != null) obj.SetActive(true);
        }
    }

    public void BadWordDestroyed()
    {
        destroyedBadWords++;

        if (destroyedBadWords >= totalBadWords)
        {
            if (confirm2 != null) confirm2.SetActive(true);
            if (startScene3Button != null) startScene3Button.SetActive(true);
        }
    }

    public void StartScene3()
    {
        if (startScene3Button != null)
            startScene3Button.SetActive(false);

        foreach (GameObject obj in goodWords)
        {
            if (obj != null) obj.SetActive(true);
        }
    }

    public void GoodWordCompleted()
    {
        destroyedGoodWords++;

        if (destroyedGoodWords >= totalGoodWords)
        {
            if (redHeart != null) redHeart.SetActive(true);
            if (confirm3 != null) confirm3.SetActive(true);
        }
    }

    private void StartBullyVoices()
    {
        if (bullyMaleAudioSource != null)
        {
            bullyMaleAudioSource.volume = bullyNormalVolume;
            if (!bullyMaleAudioSource.isPlaying)
                bullyMaleAudioSource.Play();
        }

        if (bullyFemaleAudioSource != null)
        {
            bullyFemaleAudioSource.volume = bullyNormalVolume;
            if (!bullyFemaleAudioSource.isPlaying)
                bullyFemaleAudioSource.Play();
        }
    }

    private void StopBullyVoices()
    {
        if (bullyMaleAudioSource != null && bullyMaleAudioSource.isPlaying)
            bullyMaleAudioSource.Stop();

        if (bullyFemaleAudioSource != null && bullyFemaleAudioSource.isPlaying)
            bullyFemaleAudioSource.Stop();
    }

    private void LowerBullyVoices()
    {
        if (bullyMaleAudioSource != null)
            bullyMaleAudioSource.volume = bullyLowVolume;

        if (bullyFemaleAudioSource != null)
            bullyFemaleAudioSource.volume = bullyLowVolume;
    }

    private void RestoreBullyVoices()
    {
        if (bullyMaleAudioSource != null)
            bullyMaleAudioSource.volume = bullyNormalVolume;

        if (bullyFemaleAudioSource != null)
            bullyFemaleAudioSource.volume = bullyNormalVolume;
    }

    private void StartVictimVoices()
    {
        if (victimVoiceAudioSource != null)
        {
            victimVoiceAudioSource.volume = victimVoiceVolume;
            victimVoiceAudioSource.Play();
        }

        if (victimHeartbeatAudioSource != null)
        {
            victimHeartbeatAudioSource.volume = victimHeartbeatVolume;
            victimHeartbeatAudioSource.Play();
        }
    }

    private void StopVictimVoices()
    {
        if (victimVoiceAudioSource != null && victimVoiceAudioSource.isPlaying)
            victimVoiceAudioSource.Stop();

        if (victimHeartbeatAudioSource != null && victimHeartbeatAudioSource.isPlaying)
            victimHeartbeatAudioSource.Stop();
    }
}