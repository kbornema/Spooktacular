using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Menu : MonoBehaviour 
{
    public string sceneName = "02_Lobby";

    public void StartNewGame()
    {
        SceneManager.LoadScene(sceneName);
    }
}
