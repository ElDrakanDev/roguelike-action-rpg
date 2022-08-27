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
        Vector3 _iconScale;
        Vector3 _scale;
        Vector2Int CurrentPosition { get => Run.instance.navigator.Position; }

        private void Awake()
        {
            var iconRectTransform = minimapObject.GetComponent<RectTransform>();
            _iconScale = iconRectTransform.localScale;
            width = iconRectTransform.rect.width * margin;
            height = iconRectTransform.rect.height * margin;
            _scale = GetComponent<RectTransform>().localScale;
            DownScale();
        }

        private async void OnEnable()
        {
            while(!Run.instance || Run.instance.Level is null) await Task.Delay(1);

            EventManager.instance.onFinishGeneration += CreateMinimap;
            EventManager.instance.onRoomChange += UpdateMinimap;
            EventManager.instance.onDoorEnter += UpScale;
            EventManager.instance.onNavigationExit += DownScale;

            CreateMinimap();
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

                iconTransform.localScale = roomPos == CurrentPosition ? _iconScale * focusMult : _iconScale;
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
