using System;
using Assets.Scripts;
using UnityEngine;

public class VictoryIndentifier : MonoBehaviour
{
    private PlayerScore _score;
    private PlaySpeedModifier _playSpeed;

    private float _timeSinceScoreChange = 0;
    private float _maxScoreBeingConstThreshold = 1;

    private int _frameThreshold = 10;
    private bool _isGameOver = true;

    public event Action Victory;

    private void Start()
    {
        _score = FindObjectOfType<PlayerScore>();
        _playSpeed = FindObjectOfType<PlaySpeedModifier>();

        UpdateConstThreshold();
        
        _playSpeed.PlaybackSpeedUpdated += UpdateConstThreshold;
        
        _score.ScoreChanged += ResetTimer;
    }

    private void UpdateConstThreshold()
    {
        _maxScoreBeingConstThreshold = _frameThreshold * (1 / _playSpeed.PlaybackSpeed);
    }

    public void GameStart()
    {
        _isGameOver = false;
    }

    private void Update()
    {
        if (_isGameOver)
        {
            return;
        }
        
        _timeSinceScoreChange += Time.deltaTime;

        if (_timeSinceScoreChange >= _maxScoreBeingConstThreshold)
        {
            OnGameWin();
        }
    }

    public void OnGameWin()
    {
        if (_isGameOver)
        {
            return;
        }

        _isGameOver = true;
        
        Manager.GameState = GameStateEnum.GameEnd;
        Victory?.Invoke();
    }

    private void ResetTimer()
    {
        _timeSinceScoreChange = 0;
    }
}
