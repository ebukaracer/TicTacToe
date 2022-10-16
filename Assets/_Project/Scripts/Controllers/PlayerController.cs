using Racer.Utilities;
using UnityEngine;

internal class PlayerController : Controller
{
    public override bool GetHitInfo(State[] states, State currentState, Sprite sprite, LayerMask clickable = default)
    {
        Hit = GetEmptySlot(clickable);

        if (!Hit) return false;

        Tic = Hit.GetComponent<Tic>();

        if (!Tic) return false;

        if (!Tic.IsClicked)
            states[Tic.Index] = currentState;

        Tic.SetOnClick(sprite, currentState);

        return true;
    }


    private static Collider2D GetEmptySlot(LayerMask layerMask)
    {
        Vector2 touchPosition = Utility.CameraMain.ScreenToWorldPoint(Input.mousePosition);
        return Physics2D.OverlapCircle(touchPosition, Metrics.TouchRadius, layerMask);
    }
}