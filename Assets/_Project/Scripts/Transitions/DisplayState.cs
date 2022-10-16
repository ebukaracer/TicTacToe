using System.Collections;
using Racer.Utilities;
using UnityEngine;

internal class DisplayState : MonoBehaviour
{
    private Animator _animator;


    private void Start()
    {
        _animator = GetComponent<Animator>();

        GameManager.OnStateWon += GameManagerOnStateWon;
    }

    private void GameManagerOnStateWon(State state)
    {
        StartCoroutine(Preview_CurrentState_OnGameover(state));
    }

    /// <summary>
    /// Displays either 'X, O or Draw' on Gameover.
    /// </summary>
    private IEnumerator Preview_CurrentState_OnGameover(State currentState)
    {
        yield return Utility.GetWaitForSeconds(Metrics.InactiveDelay);

        transform.position = Vector2.zero;

        switch (currentState)
        {
            case State.X:
                _animator.SetTrigger(Metrics.X);
                break;
            case State.O:
                _animator.SetTrigger(Metrics.O);
                break;
            case State.Draw:
                _animator.SetTrigger(Metrics.Draw);
                BoardManager.Instance.FadeOutBoard();
                break;
        }
    }

    private void OnDestroy()
    {
        GameManager.OnStateWon -= GameManagerOnStateWon;
    }
}
