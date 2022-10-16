using UnityEngine;

internal class LineMover : Interpolation
{
    private void OnEnable()
    {
        GameManager.OnGameStatus += GameManagerOnGameStatus;
    }

    private void GameManagerOnGameStatus(GameStatus status)
    {
        if (status.Equals(GameStatus.Gameover))
            DoMoveToCenter();
    }


    /// <summary>
    /// Moves the drawn-line to the center, depending on it's position.
    /// </summary>
    public void DoMoveToCenter()
    {
        if (!gameObject.activeInHierarchy)
            return;

        StartCoroutine(Interpolate(transform.position, Vector2.zero));
    }

    private void OnDisable()
    {
        GameManager.OnGameStatus -= GameManagerOnGameStatus;
    }
}
