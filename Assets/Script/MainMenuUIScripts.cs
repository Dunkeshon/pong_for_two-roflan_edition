using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuUIScripts : MonoBehaviour
{
    public void LoadLocalMultiplayerScene(){
        SceneManager.LoadScene("Local Multiplayer");
    }
    public void QuitGame(){
        Application.Quit();
    }
}
