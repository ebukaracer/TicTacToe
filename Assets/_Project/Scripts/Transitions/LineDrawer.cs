using System.Collections;
using Racer.Utilities;
using UnityEngine;

internal class LineDrawer : Interpolation
{
    private LineRenderer _lineRenderer;

    // Length factor of the line.
    [SerializeField] private float offset;


    private void Start()
    {
        _lineRenderer = GetComponent<LineRenderer>();

        gameObject.ToggleActive(false);
    }

    /// <summary>
    /// Establishes and draws a line from a starting position.
    /// </summary>
    /// <param name="r">Right-end line position</param>
    /// <param name="l">Left-end line position</param>
    /// <param name="m">Middle-end line position</param>
    public void DrawLine(Vector2 r, Vector2 l, Vector2 m)
    {
        transform.position = m;

        gameObject.ToggleActive(true);

        StartCoroutine(Interpolate(r, index: 0, midPos: m));
        StartCoroutine(Interpolate(l, index: 1, midPos: m));
    }

    protected override IEnumerator Interpolate(Vector2 pos, Vector2 midPos, int index = 0)
    {
        float initial = 0;
        var final = offset;

        ElapsedTime = 0;

        // Caches the previous time, since it's dynamic.
        var elapsedTime = ElapsedTime;

        // Wait before transition begins
        yield return Utility.GetWaitForSeconds((Delay / 2f));

        // Establishes and Opens Line
        while (ElapsedTime < duration)
        {
            var newPos = Mathf.Lerp(initial, final, ElapsedTime / duration);

            MoveToPoint(pos, midPos, index, newPos);

            ElapsedTime += Time.deltaTime;

            yield return 0;
        }

        MoveToPoint(pos, midPos, index, final);


        // Closes Line
        yield return Utility.GetWaitForSeconds(Delay / 4f);

        if (ElapsedTime > duration)
        {
            duration -= .15f;

            while (elapsedTime < duration)
            {
                var newPos = Mathf.Lerp(final, initial, elapsedTime / duration);

                MoveToPoint(pos, midPos, index, newPos);

                elapsedTime += Time.deltaTime;

                yield return 0;
            }

            MoveToPoint(pos, midPos, index, initial);
        }
    }

    private void MoveToPoint(Vector2 pos, Vector2 midPos, int index, float factor)
    {
        if (Mathf.Abs(midPos.x) == 0 && Mathf.Abs(midPos.y) == 0)
            _lineRenderer.SetPosition(index, pos * factor);

        else if (Mathf.Abs(midPos.x) > 0)
            _lineRenderer.SetPosition(index, new Vector2(0, pos.y) * factor);

        else if (Mathf.Abs(pos.y) > 0)
            _lineRenderer.SetPosition(index, new Vector2(pos.x, 0) * factor);
    }
}
