using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;

public class MusicManager : MonoBehaviour
{
    [SerializeField] private AudioSource m_audio;

    public AudioClip MainMenuSong;
    public AudioClip Level1CinematicSong;
    public AudioClip Level1Song;
    public AudioClip Level2CinematicSong;
    public AudioClip Level2Song;
    public AudioClip Level3CinematicSong;
    public AudioClip Level3Song;
    public AudioClip EndCinematicSong;
    
    public static MusicManager Instance { get; private set; }
    
    private void Awake()
    {
        if (Instance != null)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject);
        
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    private void OnDisable()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        switch (scene.name)
        {
            case "MainMenu":
                PlayMusic(MainMenuSong);
                break;
            case "Cinematic1":
                PlayMusic(Level1CinematicSong);
                break;
            case "Level1":
                PlayMusic(Level1Song);
                break;
            case "Cinematic2":
                PlayMusic(Level2CinematicSong);
                break;
            case "Level2":
                PlayMusic(Level2Song);
                break;
            case "Cinematic3":
                PlayMusic(Level3CinematicSong);
                break;
            case "Level3":
                PlayMusic(Level3Song);
                break;
            case "Cinematic4":
                PlayMusic(EndCinematicSong);
                break;
        }
    }

    // ReSharper disable Unity.PerformanceAnalysis
    public void FadeOutMusic(float fadeTime = 0.18f) // Default fade out time is 2 seconds
    {
        // Don't Fade out the last cinematic song. Keep it going for the credits
        if (SceneManager.GetActiveScene().name == "EndSceneName")
        {
            return;
        }
        
        if (m_audio == null)
        {
            Debug.LogError("AudioSource is not assigned in FadeOutMusic");
            return;
        }

        if (m_audio.isPlaying)
        {
            StartCoroutine(FadeOut(m_audio, fadeTime));
        }
        else
        {
            Debug.Log("No music is playing to fade out.");
        }
    }

    private IEnumerator FadeOut(AudioSource audioSource, float fadeTime)
    {
        float startVolume = 0.15f;

        while (audioSource.volume > 0)
        {
            audioSource.volume -= startVolume * Time.deltaTime / fadeTime;

            yield return null;
        }

        audioSource.Stop();
        audioSource.volume = 0; // Reset volume for next play
    }

    public void PlayMusic(AudioClip clip)
    {
        Debug.Log("Play Music Clip: " + clip.name);
        if (clip == null)
        {
            Debug.LogError("No AudioClip provided to PlayMusic");
            return;
        }

        if (m_audio == null)
        {
            Debug.LogError("AudioSource is not assigned in PlayMusic");
            return;
        }

        if (!Application.isPlaying) return;

        m_audio.clip = clip;
        m_audio.Play();
        StartCoroutine(FadeIn(m_audio, 0.18f)); // 2 seconds fade in
    }

    private IEnumerator FadeIn(AudioSource audioSource, float fadeTime)
    {
        float startVolume = 0.5f;

        audioSource.volume = 0;
        while (audioSource.volume < 0.3f)
        {
            audioSource.volume += startVolume * Time.deltaTime / fadeTime;

            yield return null;
        }

        audioSource.volume = 0.15f;
    }
}
