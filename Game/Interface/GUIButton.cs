using UnityEngine;
using System.Collections;

public class GUIButton : MonoBehaviour {

    public GameObject pauseMenu;
    private GameState _oldState;

    public void Pause()
    {
        if (Gamesystem.gameState != GameState.PAUSED)
        {
            _oldState = Gamesystem.gameState;
            Time.timeScale = 0;
            pauseMenu.SetActive(true);
            Gamesystem.gameState = GameState.PAUSED;
        }
        else
        {
            Gamesystem.gameState = _oldState;
            Time.timeScale = 1;
            pauseMenu.SetActive(false);
        }
    }

    public void ChangeScene(string sceneName)
    {
        Application.LoadLevel(sceneName);
    }
}
