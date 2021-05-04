using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;
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

    [SerializeField]
    GameObject Winninglabel;
    [SerializeField]
    GameObject WinningCanvas;

    TextMeshProUGUI winningTextUI; 
    private void OnEnable() {
        Events.PlayerWon += ShowWinnerLabel;
    }
    private void OnDisable() {
        Events.PlayerWon -= ShowWinnerLabel;
    }
    private void Awake() {
        gameControllerScript = gameController.GetComponent<GameController>();
        pauseButtonTransform = PauseButton.GetComponent<Transform>();
        winningTextUI = Winninglabel.GetComponent<TextMeshProUGUI>();
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
        if(WinningCanvas.activeSelf==true){
            WinningCanvas.SetActive(false);
        }
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

    public void ShowWinnerLabel(PlayerType playerType)
    {
        string winnerText;
        if(playerType==PlayerType.Left){
            winnerText = "Left player Win!";
        }
        
        else if(playerType==PlayerType.Right){
            winnerText = "Right player Win!";
        }
        else{
            winnerText = "bug win! lmao";
        }
        WinningCanvas.SetActive(true);
        winningTextUI.text = winnerText;
    }

}
