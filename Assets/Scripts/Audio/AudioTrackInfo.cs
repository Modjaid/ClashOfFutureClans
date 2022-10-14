using UnityEngine;

namespace Tars.Audio
{
    public class AudioTrackInfo
    {
        public AudioTrackInfo(GameObject linkedGameObject, AudioTrack audioTrack, AudioSource audioSource)
        {
            this.linkedGameObject = linkedGameObject;
            this.audioTrack = audioTrack;
            this.audioSource = audioSource;
        }

        public GameObject linkedGameObject;
        public AudioTrack audioTrack;
        public AudioSource audioSource;
    }
}