using System.Collections;
using Racer.Utilities;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

internal class UiController : MonoBehaviour
{
    private Animator _animator;
    private ScoreManager _scoreManager;
    private BoardManager _boardManager;


    [Space(10),
    Header("TEXTS")]
    [SerializeField] private TextMeshProUGUI winnerText;
    [SerializeField] private TextMeshProUGUI countOText;
    [SerializeField] private TextMeshProUGUI countXText;
    [SerializeField] private TextMeshProUGUI turnText;
    [SerializeField] private TextMeshProUGUI stateText;
    [SerializeField] private TextMeshProUGUI hintText;

    [Space(10),
    Header("CANVAS GROUPS")]
    [SerializeField] private CanvasGroup xSelector;
    [SerializeField] private CanvasGroup oSelector;

    [Space(5),
    SerializeField]
    private float fadeSpeed = .5f;

    [Space(10),
     Header("BUTTONS")]
    [SerializeField] private Button restart;
    [SerializeField] private Button reset;
    [SerializeField] private Button exit;

    private void Awake()
    {
        _animator = GetComponent<Animator>();

        RegisterButtons();
    }

    private void Start()
    {
        _scoreManager = ScoreManager.Instance;
        _boardManager = BoardManager.Instance;

        SetScoreTexts();

        GameManager.OnStateWon += GameManagerOnStateWon;
        GameManager.OnStateTurn += GameManagerOnStateTurn;
    }

    private void RegisterButtons()
    {
        restart.onClick.AddListener(() =>
            LoadScene(SceneManager.GetActiveScene().buildIndex));

        reset.onClick.AddListener(ClearData);

        exit.onClick.AddListener(ExitGame);
    }

    private void GameManagerOnStateTurn(State currentState)
    {
        SetTurnTextOnPlaying(currentState);

        switch (currentState)
        {
            case State.Draw:
                StartCoroutine(DoChangeSelector(false, xSelector));
                StartCoroutine(DoChangeSelector(false, oSelector));
                break;

            case State.X:
                StartCoroutine(DoChangeSelector(true, xSelector));
                StartCoroutine(DoChangeSelector(false, oSelector));
                break;

            case State.O:
                StartCoroutine(DoChangeSelector(true, oSelector));
                StartCoroutine(DoChangeSelector(false, xSelector));
                break;
        }
    }

    /// <summary>
    /// Marker that indicates the current state(X or O),
    /// </summary>
    private IEnumerator DoChangeSelector(bool isState, CanvasGroup selector)
    {
        if (!isState)
        {
            while (selector.alpha > 0)
            {
                yield return 0;

                selector.alpha -= fadeSpeed * Time.deltaTime;
            }

            selector.alpha = 0;
        }
        else
        {
            while (selector.alpha <= 0)
            {
                yield return 0;

                selector.alpha += fadeSpeed * Time.deltaTime;
            }

            selector.alpha = 1;
        }
    }


    private void GameManagerOnStateWon(State currentState)
    {
        _animator.SetTrigger(Metrics.Win);

        SetTurnTextOnGameover();

        StartCoroutine(DoChangeSelector(false, xSelector));
        StartCoroutine(DoChangeSelector(false, oSelector));

        switch (currentState)
        {
            case State.Draw:
                winnerText.text = "DRAW";
                break;
            case State.X:
                _scoreManager.SetXCount(1);
                break;
            case State.O:
                _scoreManager.SetOCount(1);
                break;
        }

        SetScoreTexts();
    }

    private void SetScoreTexts()
    {
        if (_scoreManager.GetOCount != 0)
            countOText.text = $"{_scoreManager.GetOCount}";

        if (_scoreManager.GetXCount != 0)
            countXText.text = $"{_scoreManager.GetXCount}";
    }

    private void SetTurnTextOnGameover()
    {
        stateText.gameObject.ToggleActive(false);

        turnText.text = "Game Over";
    }

    private void SetTurnTextOnPlaying(State state)
    {
        if (_boardManager.IsGameStarted)
            hintText.text = string.Empty;

        if (!string.IsNullOrEmpty(hintText.text))
            return;

        turnText.text = "Turn";

        switch (state)
        {
            case State.X:
                stateText.text = "X";
                break;
            case State.O:
                stateText.text = "O";
                break;
        }
    }

    public void SelectX() => Select(State.X);
    public void SelectO() => Select(State.O);

    private void Select(State state)
    {
        if (_boardManager.IsGameStarted || _boardManager.GetCurrentState() == state)
            return;

        GameManagerOnStateTurn(state);

        _boardManager.AutoPlayAI();

        _boardManager.SetCurrentState(state);
    }

    private void ClearData()
    {
        _scoreManager.ClearData();

        LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    private static void LoadScene(int index = 0)
    {
        SceneManager.LoadScene(index);
    }

    private static void ExitGame()
    {
#if !UNITY_EDITOR
        Application.Quit();
#endif
    }

    private void OnDestroy()
    {
        GameManager.OnStateWon -= GameManagerOnStateWon;
        GameManager.OnStateTurn -= GameManagerOnStateTurn;
    }
}
