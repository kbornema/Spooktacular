using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Menu : MonoBehaviour 
{   
    public void StartNewGame()
    {
        SceneManager.LoadScene("02_Lobby");
    }
}
