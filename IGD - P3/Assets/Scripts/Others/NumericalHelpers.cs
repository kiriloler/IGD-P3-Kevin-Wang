using System;
using UnityEngine;

[Serializable]
public struct FloatRange
{
    public float min, max;
    public float Random() => UnityEngine.Random.Range(min, max);
}
