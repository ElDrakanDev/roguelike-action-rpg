using System.Collections.Generic;
using UnityEngine;
using Game.Events;
using UnityEngine.Tilemaps;
using System.Threading.Tasks;
using System;

namespace Game.General
{
    [RequireComponent(typeof(Camera))]
    public class CameraFollow : MonoBehaviour
    {
        Camera cam;
        List<Transform> targets = new List<Transform>();
        public float followSpeed = 0.9f;
        Vector3 _min, _max;
        Vector3 RoomCenter { get => (_min + _max) * 0.5f; }

        void Awake()
        {
            cam = GetComponent<Camera>();
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
        void Update()
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
                transform.position = Vector3.Lerp(transform.position, finalPos, followSpeed);
            }
        }

        void AddTarget(GameObject target)
        {
            targets.Add(target.transform);
        }
        void RemoveTarget(GameObject target)
        {
            targets.Remove(target.transform);
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
                _min.y += halfHeight; _max.y -= halfHeight;

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
    }
}
