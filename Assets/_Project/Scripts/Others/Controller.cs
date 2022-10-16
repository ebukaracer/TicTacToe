using System;
using UnityEngine;

internal abstract class Controller : MonoBehaviour
{
    [NonSerialized] protected Tic Tic;
    [NonSerialized] protected Collider2D Hit;

    public abstract bool GetHitInfo(State[] states, State currentState, Sprite sprite, LayerMask clickable = default);
}
