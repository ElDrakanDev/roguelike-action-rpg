using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Game.Stats;
using Game.Interfaces;
using Game.Helpers;

namespace Game.Players
{
    public class Player : MonoBehaviour
    {
        public CharacterStats stats;
        [SerializeField] BaseStatObject baseStats;
        int steps = 0;
        private void Awake()
        {
            stats = new CharacterStats(this, baseStats.baseStats);
        }

        private void Start()
        {
            IInteractable[] interactables = GameObject.FindGameObjectWithTag("Interactable")?.GetComponents<IInteractable>();
            if(interactables != null && interactables.Length > 0)
            {
                foreach (var interactable in interactables)
                {
                    interactable.Interact(gameObject);
                }
            }
        }
        public void Update()
        {
            if (steps % 1000 == 0)
                PrintAttributes();
            steps++;
        }
        public void PrintAttributes()
        {
            string msg = "";
            foreach (var attribute in stats.Attributes)
            {
                msg += $"{attribute}: {stats[attribute].Value}\n";
            }
            Debug.Log(msg);
        }

    }
}