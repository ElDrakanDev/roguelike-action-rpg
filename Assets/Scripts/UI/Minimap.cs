using UnityEngine;
using UnityEngine.UI;
using Game.Events;
using Game.Generation;
using System.Collections.Generic;
using System;

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
        Vector2Int CurrentPosition { get => Run.instance.navigator.Position; }

        private void Awake()
        {
            var rectTransform = minimapObject.GetComponent<RectTransform>();
            width = rectTransform.rect.width * margin;
            height = rectTransform.rect.height * margin;
        }

        private void OnEnable()
        {
            EventManager.instance.onFinishGeneration += CreateMinimap;
            EventManager.instance.onRoomChange += UpdateMinimap;

            if (_level == null && minimapIcons.Count == 0 && Run.instance.Level != null)
            {
                CreateMinimap();
                UpdateMinimap();
            }
        }

        private void OnDisable()
        {
            EventManager.instance.onFinishGeneration -= CreateMinimap;
            EventManager.instance.onRoomChange -= UpdateMinimap;
        }

        void CreateMinimap()
        {
            Debug.Log("Creando minimapa");
            foreach (var go in minimapIcons.Values) Destroy(go);
            minimapIcons.Clear();

            _level = Run.instance.Level;

            var rectTransform = minimapObject.GetComponent<RectTransform>();

            foreach (var position in _level.Keys)
            {
                Room room = _level[position];
                var icon = Instantiate(minimapObject);
                var img = icon.GetComponent<Image>();
                rectTransform = icon.GetComponent<RectTransform>();
                icon.transform.SetParent(transform, false);
                rectTransform.anchoredPosition = new Vector2(width * position.x, height * position.y);
                Color newColor = GetColorByType(room);
                newColor.a = 0;
                img.color = newColor;
                icon.name = position.ToString();

                minimapIcons.Add(position, icon);
            }
        }
        void UpdateMinimap()
        {
            foreach (var roomPos in minimapIcons.Keys)
            {
                Room room = _level[roomPos];
                var icon = minimapIcons[roomPos];
                var img = icon.GetComponent<Image>();
                var rectTransform = icon.GetComponent<RectTransform>();
                Color newColor = img.color;
                newColor.a = GetAlphaByExploreState(room);
                img.color = newColor;
                rectTransform.anchoredPosition = new Vector2(width * (roomPos.x - CurrentPosition.x), height * (roomPos.y - CurrentPosition.y));

                rectTransform.localScale = roomPos == CurrentPosition ? rectTransform.localScale * focusMult : Vector3.one;
            }
        }

        Color GetColorByType(Room room)
        {
            switch (room.Type)
            {
                case RoomType.Treasure:
                    return Color.yellow;
                case RoomType.Shop:
                    return Color.magenta;
                case RoomType.NextLevel:
                    return Color.red;
                case RoomType.Start:
                    return Color.gray;
                default:
                    return Color.white;
            }
        }

        float GetAlphaByExploreState(Room room)
        {
            switch (room.exploredState)
            {
                case ExploredState.NotDiscovered:
                    return 0;
                case ExploredState.Nearby:
                    return 0.3f;
                case ExploredState.Explored:
                    return 1;
                default:
                    throw new ArgumentException($"{room.exploredState} no es un estado de exploracion valido");
            }
        }
    }
}
