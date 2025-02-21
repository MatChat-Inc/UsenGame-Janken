using Luna;
using Luna.Extensions;
using UnityEngine;

public class HandleSpineAudioEvent : MonoBehaviour
{
    public void Play(AudioClip audioClip)
    {
        SFXManager.Play(audioClip);
    }
    
    public void PlayAsync(string address)
    {
        new Asset<AudioClip>(address).Load().Then(Play);
    }
}
