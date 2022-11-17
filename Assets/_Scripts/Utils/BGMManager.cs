using System.Collections;
using UnityEngine;
using DG.Tweening;

namespace Game.Utils
{
    public class BGMManager : MonoBehaviour
    {
        public static BGMManager Instance { get; protected set; }
        [SerializeField] float _fadeTime = 0.3f;
        [field:SerializeField] public AudioSource Source { get; protected set; }
        AudioClip[] currentClips;

        private void Awake()
        {
            if (Instance == null)
            {
                Instance = this;
                DontDestroyOnLoad(gameObject);
            };
            Source.priority = 0; // Max priority
        }

        public void Play(AudioClip[] clips, bool useFade = true)
        {
            if (clips == currentClips) return;
            if(useFade is false)
            {
                Source.Stop();
                Source.loop = false;
                StopCoroutine("PlayClipSequence");
                StartCoroutine(PlayClipSequence(clips));
            }
            else
            {
                DOVirtual.Float(1, 0, _fadeTime * 0.5f, volume => Source.volume = volume)
                    .OnComplete(() => {
                        StopCoroutine("PlayClipSequence");
                        Source.Stop();
                        Source.loop = false;
                        StartCoroutine(PlayClipSequence(clips));
                        DOVirtual.Float(0, 1, _fadeTime * 0.5f, volume => Source.volume = volume);
                    });
            }
            currentClips = clips;
        }

        IEnumerator PlayClipSequence(AudioClip[] clips)
        {
            yield return null;

            //1.Loop through each AudioClip
            int i = 0;
            while(true)
            {
                //2.Assign current AudioClip to audiosource
                Source.clip = clips[i % clips.Length];

                //3.Play Audio
                Source.Play();

                //4.Wait for it to finish playing
                while (Source.isPlaying)
                {
                    yield return null;
                }
                i++;
                //5. Go back to #2 and play the next audio in the adClips array
            }
        }
    }
}
