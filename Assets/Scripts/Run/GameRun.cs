using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Game.RNG;

public class GameRun : MonoBehaviour
{
    [SerializeField] int _seed;
    public int Seed { get => _seed; }
    public static GameRun instance;
    // Start is called before the first frame update
    void Awake()
    {
        if (!instance)
        {
            instance = this;
        }
        else
        {
            Destroy(gameObject);
        }

        Initialize();
    }

    void Initialize()
    {
        RNG.Initialize(Seed);
    }
}
