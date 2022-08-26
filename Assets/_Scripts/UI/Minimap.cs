using UnityEngine;
using UnityEngine.UI;
using Game.Events;
using Game.Generation;
using System.Collections.Generic;
using System;
using System.Threading.Tasks;

namespace Game.UI
{
    using Game.Run;

    public class Minimap : MonoBehaviour
    {
        [SerializeField] GameObject minimapObject;
        [SerializeField] float margin = 1.2f;
        [SerializeField] float focusMult = 1.2f;
        Dictionary<Vector2Int, GameObject> minimapIcons = new Dictionary<Vector2Int, GameObject>();
        Level _level;
        float width, height;
        Vector3 _scale;
        Vector2Int CurrentPosition { get => Run.instance.navigator.Position; }

        private void Awake()
        {
            var rectTransform = minimapObject.GetComponent<RectTransform>();
            width = rectTransform.rect.width * margin;
            height = rectTransform.rect.height * margin;
            _scale = GetComponent<RectTransform>().localScale;
            DownScale();
        }

        private async void OnEnable()
        {
            if (EventManager.instance == null) await Task.Delay(1);

            EventManager.instance.onFinishGeneration += CreateMinimap;
            EventManager.instance.onRoomChange += UpdateMinimap;
            EventManager.instance.onDoorEnter += UpScale;
            EventManager.instance.onNavigationExit += DownScale;


            if (_level == null && minimapIcons.Count == 0 && Run.instance.Level != null)
            {
                CreateMinimap();
                UpdateMinimap();
            }
            else if (_level != null && minimapIcons.Count > 0)
                UpdateMinimap();
        }

        private void OnDisable()
        {
            EventManager.instance.onFinishGeneration -= CreateMinimap;
            EventManager.instance.onRoomChange -= UpdateMinimap;
            EventManager.instance.onDoorEnter -= UpScale;
            EventManager.instance.onNavigationExit -= DownScale;
        }

        void CreateMinimap()
        {
            foreach (var go in minimapIcons.Values) Destroy(go);
            minimapIcons.Clear();

            _level = Run.instance.Level;

            var rectTransform = minimapObject.GetComponent<RectTransform>();

            foreach (var position in _level.Keys)
            {
                Room room = _level[position];
                var icon = Instantiate(minimapObject);
                rectTransform = icon.GetComponent<RectTransform>();
                icon.transform.SetParent(transform, false);
                rectTransform.anchoredPosition = new Vector2(width * position.x, height * position.y);
                icon.GetComponent<MinimapRoom>().room = room;

                minimapIcons.Add(position, icon);
            }

            UpdateMinimap();
        }
        void UpdateMinimap()
        {
            foreach (var roomPos in minimapIcons.Keys)
            {
                var icon = minimapIcons[roomPos];
                var iconTransform = icon.GetComponent<RectTransform>();
                iconTransform.anchoredPosition = new Vector2(width * (roomPos.x - CurrentPosition.x), height * (roomPos.y - CurrentPosition.y));
                icon.GetComponent<MinimapRoom>().UpdateIcon();

                iconTransform.localScale = roomPos == CurrentPosition ? iconTransform.localScale * focusMult : Vector3.one;
            }
        }

        void DownScale()
        {
            GetComponent<RectTransform>().localScale = Vector3.zero;
        }
        void UpScale()
        {
            GetComponent<RectTransform>().localScale = _scale;
        }
    }
}
