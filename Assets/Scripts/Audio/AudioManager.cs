using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Tars.Audio
{
    public class AudioManager : MonoBehaviour
    {
        public static readonly List<AudioSource> audioSources = new List<AudioSource>();
        public static readonly Dictionary<(int x, int y, int z), AudioSource> audioDictionary = new Dictionary<(int x, int y, int z), AudioSource>();
        private static GameObject container;
        private static GameObject Container
        {
            get
            {
                if (container == null)
                    container = new GameObject("AudioPull");
                return container;
            }
        }

        public static void PlaySound(AudioClip clip, GameObject player, float volume = 0.5f)
        {
            AudioSource source = null;
            for (int i = 0; i < audioSources.Count; i++)
            {
                if (!audioSources[i].isPlaying)
                {
                    source = audioSources[i];
                    break;
                }
            }
            if(source == null)
            {
                source = new GameObject("AudioSource").AddComponent<AudioSource>();
                source.spatialBlend = 0.7f;
                source.transform.SetParent(Container.transform);
                audioSources.Add(source);
            }
	    source.volume = volume;
            source.transform.position = player.transform.position;
            source.PlayOneShot(clip);
        }

        public static void PlaySoundNear(AudioClip clip, GameObject player, float volume = 0.5f)
        {
            AudioSource source = null;
            var rPos = VecToTuple(player.transform.position);
            if (audioDictionary.ContainsKey(rPos))
            {
                audioDictionary[rPos].volume = volume;
                audioDictionary[rPos].PlayOneShot(clip);
                return;
            }
            else
            {
                foreach (var item in audioDictionary)
                {
                    if (!item.Value.isPlaying)
                    {
                        source = item.Value;
                        break;
                    }
                }
            }
            if (source == null)
            {
                source = new GameObject("AudioSource").AddComponent<AudioSource>();
                source.spatialBlend = 0.7f;
                source.transform.SetParent(Container.transform);
            }
            else
            {
                audioDictionary.Remove(VecToTuple(source.transform.position));
            }
	    source.volume = volume;
            source.transform.position = player.transform.position;
            audioDictionary.Add(rPos, source);
            source.PlayOneShot(clip);
        }

        public static void PlaySoundNear(AudioClip clip, Vector3 player, float volume = 0.5f)
        {
            AudioSource source = null;
            var rPos = VecToTuple(player);
            if (audioDictionary.ContainsKey(rPos))
            {
	    	audioDictionary[rPos].volume = volume;
                audioDictionary[rPos].PlayOneShot(clip);
                return;
            }
            else
            {
                foreach (var item in audioDictionary)
                {
                    if (!item.Value.isPlaying)
                    {
                        source = item.Value;
                        break;
                    }
                }
            }
            if (source == null)
            {
                source = new GameObject("AudioSource").AddComponent<AudioSource>();
                source.spatialBlend = 0.7f;
                source.transform.SetParent(Container.transform);
            }
            else
            {
                audioDictionary.Remove(VecToTuple(source.transform.position));
            }
	    source.volume = volume;
            source.transform.position = player;
            audioDictionary.Add(rPos, source);
            source.PlayOneShot(clip);
        }

        public static void PlaySound(AudioClip clip, Vector3 player, float volume = 0.5f)
        {
            AudioSource source = null;
            for (int i = 0; i < audioSources.Count; i++)
            {
                if (!audioSources[i].isPlaying)
                {
                    source = audioSources[i];
                    break;
                }
            }
            if (source == null)
            {
                source = new GameObject("AudioSource").AddComponent<AudioSource>();
                source.spatialBlend = 0.7f;
                source.transform.SetParent(Container.transform);
                audioSources.Add(source);
            }
	    source.volume = volume;
            source.transform.position = player;
            source.PlayOneShot(clip);
        }

        private static (int x,int y, int z) VecToTuple(Vector3 vec)
        {
            var intVec = Vector3Int.RoundToInt(vec * 4);
            return (intVec.x, intVec.y, intVec.z);
        }

        private void Update()
        {
            int c = 0;
            for (int i = 0; i < audioSources.Count; i++)
            {
                if (audioSources[i].isPlaying)
                    c++;
            }
            //Debug.Log("Playing Count is " + c.ToString());
        }

	private void OnDestroy()
	{
	    audioSources.Clear();
	    audioDictionary.Clear();
	}
    }
}