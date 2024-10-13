using System;
using System.Collections;
using System.Collections.Generic;
using Assets.Scripts;
using UnityEngine;

public class PlayerScoreControls : MonoBehaviour
{
    private GridGenerator _gridGenerator;
    private PlayerScore _playerScore;
    
    private void Start()
    {
        _gridGenerator = FindObjectOfType<GridGenerator>();
        _playerScore = FindObjectOfType<PlayerScore>();
        
        _gridGenerator.CellRevived += AddScore;
    }

    private void AddScore()
    {
        _playerScore.AddScore();
    }
}
