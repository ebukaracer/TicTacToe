using UnityEngine;

internal class RandomAiController : Ai
{
    private int _searchCount;

    [SerializeField] private Transform[] allTics;


    /// <summary>
    /// Places either an 'X or O' element on empty <see cref="Tic"/> slot.
    /// </summary>
    public override bool GetHitInfo(State[] states, State currentState, Sprite sprite, LayerMask clickable = default)
    {
        Hit = null; _searchCount = 0;

        while (!Hit)
        {
            _searchCount++;

            Hit = GetEmptySlot(clickable);

            // In-case we run into an infinite-loop
            if (_searchCount >= Metrics.MaxSearchCount)
                break;
        }

        if (!Hit.TryGetComponent(out Tic))
            return false;

        if (!Tic.IsClicked)
            states[Tic.Index] = currentState;

        Tic.SetOnClick(sprite, currentState);

        return true;
    }

    /// <summary>
    /// Searches for an empty slot available.
    /// </summary>
    private Collider2D GetEmptySlot(LayerMask layerMask)
    {
        return Physics2D.OverlapCircle(allTics[Random.Range(0, allTics.Length)].position,
            Metrics.TouchRadius,
            layerMask);
    }
}
