using UnityEngine;
using System;

namespace Tars.Audio
{
    [Serializable]
    public class AudioTrack
    {
        public SoundType soundType;
        public AudioClip audioClip;
        [Range(0, 1)] public float volume = 1f;
    }
}