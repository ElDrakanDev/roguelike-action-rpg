using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Game.Events;

namespace Game.General
{
    public class CameraTargetFollow : MonoBehaviour
    {
        List<Transform> targets = new List<Transform>();
        Transform _transform;
        void Awake() => _transform = transform;

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
            if (targets.Count > 0)
            {
                Vector3 finalPos = new Vector3(0, 0, transform.position.z);
                foreach (var target in targets)
                {
                    finalPos.x += target.position.x;
                    finalPos.y += target.position.y;
                }
                _transform.position = finalPos / targets.Count;
            }
        }

        void AddTarget(GameObject target)
        {
            Transform t = target.transform;
            targets.Add(t);
        }
        void RemoveTarget(GameObject target)
        {
            Transform t = target.transform;
            targets.Remove(t);
        }
    }
}
