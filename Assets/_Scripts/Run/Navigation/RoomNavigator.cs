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
        public Level CurrentLevel { get => _level; private set => _level = value; }
        public Room ActiveRoom { get { _activeRoom = CurrentLevel[Position] ; return CurrentLevel[Position]; } }
        public Vector2Int Position { get => _position; set =>  _position = value; }
        LevelGenerator generator;

        
        bool _isHandlingMovementInput = false;
        Vector2Int _inputDir = Vector2Int.zero;
        bool _inputCanceled;

        public RoomNavigator(PlayerActionsControls controls)
        {
            generator = new LevelGenerator();
            this.controls = controls;

            EventManager.instance.onDoorEnter += OnDoorEnter;
            //EventManager.instance.onDoorExit += OnDoorExit;
            controls.Enable();
        }
        ~RoomNavigator()
        {
            EventManager.instance.onDoorEnter -= OnDoorEnter;
            //EventManager.instance.onDoorExit -= OnDoorExit;
            controls.Disable();
        }
        public void Generate(int normals, int specials, int shops)
        {
            Position = Vector2Int.zero;
            _level = generator.Generate(normals, specials, shops);
            EventManager.instance.OnFinishGeneration();
        }

        public bool Move(int x, int y)
        {
            if (x == 0 && y == 0) return false;

            var newPos = Position + new Vector2Int(x, y);
            if(CurrentLevel.ContainsKey(newPos))
            {
                CurrentLevel.EnterRoom(Position, newPos);
                Position = newPos;
                EventManager.instance.OnRoomChange();
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
                EventManager.instance.OnRoomChange();
                return true;
            }
            return false;
        }
        public void MoveTo(int x, int y)
        {
            var newPos = new Vector2Int(x, y);
            CurrentLevel.EnterRoom(Position, newPos);
            Position = newPos;
            EventManager.instance.OnRoomChange();
        }
        public void MoveTo(Vector2Int pos)
        {
            CurrentLevel.EnterRoom(Position, pos);
            Position = pos;
            EventManager.instance.OnRoomChange();
        }

        public void HandleRoomMovement()
        {
            if (!_isHandlingMovementInput)
            {
                _inputDir = Vector2Int.zero;
                controls.UI.Direction.performed += ReadInputDirection;
                controls.UI.Exit.performed += ReadInputCancel;
                _isHandlingMovementInput = true;
            }
        }
        public void ReadInputDirection(InputAction.CallbackContext context)
        {
            var vec2Dir = context.ReadValue<Vector2>();
            _inputDir = new Vector2Int(Mathf.RoundToInt(vec2Dir.x), Mathf.RoundToInt(vec2Dir.y));

            if(_inputDir != Vector2Int.zero)
            {
                if (Move(_inputDir))
                {
                    controls.UI.Direction.performed -= ReadInputDirection;
                    controls.UI.Exit.performed -= ReadInputCancel;
                    EventManager.instance.OnNavigationExit();
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
                controls.UI.Direction.performed -= ReadInputDirection;
                controls.UI.Exit.performed -= ReadInputCancel;
                EventManager.instance.OnNavigationExit();
                _inputDir = Vector2Int.zero;
            }
        }

        void OnDoorEnter() => HandleRoomMovement();
        //void OnDoorExit() => HandleRoomMovement(false);
    }
}
