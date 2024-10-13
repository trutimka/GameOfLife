using System;
using System.Collections;
using UnityEngine;
using Assets.Scripts;

public class PlaySpeedModifier : MonoBehaviour
{
    private float _defaultPlaybackSpeed = 2;
    private float _currentModifier = 1;

    [SerializeField] private float _debuffCooldown = 1;
    [SerializeField] private float _buffPercent = 1.01f;
    [SerializeField] private float _debuffPercent = 0.1f;

    public event Action PlaybackSpeedUpdated;
    
    public float PlaybackSpeed => _defaultPlaybackSpeed * _currentModifier;

    private void Start()
    {
        StartCoroutine(Debuff());
    }

    public void SpeedUp()
    {
        float modifier = Manager.GameMode == GameModeEnum.Multiplayer ? _buffPercent + 0.01f : _buffPercent;
        
        _currentModifier *= modifier;
        PlaybackSpeedUpdated?.Invoke();
    }

    public void SlowDownMultiplayer()
    {
        _currentModifier -= _currentModifier * (_debuffPercent / 2);
        PlaybackSpeedUpdated?.Invoke();
    }

    private IEnumerator Debuff()
    {
        if (Manager.GameMode == GameModeEnum.Singleplayer)
        {
            if (Manager.GameState == GameStateEnum.Run)
            {
                _currentModifier -= _currentModifier * _debuffPercent;
                PlaybackSpeedUpdated?.Invoke();
            }

            yield return new WaitForSeconds(_debuffCooldown);
            StartCoroutine(Debuff());
        }
    }
}
