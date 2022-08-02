using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Game.States;

namespace Game.Players
{
    public abstract class ControllerState : State
    {
        public abstract void Move(Vector2 direction, float speed, float maxSpeed, float gravity, float maxFall);
        public abstract void Jump(float force);
    }
}
