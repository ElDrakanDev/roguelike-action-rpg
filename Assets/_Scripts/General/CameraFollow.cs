using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Game.Events;

namespace Game.General
{
    [RequireComponent(typeof(Camera))]
    public class CameraFollow : MonoBehaviour
    {
        Camera cam;
        List<Transform> targets = new List<Transform>();
        public float followSpeed = 0.9f;

        void Awake()
        {
            cam = GetComponent<Camera>();
        }
        private void OnEnable()
        {
            EventManager.onPlayerSpawn += AddTarget;
            EventManager.onPlayerDespawn += RemoveTarget;
        }
        private void OnDisable()
        {
            EventManager.onPlayerSpawn -= AddTarget;
            EventManager.onPlayerDespawn -= RemoveTarget;
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
                finalPos.x /= targets.Count;
                finalPos.y /= targets.Count;
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
    }
}
