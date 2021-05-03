using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
public class InGameUiScripts : MonoBehaviour
{
    [SerializeField]
    GameObject gameController;
    GameController gameControllerScript;

    [SerializeField]
    GameObject PauseButton;
    [SerializeField]
    GameObject PauseCanvas;
    Transform pauseButtonTransform;
    private void Awake() {
        gameControllerScript = gameController.GetComponent<GameController>();
        pauseButtonTransform = PauseButton.GetComponent<Transform>();
    }
    private void Update() {
        if(Input.GetKey(KeyCode.Escape)){
            Pause();
        }
    }
    public void Resume(){
        Time.timeScale = 1;
        pauseButtonTransform.rotation = Quaternion.Euler(0,0,0);
        PauseCanvas.SetActive(false);
    }
    public void Restart(){
        gameControllerScript.StartNewMatch();
        Resume();
    }
    public void ReturnToMainMenu(){
        Resume();
        SceneManager.LoadScene("MainMenu");
    }
    public void Pause(){
        Time.timeScale = 0;
        pauseButtonTransform.rotation = Quaternion.Euler(0,0,90);
        PauseCanvas.SetActive(true);
    }


}
