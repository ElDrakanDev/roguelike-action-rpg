using TMPro;
using UnityEngine;

namespace Game.UI
{
    public class PlayerSelectionName : MonoBehaviour
    {
        [SerializeField] TextMeshProUGUI _textMeshPro;
        public string Text { get => _textMeshPro.text; set => _textMeshPro.text = value; }
        private void Awake()
        {
            if(!_textMeshPro) _textMeshPro = GetComponent<TextMeshProUGUI>();
        }
    }
}
