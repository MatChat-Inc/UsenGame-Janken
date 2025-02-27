using Luna;
using Luna.Extensions;
using UnityEngine;

public class HandleSpineAudioEvent : MonoBehaviour
{
    private AudioSource _audioSource;
    
    private void Awake()
    {
        _audioSource = GetComponent<AudioSource>();
    }
    
    public void Play(AudioClip audioClip)
    {
        _audioSource.PlayOneShot(audioClip);
    }
    
    public void PlayAsync(string address)
    {
        new Asset<AudioClip>(address).Load().Then(Play);
    }
}
