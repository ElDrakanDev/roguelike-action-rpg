using UnityEngine;
using UnityEngine.InputSystem;
using Game.Events;

namespace Game.Unlocks
{
    public class UnlockTester : MonoBehaviour
    {
        [SerializeField] UnlockManager unlocks;
        [SerializeField] int saveSlot = 1;
        [SerializeField] UnlockScriptableObject unlock;
        // Start is called before the first frame update
        void Start()
        {
            unlocks = new UnlockManager(saveSlot);
        }

        // Update is called once per frame
        void Update()
        {
            Keyboard current = Keyboard.current;

            if (current.nKey.wasPressedThisFrame)
            {
                unlocks.Save();
                Debug.Log("Guardado: " + unlocks.saveFileData.ToString());
            }
            else if (current.cKey.wasPressedThisFrame)
            {
                unlocks.saveFileData.stats.deaths = 0;
                unlock.SetUnlocked(false);
                unlocks.Save();
            }
            else if (current.mKey.wasPressedThisFrame)
            {
                unlocks.Load();
                Debug.Log(unlocks.saveFileData.ToString());
            }
            else if (current.bKey.wasPressedThisFrame)
            {
                unlocks.saveFileData.stats.deaths++;
                EventManager.OnPlayerLose();
            }
        }
    }
}
