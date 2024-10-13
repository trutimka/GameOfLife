using Assets.Scripts;
using UnityEngine;

public class PlaybackSpeedControls : MonoBehaviour
{
    private PlaySpeedModifier _playSpeedModifier;

    private void Start()
    {
        _playSpeedModifier = FindObjectOfType<PlaySpeedModifier>();
    }

    private void Update()
    {
        if (Manager.GameMode == GameModeEnum.Singleplayer)
        {
            if (Input.GetKeyDown(KeyCode.Space))
            {
                HandleSpeedUpInput();
            }
        }

        if (Manager.GameMode == GameModeEnum.Multiplayer)
        {
            if (Input.GetKeyDown(KeyCode.A))
            {
                HandleSlowDownInput();
            }

            if (Input.GetKeyDown(KeyCode.L))
            {
                HandleSpeedUpInput();
            }
        }
    }

    public void HandleSpeedUpInput()
    {
        _playSpeedModifier.SpeedUp();
    }

    public void HandleSlowDownInput()
    {
        _playSpeedModifier.SlowDownMultiplayer();
    }
}
