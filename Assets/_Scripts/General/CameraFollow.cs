using System.Collections.Generic;
using UnityEngine;
using Game.Events;
using Game.Utils;
using UnityEngine.Tilemaps;
using System;
using Cinemachine;

namespace Game.General
{
    [RequireComponent(typeof(Camera))]
    public class CameraFollow : MonoBehaviour
    {
        [SerializeField] GameObject offscreenArrow;
        [SerializeField] float pointerMargin = 1.5f;
        CinemachineConfiner2D _confiner;
        Transform _transform;
        Camera _cam;
        Dictionary<Transform, GameObject> offscreenPointers = new Dictionary<Transform, GameObject>();

        void Awake()
        {
            _transform = transform;
            _cam = GetComponent<Camera>();
            _confiner = GetComponent<CinemachineConfiner2D>();
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
        void Update()
        {
            UpdateOffscreenPointers();
        }
        void UpdateOffscreenPointers()
        {
            float halfHeight = _cam.orthographicSize;
            float halfWidth = _cam.aspect * halfHeight;

            // Calculations assume map is position at the origin
            float minXBoundary = _transform.position.x - halfWidth;
            float maxXBoundary = _transform.position.x + halfWidth;
            float minYBoundary = _transform.position.y - halfHeight;
            float maxYBoundary = _transform.position.y + halfHeight;
            foreach (var target in offscreenPointers.Keys)
            {
                var pointer = offscreenPointers[target];
                Vector2 pos = target.position;
                if(InView(pos))
                    pointer.SetActive(false);
                else
                {
                    pointer.SetActive(true);
                    Vector2 pointerPos = new Vector2(_transform.position.x, _transform.position.y);
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
            offscreenPointers.Add(t, Instantiate(offscreenArrow, t.position, Quaternion.identity));
        }
        void RemoveTarget(GameObject target)
        {
            Transform t = target.transform;
            Destroy(offscreenPointers[t]);
            offscreenPointers.Remove(t);
        }
        void UpdateBoundaries()
        {
            var tilemaps = Run.Run.instance.ActiveRoom.gameObject.GetComponentsInChildren<Tilemap>();
            foreach (var tilemap in tilemaps)
            {
                if (!tilemap.gameObject.CompareTag("Background")) continue;
                tilemap.CompressBounds();
                _confiner.m_BoundingShape2D = tilemap.GetComponent<CompositeCollider2D>();
                _confiner.InvalidateCache();
                return;
            }

            throw new NullReferenceException($"No se encuntro ningun tilemap con tag 'Ground' para establecer los límites de la camara.");
        }
        bool InView(Vector3 position)
        {
            Vector3 viewPos = _cam.WorldToViewportPoint(position);
            return viewPos.x >= 0 && viewPos.x <= 1 && viewPos.y >= 0 && viewPos.y <= 1 && viewPos.z > 0;
        }
    }
}
