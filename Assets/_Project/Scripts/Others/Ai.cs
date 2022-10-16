using UnityEngine;

internal abstract class Ai : Controller
{
    [field: SerializeField]
    public float WaitDelay { get; private set; } = .75f;
}
