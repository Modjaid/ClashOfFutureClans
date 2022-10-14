using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimationSoundBinderRandom : MonoBehaviour
{
    [SerializeField] private List<AudioClip> clips;
    [SerializeField] private float volume;

    public void Step()
    {
        Tars.Audio.AudioManager.PlaySound(clips[Random.Range(0, clips.Count)], transform.position, volume);
    }
}
