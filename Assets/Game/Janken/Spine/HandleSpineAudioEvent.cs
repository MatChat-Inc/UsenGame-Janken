using Luna;
using UnityEngine;

public class HandleSpineAudioEvent : MonoBehaviour
{
    public void Play(AudioClip audioClip)
    {
        SFXManager.Play(audioClip);
    }
}
