using System;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class GameController : MonoBehaviour
{
    public GameStats gameStats;

    [SerializeField]
    private GameObject ball;
    private BallController ballController;

    [SerializeField]
    private GameObject leftPlayerScoreUiHandler;
    
    private TextMeshProUGUI leftPlayerScoreUiText; 

    [SerializeField]
    private GameObject rightPlayerScoreUiHandler;

    private TextMeshProUGUI rightPlayerScoreUiText; 

    

    private void Awake() {
        ball = GameObject.FindGameObjectWithTag("ball");
        ballController = ball.GetComponent<BallController>();    
        leftPlayerScoreUiText = leftPlayerScoreUiHandler.GetComponent<TextMeshProUGUI>();
        rightPlayerScoreUiText = rightPlayerScoreUiHandler.GetComponent<TextMeshProUGUI>();
    }
    void Start()
    {
        gameStats = new GameStats();
        //temporary
        StartNewMatch();
    }
    private void OnEnable() {
        Events.WallCollision += ScoreGoal;
        Events.WallCollision += StartNewRound;
        Events.ScoreChanged += UpdateUiscore;
        // Events.PlayerWon += 
    }

    private void UpdateUiscore(PlayerType playerType, int newScore)
    {
        if(playerType == PlayerType.Left){
            leftPlayerScoreUiText.text = newScore.ToString();
        }
        else if(playerType == PlayerType.Right){
            rightPlayerScoreUiText.text = newScore.ToString();
        }
        else{
            Debug.Log("don't know how to output unknown player type's score");
        }
    }

    private void OnDisable() {
        Events.WallCollision -= ScoreGoal;
        Events.WallCollision -= StartNewRound;
        Events.ScoreChanged  -= UpdateUiscore;
    }


    void ScoreGoal(WallType wallType){
        if(wallType==WallType.leftWall){
            gameStats.RightPlayerScore++;
        }
        else if(wallType==WallType.RightWall){
            gameStats.LeftPlayerScore++;
        }
    }

    
    public void StartNewMatch(){
        ResetScore();
        StartNewRound();
    }
    //param to correspond signal
    public void StartNewRound(WallType obj){
        if(gameStats.LeftPlayerScore==gameStats.ScoreToWin||gameStats.RightPlayerScore==gameStats.ScoreToWin)
        {
            return;
        }
        RepositionBall();
        ballController.BallSpeed = gameStats.DefaultBallSpeed;
        ballController.rb.velocity = gameStats.StartVelocity;
        ballController.PlayFadeAnimation();
        StartCoroutine(ballController.WaitAndStartMoving());
    }
    //same method but without param
    public void StartNewRound(){
        if(gameStats.LeftPlayerScore==gameStats.ScoreToWin||gameStats.RightPlayerScore==gameStats.ScoreToWin)
        {
            return;
        }
        RepositionBall();
        ballController.BallSpeed = gameStats.DefaultBallSpeed;
        ballController.PlayFadeAnimation();
        StartCoroutine(ballController.WaitAndStartMoving());
    }
    
    private void RepositionBall()
    {
        ball.transform.position = gameStats.InitBallPosition;
    }
    public void ResetScore(){
        gameStats.LeftPlayerScore = gameStats.DefaultScore; 
        gameStats.RightPlayerScore = gameStats.DefaultScore;
    }
}
public class GameStats{

    public GameStats(){
        LeftPlayerScore = DefaultScore; 
        RightPlayerScore = DefaultScore;
        InitBallPosition = new Vector3(0,0,0);
        StartVelocity = new Vector3(0,0,0);
    }
    
    public Vector3 InitBallPosition; 

    public int MaxBallSpeed = 20;
    public int DefaultBallSpeed = 9;
    public Vector3 StartVelocity;
    public int ScoreToWin = 10 ;
    public int DefaultScore = 0;
    private int leftPlayerScore;
    private int rightPlayerScore;

    //i`m a dumbass 
    //contains action calls
    public int LeftPlayerScore{
        get{
            return leftPlayerScore;
        }
        set{
            if(value>ScoreToWin){
                Debug.Log("Score exceeded ScoreToWin");
                return;
            }
            else if(value<ScoreToWin){
                leftPlayerScore = value;
                Events.ScoreChanged?.Invoke(PlayerType.Left,leftPlayerScore);
            }
            if(value==ScoreToWin){
                Events.ScoreChanged?.Invoke(PlayerType.Left,leftPlayerScore);
                // new function which show winner label
                Events.PlayerWon?.Invoke(PlayerType.Left);
            }
        }
    }
    //contains action calls
    public int RightPlayerScore{
        get{
            return rightPlayerScore;
        }
        set{
            if(value>ScoreToWin){
                Debug.Log("Score exceeded ScoreToWin");
                return;
            }
            else if(value<ScoreToWin){
                rightPlayerScore = value;
                Events.ScoreChanged?.Invoke(PlayerType.Right,rightPlayerScore);
            }
            if(value==ScoreToWin){
                rightPlayerScore = value;
                Events.ScoreChanged?.Invoke(PlayerType.Right,rightPlayerScore);
                Events.PlayerWon?.Invoke(PlayerType.Right);
            }
        }
    }
    
}

public static class Events{
    public static Action<PlayerType> PlayerWon; 
    public static Action<WallType> WallCollision;
    public static Action<PlayerType,int> ScoreChanged;
    
}

