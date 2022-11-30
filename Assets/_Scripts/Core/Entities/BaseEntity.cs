using Game.Utils;
using UnityEngine;

public abstract class BaseEntity : MonoBehaviour
{
    [SerializeField] float _timeScale = 1f;
    public float TimeScale { get => _timeScale; set { if (value >= 0) _timeScale = value; } }
    [field: SerializeField] public Team Team { get; set;}
}
