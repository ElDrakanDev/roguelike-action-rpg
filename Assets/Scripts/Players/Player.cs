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
        private void Awake()
        {
            stats = new CharacterStats(this, baseStats.baseStats);
            Debug.Log("Base Atributes:");
            PrintAttributes();
        }

        private void Start()
        {
            IInteractable[] interactables = GameObject.FindGameObjectWithTag("Interactable").GetComponents<IInteractable>();
            foreach(var interactable in interactables) interactable.Interact(gameObject);

            Debug.Log("Post-pickup attributes:");
            PrintAttributes();
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
