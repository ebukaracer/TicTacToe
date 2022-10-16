using System.Collections;
using Racer.Utilities;
using UnityEngine;

internal class BoardManager : SingletonPattern.Singleton<BoardManager>
{
    private bool _isTurn;
    private bool _isAiBusy;
    private bool _isGameStarted;
    private bool _isGameEnded;

    private IEnumerator _coroutine;

    private State[] _states;
    private State _currentState;

    private Merger _merger;
    private Mover _mover;
    private Animator _animator;
    private GameManager _gameManager;

    [SerializeField] private LineDrawer lineDrawer;

    [Header("PROPERTIES")]
    [SerializeField] private int playCount;

    [Space(5)]
    [SerializeField] private Sprite spriteX;
    [SerializeField] private Sprite spriteO;

    [Space(5)]
    [SerializeField] private LayerMask clickable;

    [Header("REFERENCES")]
    [SerializeField] private Ai aiController;
    [SerializeField] private PlayerController playerController;


    // Prevents setting this property more than once.
    public bool IsGameStarted
    {
        get => _isGameStarted;
        set
        {
            if (_isGameStarted == value)
                return;

            _isGameStarted = value;
            _gameManager.SetGameStatus(GameStatus.Playing);
        }
    }

    protected override void Awake()
    {
        base.Awake();

        _animator = GetComponent<Animator>();
        _merger = GetComponent<Merger>();
        _mover = GetComponent<Mover>();
    }

    private void Start()
    {
        _states = new State[9];

        _gameManager = GameManager.Instance;

        GameManager.OnGameStatus += GameManagerOnGameStatus;

        _currentState = State.X;
        _gameManager.SetCurrentState(_currentState);
    }

    private void GameManagerOnGameStatus(GameStatus status)
    {
        _isGameEnded = status.Equals(GameStatus.Gameover);
    }

    private void Update()
    {
        if (_isGameEnded)
            return;

        // Player's Turn
        if (Input.GetMouseButtonUp(0) && !_isTurn && !_isAiBusy)
            PlayerTurn();

        // AI's Turn
        if (!_isTurn) return;

        if (_coroutine != null)
            StopCoroutine(_coroutine);

        _coroutine = DelayBeforeAiPlay();

        StartCoroutine(_coroutine);

        _isTurn = !_isTurn;
    }

    private void PlayerTurn()
    {
        if (!playerController.GetHitInfo(_states, _currentState, SetSprite(), clickable))
            return;

        _isTurn = !_isTurn;

        IsGameStarted = true;

        SyncState();
    }

    private void AiTurn()
    {
        aiController.GetHitInfo(_states, _currentState, SetSprite(), clickable);

        IsGameStarted = true;

        SyncState();
    }

    private IEnumerator DelayBeforeAiPlay()
    {
        if (_isGameEnded)
            yield break;

        _isAiBusy = true;

        yield return Utility.GetWaitForSeconds(aiController.WaitDelay);

        AiTurn();

        _isAiBusy = false;
    }

    private bool IsCompleted()
    {
        if (CheckEqualRowAndColumn())
        {
            // Game over on Win
            _gameManager.SetWinnerState(_currentState);

            return true;
        }

        if (playCount == _states.Length)
        {
            // Game over on Draw
            _currentState = State.Draw;

            _gameManager.SetWinnerState(_currentState);

            return true;
        }

        return false;
    }

    private void SyncState()
    {
        playCount++;

        if (IsCompleted())
            return;

        _currentState = ReverseState();

        _gameManager.SetCurrentState(_currentState);
    }

    private bool CheckEqualRowAndColumn()
    {
        return
                  AreEqual(0, 1, 2) || AreEqual(3, 4, 5) || AreEqual(6, 7, 8) // Horizontal
               || AreEqual(0, 3, 6) || AreEqual(1, 4, 7) || AreEqual(2, 5, 8) // Vertical
               || AreEqual(0, 4, 8) || AreEqual(2, 4, 6); // Diagonal

    }

    private bool AreEqual(int v1, int v2, int v3)
    {
        var state = _currentState;

        var matched = _states[v1] == state && _states[v2] == state && _states[v3] == state;

        if (matched)
        {
            _merger.Collapse(v1, v2, v3);

            _mover.MoveToCenter(v2);

            lineDrawer.DrawLine(transform.GetChild(v1)
                    .position,
                transform.GetChild(v3)
                    .position,
                transform.GetChild(v2)
                    .position);

            StartCoroutine(SetInactiveOnGameover(v1, v2, v3));
        }

        return matched;
    }

    private IEnumerator SetInactiveOnGameover(int a, int b, int c)
    {
        yield return Utility.GetWaitForSeconds(Metrics.InactiveDelay);

        FadeOutBoard();

        transform.GetChild(a).gameObject.ToggleActive(false);
        transform.GetChild(b).gameObject.ToggleActive(false);
        transform.GetChild(c).gameObject.ToggleActive(false);
    }

    private State ReverseState() => (_currentState == State.X) ? State.O : State.X;

    public State GetCurrentState() => _currentState;

    private Sprite SetSprite() => (_currentState == State.X) ? spriteX : spriteO;

    public void SetCurrentState(State state) => _currentState = state;

    public void AutoPlayAI() => AiTurn();

    public void FadeOutBoard() => _animator.SetTrigger(Metrics.Fade);


    private void OnDestroy()
    {
        GameManager.OnGameStatus -= GameManagerOnGameStatus;
    }
}
