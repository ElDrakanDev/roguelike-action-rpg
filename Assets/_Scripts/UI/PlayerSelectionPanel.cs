using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Game.Players;
using UnityEngine.InputSystem;

namespace Game.UI
{
    public class PlayerSelectionPanel : MonoBehaviour
    {
        [SerializeField] PlayerSelectionMenu menu;
        [SerializeField] List<PlayerSelectSO> selectablePlayers;
        [SerializeField] GameObject selectableTemplate;
        List<GameObject> selectables = new List<GameObject>();
        [SerializeField] int _activeSelection = 0;
        public int ActiveSelection
        {
            get { return _activeSelection; }
            set => _activeSelection = Mathf.Abs(value % selectablePlayers.Count);
        }
        #region Setup
        private void Awake()
        {
            //AddRandomPlayer();
            foreach (var selectablePlayer in selectablePlayers)
            {
                if (selectablePlayer.Unlocked)
                {
                    var selectable = Instantiate(selectableTemplate, transform);
                    var image = selectable.GetComponentInChildren<Image>();
                    image.sprite = selectablePlayer.sprite;
                    image.SetNativeSize();
                    selectable.GetComponentInChildren<PlayerSelectionData>().scriptable = selectablePlayer;
                    selectable.GetComponentInChildren<PlayerSelectionName>().Text = selectablePlayer.characterName;
                    selectable.GetComponentInChildren<PlayerSelectionDescription>().Text = selectablePlayer.characterDescription;
                    selectables.Add(selectable);
                    selectable.SetActive(false);
                }
            }
        }
        private void OnEnable()
        {
            menu.controls.Player.Movement.performed += HandleMovement;
            menu.controls.Player.Start.started += HandleSubmit;
            selectables[ActiveSelection].SetActive(false);
            ActiveSelection = 0;
            selectables[ActiveSelection].SetActive(true);
        }
        private void OnDisable()
        {
            menu.controls.Player.Movement.performed -= HandleMovement;
            menu.controls.Player.Start.started -= HandleSubmit;
        }
        #endregion
        void HandleMovement(InputAction.CallbackContext ctxt)
        {
            if (ctxt.control.device == menu.ActiveDevice)
            {
                var direction = ctxt.ReadValue<Vector2>();
                if (direction.x <= -0.5f) MovePrev();
                if (direction.x >= 0.5f) MoveNext();
            }
        }
        void HandleSubmit(InputAction.CallbackContext ctxt)
        {
            if (ctxt.control.device == menu.ActiveDevice)
            {
                var scriptable = selectables[ActiveSelection].GetComponentInChildren<PlayerSelectionData>().scriptable;
                menu.JoinPlayer(scriptable);
            }
        }
        public void MoveNext()
        {
            selectables[ActiveSelection].SetActive(false);
            ActiveSelection++;
            selectables[ActiveSelection].SetActive(true);
        }
        public void MovePrev()
        {
            selectables[ActiveSelection].SetActive(false);
            ActiveSelection--;
            selectables[ActiveSelection].SetActive(true);
        }
    }
}

