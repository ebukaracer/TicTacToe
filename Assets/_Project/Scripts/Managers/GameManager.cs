using UnityEngine;
using Racer.Utilities;
using UnityEngine.Events;

internal class GameManager : SingletonPattern.StaticInstance<GameManager>
{
    // For Visualization purpose..
    [SerializeField] private GameStatus gameStatus;
    [SerializeField] private State won;
    [SerializeField] private State turn;
    //..

    public static event UnityAction<GameStatus> OnGameStatus;
    public static event UnityAction<State> OnStateWon;
    public static event UnityAction<State> OnStateTurn;


    public void SetGameStatus(GameStatus status)
    {
        gameStatus = status;

        OnGameStatus?.Invoke(status);
    }

    public void SetWinnerState(State state)
    {
        won = state;

        OnStateWon?.Invoke(state);

        SetGameStatus(GameStatus.Gameover);
    }

    public void SetCurrentState(State state)
    {
        turn = state;

        OnStateTurn?.Invoke(state);
    }
}