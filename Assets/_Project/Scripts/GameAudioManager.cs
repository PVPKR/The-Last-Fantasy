using UnityEngine;

public class GameAudioManager : MonoBehaviour
{
    [Header("Audio Sources")]
    [SerializeField] private AudioSource bgmSource;
    [SerializeField] private AudioSource sfxSource;
    [SerializeField] private AudioSource footstepSource;

    [Header("Audio Clips")]
    [SerializeField] private AudioClip backgroundMusic;
    [SerializeField] private AudioClip buttonClickSound;
    [SerializeField] private AudioClip footstepSound;
    [SerializeField] private AudioClip attackSound;

    private void Start()
    {
        PlayBackgroundMusic();
    }

    private void PlayBackgroundMusic()
    {
        if (bgmSource == null || backgroundMusic == null)
            return;

        bgmSource.clip = backgroundMusic;
        bgmSource.loop = true;

        if (!bgmSource.isPlaying)
        {
            bgmSource.Play();
        }
    }

    public void PlayButtonClick()
    {
        if (sfxSource == null || buttonClickSound == null)
            return;

        sfxSource.PlayOneShot(buttonClickSound);
    }

    public void PlayAttackSound()
    {
        if (sfxSource == null || attackSound == null)
            return;

        sfxSource.PlayOneShot(attackSound);
    }

    public void StartFootstepSound()
    {
        if (footstepSource == null || footstepSound == null)
            return;

        if (footstepSource.isPlaying)
            return;

        footstepSource.clip = footstepSound;
        footstepSource.loop = true;
        footstepSource.Play();
    }

    public void StopFootstepSound()
    {
        if (footstepSource == null)
            return;

        if (footstepSource.isPlaying)
        {
            footstepSource.Stop();
        }
    }
}