using UnityEngine;
using System.Collections;

public class MenuFunctions : MonoBehaviour {

    public void ChangeScene(string sceneName)
    {
        Application.LoadLevel(sceneName);
    }

    public void Exit()
    {
        Application.Quit();
    }
}
