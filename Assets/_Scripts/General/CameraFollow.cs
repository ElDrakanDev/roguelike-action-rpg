using System.Collections.Generic;
using UnityEngine;
using Game.Events;
using Game.Utils;
using UnityEngine.Tilemaps;
using System.Threading.Tasks;
using System;

namespace Game.General
{
    [RequireComponent(typeof(Camera))]
    public class CameraFollow : MonoBehaviour
    {
        [SerializeField] GameObject offscreenArrow;
        [SerializeField] float pointerMargin = 1.5f;
        Camera cam;
        List<Transform> targets = new List<Transform>();
        Vector3 _min, _max;
        Vector3 RoomCenter { get => (_min + _max) * 0.5f; }
        Dictionary<Transform, GameObject> offscreenPointers = new Dictionary<Transform, GameObject>();
        Transform t;

        void Awake()
        {
            cam = GetComponent<Camera>();
            t = transform;
        }
        private async void Start()
        {
            while (Run.Run.instance.ActiveRoom == null) await Task.Delay(1);
            UpdateBoundaries();
        }
        private void OnEnable()
        {
            EventManager.onPlayerSpawn += AddTarget;
            EventManager.onPlayerDespawn += RemoveTarget;
            EventManager.onRoomChange += UpdateBoundaries;

        }
        private void OnDisable()
        {
            EventManager.onPlayerSpawn -= AddTarget;
            EventManager.onPlayerDespawn -= RemoveTarget;
            EventManager.onRoomChange -= UpdateBoundaries;
        }
        void LateUpdate()
        {
            if(targets.Count > 0)
            {
                Vector3 finalPos = new Vector3(0, 0, transform.position.z);
                foreach(var target in targets)
                {
                    finalPos.x += target.position.x;
                    finalPos.y += target.position.y;
                }
                finalPos.x = Mathf.Clamp(finalPos.x / targets.Count, _min.x, _max.x);
                finalPos.y = Mathf.Clamp(finalPos.y / targets.Count, _min.y, _max.y);
                transform.position = finalPos;

                UpdateOffscreenPointers();
            }
        }
        void UpdateOffscreenPointers()
        {
            float halfHeight = cam.orthographicSize;
            float halfWidth = cam.aspect * halfHeight;

            // Calculations assume map is position at the origin
            float minXBoundary = t.position.x - halfWidth;
            float maxXBoundary = t.position.x + halfWidth;
            float minYBoundary = t.position.y - halfHeight;
            float maxYBoundary = t.position.y + halfHeight;
            foreach (var target in offscreenPointers.Keys)
            {
                var pointer = offscreenPointers[target];
                Vector2 pos = target.position;
                if(InView(pos))
                    pointer.SetActive(false);
                else
                {
                    pointer.SetActive(true);
                    Vector2 pointerPos = new Vector2(t.position.x, t.position.y);
                    Vector2 towardsTarget = (pos - pointerPos).normalized;
                    pointerPos.x = Mathf.Clamp(pos.x + towardsTarget.x, minXBoundary + pointerMargin, maxXBoundary - pointerMargin);
                    pointerPos.y = Mathf.Clamp(pos.y + towardsTarget.y, minYBoundary + pointerMargin, maxYBoundary - pointerMargin);
                    pointer.transform.position = pointerPos;
                    towardsTarget = (pos - pointerPos).normalized;
                    pointer.transform.rotation = Quaternion.Euler(0, 0, Vector2Extension.AngleFromDirection(towardsTarget));
                }
            }
        }
        void AddTarget(GameObject target)
        {
            Transform t = target.transform;
            targets.Add(t);
            offscreenPointers.Add(t, Instantiate(offscreenArrow, t.position, Quaternion.identity));
        }
        void RemoveTarget(GameObject target)
        {
            Transform t = target.transform;
            targets.Remove(t);
            Destroy(offscreenPointers[t]);
            offscreenPointers.Remove(t);
        }
        void UpdateBoundaries()
        {
            var tilemaps = Run.Run.instance.ActiveRoom.gameObject.GetComponentsInChildren<Tilemap>();
            foreach(var tilemap in tilemaps)
            {
                if (!tilemap.gameObject.CompareTag("Ground")) continue;
                tilemap.CompressBounds();
                float halfHeight = cam.orthographicSize;
                float halfWidth = cam.aspect * halfHeight;          
                _min = tilemap.transform.TransformPoint(tilemap.localBounds.min + tilemap.tileAnchor - new Vector3(0.5f, 0.5f, 0));
                _max = tilemap.transform.TransformPoint(tilemap.localBounds.max + tilemap.tileAnchor - new Vector3(0.5f, 0.5f, 0));
                _min.x += halfWidth; _max.x -= halfWidth;
                if(_min.x > _max.x)
                {
                    float min = _min.x;
                    _min.x = _max.x;
                    _max.x = min;
                }
                _min.y += halfHeight; _max.y -= halfHeight;
                if (_min.y > _max.y)
                {
                    float min = _min.y;
                    _min.y = _max.y;
                    _max.y = min;
                }

                return;
            }

            throw new NullReferenceException($"No se encuntro ningun tilemap con tag 'Ground' para establecer los límites de la camara.");
        }
        private void OnDrawGizmosSelected()
        {
            Gizmos.color = Color.blue;
            Gizmos.DrawWireCube(RoomCenter, new Vector3(_max.x - _min.x, _max.y - _min.y, 0));
            Gizmos.DrawSphere(RoomCenter, 0.5f);
        }

        bool InView(Vector3 position)
        {
            Vector3 viewPos = cam.WorldToViewportPoint(position);
            return viewPos.x >= 0 && viewPos.x <= 1 && viewPos.y >= 0 && viewPos.y <= 1 && viewPos.z > 0;
            // position.x > _min.x && position.x < _max.x && position.y > _min.y && position.y < _max.y;
        }
    }
}
