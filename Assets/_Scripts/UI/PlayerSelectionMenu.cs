using UnityEngine;
using UnityEngine.InputSystem;
using Game.Players;
using Game.Input;
using Game.Generation;
using Game.Run;

namespace Game.UI
{
    public class PlayerSelectionMenu : MonoBehaviour
    {
        public PlayerActionsControls controls;
        InputDevice listeningToDevice;
        public InputDevice ActiveDevice { get => listeningToDevice; protected set => listeningToDevice = value; }
        [SerializeField] GameObject selectionPanel;

        private void Awake()
        {
            controls = new PlayerActionsControls();
        }
        private void OnEnable()
        {
            controls.Enable();
            controls.Player.Start.started += OpenSelectionIfJoinConditionMet;
        }
        private void OnDisable()
        {
            controls.Disable();
            controls.Player.Start.started -= OpenSelectionIfJoinConditionMet;
        }

        void OpenSelectionIfJoinConditionMet(InputAction.CallbackContext ctxt)
        {
            if(JoinConditionMet(ctxt.control.device))
            {
                listeningToDevice = ctxt.control.device;
                selectionPanel.SetActive(true);
                Time.timeScale = 0;
            }
        }
        bool JoinConditionMet(InputDevice device)
        {
            return (
                listeningToDevice is null &&
                PlayerInput.FindFirstPairedToDevice(device) is null &&
                Room.ActiveRoom.Type == RoomType.Start
            );
        }
        public void JoinPlayer(PlayerSelectSO selection)
        {
            Debug.Log($"{selection.characterName} joined");
            selectionPanel.SetActive(false);
            Time.timeScale = 1.0f;
            PlayerInputManager.instance.playerPrefab = selection.playerPrefab;
            var pInput = PlayerInputManager.instance.JoinPlayer(pairWithDevice: ActiveDevice);
            ActiveDevice = null;

            Vector3 doorPos;
            foreach (var interactable in GameObject.FindGameObjectsWithTag("Interactable"))
            {
                if (interactable.TryGetComponent(out RoomDoor roomDoor))
                {
                    doorPos = interactable.transform.position;
                    float doorOffset = interactable.GetComponent<SpriteRenderer>().bounds.size.y * 0.5f;
                    Vector3 groundPos = Physics2D.Raycast(doorPos, Vector2.down, float.MaxValue, LayerMask.GetMask("Ground", "Platform")).point;
                    
                    float playerOffset = 0;
                    if (pInput.TryGetComponent<BoxCollider2D>(out var playerCollider))
                        playerOffset = playerCollider.bounds.size.y * 0.5f;
                    if (groundPos != null)
                        pInput.transform.position = new Vector3(groundPos.x, groundPos.y + playerOffset, 0);
                    else
                    {
                        float yPos = doorPos.y;
                        if (playerOffset > -0.01f && playerOffset < 0.01f && doorOffset > -0.01f && doorOffset < 0.01f) yPos += -doorOffset + playerOffset;
                        pInput.transform.position = new Vector3(doorPos.x, yPos, 0);
                    }
                    return;
                }
            }
        }
    }
}
