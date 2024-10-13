using System.Collections;
using System.Collections.Generic;
using Assets.Scripts;
using UnityEngine;
using UnityEngine.Serialization;

public class SelectGameModeMenu : MonoBehaviour
{
    [SerializeField] private GameObject _gameView;
    [SerializeField] private GameObject _tapToStart;
    
    [SerializeField] private GameObject _setupFieldSize;

    [SerializeField] private GameObject _multiplayerView;
    [SerializeField] private GameObject _singlePlayerClickButton;
    
    public void SelectSingleplayer()
    {
       SetupGame();

       Manager.GameMode = GameModeEnum.Singleplayer;
    }

    public void SelectMultiplayer()
    {
       SetupGame();
       
       _multiplayerView.SetActive(true);
       _singlePlayerClickButton.SetActive(false);

       Manager.GameMode = GameModeEnum.Multiplayer;
    }

    private void SetupGame()
    {
        gameObject.SetActive(false);
        
        _setupFieldSize.SetActive(true);
    }
}
