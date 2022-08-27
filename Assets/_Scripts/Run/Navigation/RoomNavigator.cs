using UnityEngine;
using Game.Generation;
using Game.Events;
using Game.Input;
using UnityEngine.InputSystem;

namespace Game.Run
{
    [System.Serializable]
    public class RoomNavigator
    {
        Level _level;
        [SerializeField] Vector2Int _position = Vector2Int.zero;
        [SerializeField] Room _activeRoom;
        [SerializeField] PlayerActionsControls controls;
        public Level CurrentLevel { get => _level; set{ _level = value; Position = Vector2Int.zero; _level.EnterRoom(Position); } }
        public Room ActiveRoom {
            get {
                Room value = null;
                CurrentLevel?.TryGetValue(Position, out value);
                _activeRoom = value;
                if (value != null)
                    return value;
                return null;
            }
        }
        public Vector2Int Position { get => _position; set =>  _position = value; }
        LevelGenerator generator;

        
        bool _isHandlingMovementInput = false;
        Vector2Int _inputDir = Vector2Int.zero;
        bool _inputCanceled;

        public RoomNavigator(PlayerActionsControls controls)
        {
            generator = new LevelGenerator();
            this.controls = controls;

            EventManager.onDoorEnter += OnDoorEnter;
            //EventManager.onDoorExit += OnDoorExit;
            controls.Enable();
        }
        ~RoomNavigator()
        {
            EventManager.onDoorEnter -= OnDoorEnter;
            //EventManager.onDoorExit -= OnDoorExit;
            controls.Disable();
        }
        public void Generate(int normals, int specials, int shops)
        {
            Position = Vector2Int.zero;
            _level = generator.Generate(normals, specials, shops);
            EventManager.OnFinishGeneration();
        }

        public bool Move(int x, int y)
        {
            if (x == 0 && y == 0) return false;

            var newPos = Position + new Vector2Int(x, y);
            if(CurrentLevel.ContainsKey(newPos))
            {
                CurrentLevel.EnterRoom(Position, newPos);
                Position = newPos;
                EventManager.OnRoomChange();
                return true;
            }
            return false;
        }
        public bool Move(Vector2Int direction)
        {
            if (direction.x == 0 && direction.y == 0) return false;

            var newPos = Position + direction;
            if (CurrentLevel.ContainsKey(newPos))
            {
                CurrentLevel.EnterRoom(Position, newPos);
                Position = newPos;
                EventManager.OnRoomChange();
                return true;
            }
            return false;
        }
        public void MoveTo(int x, int y)
        {
            var newPos = new Vector2Int(x, y);
            if(newPos != Position)
            {
                CurrentLevel.EnterRoom(Position, newPos);
                Position = newPos;
                EventManager.OnRoomChange();
            }
        }
        public void MoveTo(Vector2Int pos)
        {
            if(pos != Position)
            {
                CurrentLevel.EnterRoom(Position, pos);
                Position = pos;
                EventManager.OnRoomChange();
            }
        }

        public void HandleRoomMovement()
        {
            if (!_isHandlingMovementInput)
            {
                _inputDir = Vector2Int.zero;
                controls.UI.Direction.started += ReadInputDirection;
                controls.UI.Exit.started += ReadInputCancel;
                _isHandlingMovementInput = true;
            }
        }
        public void ReadInputDirection(InputAction.CallbackContext context)
        {
            var vec2Dir = context.ReadValue<Vector2>();
            _inputDir = new Vector2Int(Mathf.RoundToInt(vec2Dir.x), Mathf.RoundToInt(vec2Dir.y));
            Debug.Log($"Reading room move. {_inputDir}");
            if(_inputDir != Vector2Int.zero && _inputDir.magnitude <= 1)
            {
                if (Move(_inputDir))
                {
                    controls.UI.Direction.started -= ReadInputDirection;
                    controls.UI.Exit.started -= ReadInputCancel;
                    EventManager.OnNavigationExit();
                    _isHandlingMovementInput = false;
                    _inputDir = Vector2Int.zero;
                }
            }
        }

        public void ReadInputCancel(InputAction.CallbackContext context)
        {
            _inputCanceled = context.performed;

            if (_inputCanceled)
            {
                _isHandlingMovementInput = false;
                controls.UI.Direction.started -= ReadInputDirection;
                controls.UI.Exit.started -= ReadInputCancel;
                EventManager.OnNavigationExit();
                _inputDir = Vector2Int.zero;
            }
        }

        void OnDoorEnter() => HandleRoomMovement();
        //void OnDoorExit() => HandleRoomMovement(false);
    }
}
