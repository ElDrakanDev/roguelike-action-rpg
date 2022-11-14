using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class CoroutineUtils
{
    public static IEnumerator DelayedCall(float delay, Action callback)
    {
        yield return new WaitForSeconds(delay);
        callback();
    }
}
