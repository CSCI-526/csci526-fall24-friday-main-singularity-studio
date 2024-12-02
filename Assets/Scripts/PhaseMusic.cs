using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PhaseMusic : MonoBehaviour
{
    [SerializeField] private AudioSource phaseMusic; // AudioSource for Phase 1 music
    [SerializeField] private float fadeDuration = 2f; // Duration of the fade-out in seconds

    private void Start()
    {
        // Start playing music at the beginning of Phase 1
        if (phaseMusic != null)
        {
            phaseMusic.Play();
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        // Check if the object is the player and is colliding with the EndPhase1 trigger
        if (collision.CompareTag("Player"))
        {
            if (phaseMusic != null && phaseMusic.isPlaying)
            {
                StartCoroutine(FadeOutMusic());
            }
        }
    }

    private IEnumerator FadeOutMusic()
    {
        float startVolume = phaseMusic.volume;

        while (phaseMusic.volume > 0)
        {
            phaseMusic.volume -= startVolume * Time.deltaTime / fadeDuration;
            yield return null;
        }

        phaseMusic.Stop();
        phaseMusic.volume = startVolume; // Reset volume for next use
    }
}
