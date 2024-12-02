using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PhaseMusic : MonoBehaviour
{
    public AudioClip phase1Music; // AudioClip for Phase 1 music
    public AudioClip phase2Music; // AudioClip for Phase 2 music
    public AudioClip phase3Music; // AudioClip for Phase 3 music
    public float fadeDuration = 2f; // Duration of the fade-out and fade-in in seconds
    private AudioSource audioSource;

    private void Start()
    {
        // Ensure there is an AudioSource component attached to this GameObject
        audioSource = GetComponent<AudioSource>();
        if (audioSource == null)
        {
            audioSource = gameObject.AddComponent<AudioSource>();
        }

        // Start playing Phase 1 music
        if (phase1Music != null)
        {
            audioSource.clip = phase1Music;
            audioSource.Play();
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        // Transition to Phase 2
        if (collision.gameObject.name == "EndPhase1")
        {
            if (audioSource.isPlaying && phase2Music != null)
            {
                StartCoroutine(TransitionMusic(phase2Music));
            }
        }

        // Transition to Phase 3
        if (collision.gameObject.name == "EndPhase2")
        {
            if (audioSource.isPlaying && phase3Music != null)
            {
                StartCoroutine(TransitionMusic(phase3Music));
            }
        }
    }

    private IEnumerator TransitionMusic(AudioClip newMusic)
    {
        // Fade out current music
        float startVolume = audioSource.volume;
        while (audioSource.volume > 0)
        {
            audioSource.volume -= startVolume * Time.deltaTime / fadeDuration;
            yield return null;
        }

        audioSource.Stop();
        audioSource.clip = newMusic;

        // Fade in new music
        audioSource.Play();
        while (audioSource.volume < startVolume)
        {
            audioSource.volume += startVolume * Time.deltaTime / fadeDuration;
            yield return null;
        }
    }
}
