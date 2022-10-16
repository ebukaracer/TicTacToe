using System.Collections;
using UnityEngine;

internal class Mover : Interpolation
{
    /// <summary>
    /// Moves either the 'X or O' mid-position to center,
    /// depending on it's initial position.
    /// </summary>
    /// <param name="m">Middle-end child position.</param>
    public void MoveToCenter(int m)
    {
        Vector2 initialPos = transform.GetChild(m).localPosition;
        var finalPos = Vector2.zero;

        if (initialPos != Vector2.zero)
            StartCoroutine(Interpolate(initialPos, finalPos, m));
    }

    protected override IEnumerator Interpolate(Vector2 initialPos, Vector2 finalPos, int i = 0)
    {
        ElapsedTime = 0;

        yield return WaitBeforeMove;

        while (ElapsedTime < duration)
        {
            var newPos = Vector2.Lerp(initialPos, finalPos, ElapsedTime / duration);
            SetChildPosition(i, newPos);

            ElapsedTime += Time.deltaTime;

            yield return 0;
        }

        SetChildPosition(i, finalPos);
    }

    private void SetChildPosition(int i, Vector2 newPos)
    {
        transform.GetChild(i).localPosition = newPos;
    }
}
