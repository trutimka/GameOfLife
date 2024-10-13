using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerScore : MonoBehaviour
{
    [SerializeField] private int _score;

    public int Score => _score;

    public event Action ScoreChanged;

    public void AddScore(int additionAmount = 1)
    {
        _score += additionAmount;
        ScoreChanged?.Invoke();
    }

    public void RemoveScore(int removeAmount)
    {
        if (_score >= removeAmount)
        {
            _score -= removeAmount;
            ScoreChanged?.Invoke();
        }
}
}
