using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimationSoundBinder : MonoBehaviour
{
    [SerializeField] private AudioClip clip;
    [SerializeField] private float volume;

    public void Step()
    {
        Tars.Audio.AudioManager.PlaySound(clip, transform.position, volume);
    }
}
