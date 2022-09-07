using UnityEngine;
using TMPro;
using UnityEngine.InputSystem;

namespace Game.UI
{
    public class FpsCounter : MonoBehaviour
    {
        [SerializeField] TextMeshProUGUI fpsText;
        [SerializeField] float updateRate = 1f;
        float cooldown = 0;
        float frameCount = 0;
        void Update()
        {
            Keyboard kb = Keyboard.current;

            if (kb != null && kb.f3Key.wasPressedThisFrame)
            {
                cooldown = 0;
                frameCount = 0;
                fpsText.enabled = !fpsText.enabled;
            }

            if (fpsText.enabled)
            {
                cooldown += Time.deltaTime;
                frameCount++;

                if(cooldown > updateRate)
                {
                    int fps = Mathf.RoundToInt(frameCount / updateRate);
                    fpsText.text = $"{fps} FPS";
                    cooldown = 0;
                    frameCount = 0;
                }
            }
        }
    }
}
