using DG.Tweening;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.Pool;

namespace Game.Utils
{
    public class SFXManager : MonoBehaviour
    {
        public static SFXManager Instance { get; protected set; }
        [SerializeField] AudioMixerGroup _sfxMixerGroup;
        ObjectPool<SoundEffect> _audioPool = null;

        private void Awake()
        {
            if (Instance == null)
            {
                Instance = this;
                DontDestroyOnLoad(gameObject);
            };
            _audioPool = new ObjectPool<SoundEffect>(
            () => {
                var go = new GameObject("Sound Effect");
                go.transform.SetParent(transform, true);
                var effect = go.AddComponent<SoundEffect>();
                var source = effect.Source;
                source.loop = false;
                source.outputAudioMixerGroup = _sfxMixerGroup;
                source.playOnAwake = false;
                return effect;
            },
            actionOnGet: (effect) => effect.OnPoolGet(),
            defaultCapacity: 15,
            maxSize: 15
        );
        }

        public static void Play(AudioClip clip, Vector3 position, float pitchRange = 0.25f)
        {
            Instance._audioPool.Get(out SoundEffect effect);
            var source = effect.Source;
            source.pitch = Random.Range(1 - pitchRange, 1 + pitchRange);
            source.transform.position = position;
            source.clip = clip;
            source.Play();
            Instance.StartCoroutine(CoroutineUtils.DelayedCall(clip.length, () => Instance._audioPool.Release(effect)));
        }
    }

    public class SoundEffect : MonoBehaviour
    {
        const int BASE_PRIORITY = 127;
        public AudioSource Source { get; private set; }
        private void Awake() => Source = gameObject.AddComponent<AudioSource>();

        private void Update() => Source.priority++;
        public void OnPoolGet()
        {
            Source.priority = BASE_PRIORITY;
        }

    }
}
