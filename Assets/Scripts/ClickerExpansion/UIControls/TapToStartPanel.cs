using System;
using System.Collections;
using System.Collections.Generic;
using Assets.Scripts;
using UnityEngine;

public class TapToStartPanel : MonoBehaviour
{
    private GridGenerator _gridGenerator;
    private VictoryIndentifier _victoryIndentifier;

    private void Start()
    {
        _gridGenerator = FindObjectOfType<GridGenerator>();
        _victoryIndentifier = FindObjectOfType<VictoryIndentifier>();
    }

    public void TapToStartButton()
    {
        _gridGenerator.Initialize();
        _victoryIndentifier.GameStart();
        gameObject.SetActive(false);
    }
}
