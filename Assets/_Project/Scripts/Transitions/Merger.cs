using System.Collections;
using UnityEngine;

internal class Merger : Interpolation
{
    /// <summary>
    /// Merges either a group of X's or O's.
    /// </summary>
    /// <param name="l">Left-end child index</param>
    /// <param name="m">Middle-end child index</param>
    /// <param name="r">Right-end child index</param>
    public void Collapse(int l, int m, int r)
    {
        Vector2 initialPoint = Vector3.zero;
        Vector2 finalPoint = transform.GetChild(m).localPosition;

        if (finalPoint == Vector2.zero)
        {
            StartCoroutine(Interpolate(GetLocalPosition(l), finalPoint, l));
            StartCoroutine(Interpolate(GetLocalPosition(r), finalPoint, r));
        }
        else
        {
            StartCoroutine(Interpolate(GetLocalPosition(l), initialPoint, l));
            StartCoroutine(Interpolate(GetLocalPosition(r), initialPoint, r));
        }
    }

    protected override IEnumerator Interpolate(Vector2 oldPos, Vector2 newPos, int i = 0)
    {
        yield return WaitBeforeMove;

        ElapsedTime = 0;

        while (ElapsedTime < duration)
        {
            var pos = Vector2.Lerp(oldPos, newPos, ElapsedTime / duration);
            SetLocalPosition(i, pos);

            ElapsedTime += Time.deltaTime;

            yield return 0;
        }

        SetLocalPosition(i, newPos);
    }

    private void SetLocalPosition(int i, Vector2 pos)
    {
        transform.GetChild(i).localPosition = pos;
    }

    private Vector2 GetLocalPosition(int index)
    {
        return transform.GetChild(index).localPosition;
    }
}