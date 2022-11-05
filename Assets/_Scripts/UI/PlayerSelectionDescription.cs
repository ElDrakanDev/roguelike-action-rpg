using TMPro;
using UnityEngine;

namespace Game.UI
{
    public class PlayerSelectionDescription : MonoBehaviour
    {
        [SerializeField] TextMeshProUGUI _textMeshPro;
        [SerializeField] float descriptionWriteInterval = 0.02f;
        float counter = 0;
        public string Text { get => _textMeshPro.text; set => _textMeshPro.text = value; }
        private void Awake()
        {
            if (!_textMeshPro) _textMeshPro = GetComponent<TextMeshProUGUI>();
        }
        private void OnEnable()
        {
            _textMeshPro.maxVisibleCharacters = 0;
        }
        private void Update()
        {
            counter -= Time.unscaledDeltaTime;
            if (counter <= 0)
            {
                _textMeshPro.maxVisibleCharacters++;
                counter = descriptionWriteInterval;
            }
        }
    }
}
