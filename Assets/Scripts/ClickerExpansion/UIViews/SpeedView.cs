using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpeedView : UIView
{
    private PlaySpeedModifier _playSpeedModifier;
    
    protected override void Start()
    {
        base.Start();

        _playSpeedModifier = FindObjectOfType<PlaySpeedModifier>();
        _playSpeedModifier.PlaybackSpeedUpdated += UpdateView;
        
        UpdateView();
    }

    protected override void UpdateView()
    {
        base.UpdateView();

        if (_playSpeedModifier.PlaybackSpeed < 100f)
        {
            _text.text = $"Speed: {string.Format("{0:0.00}", _playSpeedModifier.PlaybackSpeed)}";
        }
        else
        {
            _text.text = $"Speed: {(int)_playSpeedModifier.PlaybackSpeed}";
        }
        

    }
}
