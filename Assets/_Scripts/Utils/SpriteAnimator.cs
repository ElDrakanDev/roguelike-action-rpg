using UnityEngine;

namespace Game.Utils
{
    public class SpriteAnimator : MonoBehaviour
    {
        [SerializeField] SpriteRenderer spriteRenderer;
        [SerializeField] SpriteAnimationSO currentAnimation;
        public SpriteAnimationSO Current { get => currentAnimation; }
        public float timeScale = 1f;
        float cycleCounter = 0;
        int _frame = 0;
        int Frame {
            get => _frame;
            set
            {
                if(value >= 0 && value < currentAnimation.sprites.Length)
                    spriteRenderer.sprite = currentAnimation.sprites[value];
                _frame = value;
                cycleCounter = 0;
            }
        }

        private void Awake()
        {
            if (spriteRenderer == null)
                spriteRenderer = GetComponent<SpriteRenderer>();
        }

        // Update is called once per frame
        void Update()
        {
            cycleCounter += Time.deltaTime * timeScale;
            if(cycleCounter > currentAnimation.cycleSeconds)
            {
                NextFrame();
            }
        }

        public void SetAnimation(SpriteAnimationSO animation)
        {
            if(animation != currentAnimation)
            {
                currentAnimation = animation;
                Frame = 0;
                spriteRenderer.sprite = animation.sprites[0];
                Debug.Log(animation);
            }
        }

        void NextFrame()
        {
            Frame++;
            if(Frame >= currentAnimation.sprites.Length)
            {
                if (currentAnimation.looping)
                {
                    Frame = 0;
                }
                else if (currentAnimation.nextAnimation is not null)
                {
                    SetAnimation(currentAnimation.nextAnimation);
                }
                else
                {
                    Frame = 0;
                    Debug.LogError($"No se pudo ir al siguiente frame porque no existía, reiniciando bucle.");
                }
            }
        }
    }
}
