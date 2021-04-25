using System;
using System.Collections.Generic;
using UnityEngine;

public class GameController : MonoBehaviour
{
    GameStats gameStats;

    [SerializeField]
    private GameObject ball;

    
    private void Awake() {
        ball = GameObject.FindGameObjectWithTag("ball");
    }
    void Start()
    {
        gameStats = new GameStats();
    }
    private void OnEnable() {
        Events.WallCollision += ScoreGoal;
        Events.WallCollision += RepositionBall;
    }
    private void OnDisable() {
        Events.WallCollision -= ScoreGoal;
        Events.WallCollision -= RepositionBall;
    }
    void ScoreGoal(WallType wallType){
        if(wallType==WallType.leftWall){
            gameStats.RightPlayerScore++;
        }
        else if(wallType==WallType.RightWall){
            gameStats.LeftPlayerScore++;
        }
    }
    private void RepositionBall(WallType obj)
    {
        ball.transform.position = gameStats.InitBallPosition;
    }
}
public class GameStats{

    public GameStats(){
        LeftPlayerScore = 0; 
        RightPlayerScore = 0;
        InitBallPosition = new Vector3(0,0,0);
    }
    public void ResetGame(){
        LeftPlayerScore = 0; 
        RightPlayerScore = 0;
    }
    public Vector3 InitBallPosition; 
    public int ScoreToWin = 10 ;
    private int defaultScore = 0;
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

