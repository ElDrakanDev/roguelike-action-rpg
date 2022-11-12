using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.Pool;

namespace Game.Utils
{
    public class SFXManager : MonoBehaviour
    {
        public static SFXManager Instance { get; protected set; }
        [SerializeField] AudioMixerGroup _sfxMixerGroup;
        ObjectPool<AudioSource> _audioPool;

        private void Awake()
        {
            if (Instance == null)
            {
                Instance = this;
                DontDestroyOnLoad(gameObject);
            };
            _audioPool = new ObjectPool<AudioSource>(
            () => {
                var go = new GameObject("Audio Source");
                go.transform.SetParent(transform, true);
                var source = go.AddComponent<AudioSource>();
                source.loop = false;
                source.outputAudioMixerGroup = _sfxMixerGroup;
                source.playOnAwake = false;
                return source;
            }, maxSize: 15
        );
        }

        public static void Play(AudioClip clip, Vector3 position, float pitchRange = 0.25f)
        {
            Instance._audioPool.Get(out AudioSource source);
            source.pitch = Random.Range(1 - pitchRange, 1 + pitchRange);
            source.transform.position = position;
            source.clip = clip;
            source.Play();
        }
    }
}
