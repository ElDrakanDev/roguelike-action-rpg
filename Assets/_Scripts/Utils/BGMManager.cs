using System.Collections;
using UnityEngine;
using DG.Tweening;

namespace Game.Utils
{
    public class BGMManager : MonoBehaviour
    {
        public static BGMManager Instance { get; protected set; }
        [SerializeField] float _fadeTime = 0.5f;
        [SerializeField] AudioSource _source;
        [SerializeField] AudioClip[] _testclips;

        private void Awake()
        {
            if (Instance == null)
            {
                Instance = this;
                DontDestroyOnLoad(gameObject);
            };
            _source.priority = 0; // Max priority
        }
        private void Start()
        {
            Play(_testclips);
        }
        public void Play(AudioClip[] clips, bool useFade = true)
        {
            if(useFade is false)
            {
                _source.Stop();
                _source.loop = false;
                StopCoroutine("PlayClipSequence");
                StartCoroutine(PlayClipSequence(clips));
            }
            else
            {
                DOVirtual.Float(1, 0, _fadeTime * 0.5f, volume => _source.volume = volume)
                    .OnComplete(() => {
                        StopCoroutine("PlayClipSequence");
                        _source.Stop();
                        _source.loop = false;
                        StartCoroutine(PlayClipSequence(clips));
                        DOVirtual.Float(0, 1, _fadeTime * 0.5f, volume => _source.volume = volume);
                    });
            }
        }

        IEnumerator PlayClipSequence(AudioClip[] clips)
        {
            yield return null;

            //1.Loop through each AudioClip
            int i = 0;
            while(true)
            {
                //2.Assign current AudioClip to audiosource
                _source.clip = clips[i % clips.Length];

                //3.Play Audio
                _source.Play();

                //4.Wait for it to finish playing
                while (_source.isPlaying)
                {
                    yield return null;
                }
                i++;
                //5. Go back to #2 and play the next audio in the adClips array
            }
        }
    }
}
