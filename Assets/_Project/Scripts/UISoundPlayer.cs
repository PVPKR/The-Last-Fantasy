using UnityEngine;

public class UISoundPlayer : MonoBehaviour
{
    [Header("UI Sound")]
    [SerializeField] private AudioSource audioSource;
    [SerializeField] private AudioClip buttonClickSound;

    public void PlayClickSound()
    {
        if (audioSource == null || buttonClickSound == null)
            return;

        audioSource.PlayOneShot(buttonClickSound);
    }
}