using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScoreView : UIView
{
    protected PlayerScore _playerScore;
    
    protected override void Start()
    {
        base.Start();
        
        _playerScore = FindObjectOfType<PlayerScore>();
        _playerScore.ScoreChanged += UpdateView;
        
        UpdateView();
    }

    protected override void UpdateView()
    {
        base.UpdateView();

        _text.text = $"Score: {_playerScore.Score}";
    }
}
