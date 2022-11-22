using UnityEngine;
using UnityEngine.UI;

namespace Game.UI
{
    public class LayeredBar : MonoBehaviour
    {
        [SerializeField] Image[] _fillers;
        [SerializeField, Range(0.1f, 0.9f)] float _lerpSpeed;
        [SerializeField, Range(0, 0.9f)] float _lerpDecay = 0.5f;

        public void UpdateFill(float newValue)
        {
            float lerpSpeed = _lerpSpeed;
            foreach(var fill in _fillers)
            {
                fill.fillAmount = Mathf.Lerp(fill.fillAmount, newValue, lerpSpeed);
                lerpSpeed *= _lerpDecay;
            }
        }
    }
}
