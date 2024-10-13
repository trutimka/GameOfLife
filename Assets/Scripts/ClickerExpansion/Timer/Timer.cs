using System;
using Assets.Scripts;
using UnityEngine;
using Random = UnityEngine.Random;

public class Timer : MonoBehaviour
{
    private float _timeToFinish;
    private bool _isActive;

    public int TimeToEnd => (int)_timeToFinish;

    public event Action TimerChanged;
    public event Action TimerElapsed;
    
    private void Start()
    {
        _timeToFinish = Random.Range(40, 70);
        _isActive = true;
    }

    private void Update()
    {
        if (Manager.GameState == GameStateEnum.Run && _isActive)
        {
            _timeToFinish -= Time.deltaTime;
            
            if (_timeToFinish < 0)
            {
                _timeToFinish = 0;
                _isActive = false;
                TimerElapsed?.Invoke();
            }
            
            TimerChanged?.Invoke();
        }
    }
}
