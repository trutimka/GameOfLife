using UnityEngine;

public class GameStateUIControls : MonoBehaviour
{
    [SerializeField] private GameObject _gameOverWindow;
    [SerializeField] private GameObject _victoryWindow;
    [SerializeField] private GameObject _gameViewAndControls;

    private VictoryIndentifier _gameOver;
    private Timer _timer;

    private void Start()
    {
        _gameOver = FindObjectOfType<VictoryIndentifier>();
        _timer = FindObjectOfType<Timer>();
        
        _gameOver.Victory += OnVictory;
        _timer.TimerElapsed += OnGameOver;
    }

    private void OnGameOver()
    {
        _gameViewAndControls.SetActive(false);
        _gameOverWindow.SetActive(true);
    }

    private void OnVictory()
    {
        _gameViewAndControls.SetActive(false);
        _victoryWindow.SetActive(true);
    }
}
