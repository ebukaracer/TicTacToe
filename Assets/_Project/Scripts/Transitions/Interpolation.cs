using UnityEngine;
using System.Collections;
using Racer.Utilities;

internal class Interpolation : MonoBehaviour
{
    protected WaitForSeconds WaitBeforeMove = Utility.GetWaitForSeconds(1f);

    [Header("INTERPOLATION TIMINGS")]
    // Time Elapsed
    protected float ElapsedTime;

    // Delay
    protected float Delay = 1f;

    // Initial Time
    [SerializeField, Range(0, 2)]
    protected float duration;


    protected virtual IEnumerator Interpolate(Vector2 initialPos, Vector2 finalPos, int i = 0)
    {
        ElapsedTime = 0;

        yield return WaitBeforeMove;

        while (ElapsedTime < duration)
        {
            var newPos = Vector2.Lerp(initialPos, finalPos, ElapsedTime / duration);

            transform.position = newPos;

            ElapsedTime += Time.deltaTime;

            yield return 0;
        }

        transform.position = finalPos;
    }
}
