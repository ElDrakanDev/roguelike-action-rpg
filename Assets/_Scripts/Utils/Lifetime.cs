using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Lifetime : MonoBehaviour
{
    public float lifetime;
    void Update()
    {
        if (lifetime < 0) Destroy(gameObject);
        lifetime -= Time.deltaTime;
    }
}
