using System;
using System.Collections;
using UnityEngine;
using TMPro;

public class GameController : MonoBehaviour
{
    public GameStats gameStats;

    [SerializeField]
    private GameObject ball;
    public BallController BallController;

    [SerializeField]
    private GameObject leftPlayerScoreUiHandler;
    
    private TextMeshProUGUI leftPlayerScoreUiText; 

    [SerializeField]
    private GameObject rightPlayerScoreUiHandler;

    private TextMeshProUGUI rightPlayerScoreUiText; 

    private IEnumerator increaseSpeedRoutine;


    

    private void Awake() {
        ball = GameObject.FindGameObjectWithTag("ball");
        BallController = ball.GetComponent<BallController>();    
        leftPlayerScoreUiText = leftPlayerScoreUiHandler.GetComponent<TextMeshProUGUI>();
        rightPlayerScoreUiText = rightPlayerScoreUiHandler.GetComponent<TextMeshProUGUI>();
    }
    void Start()
    {
        gameStats = new GameStats();
        increaseSpeedRoutine = IncreaseBallSpeedRoutine();
        //temporary
        StartNewMatch();
    }
    private void OnEnable() {

        Events.WallCollision += GoalScored;
        Events.ScoreChanged += UpdateUiscore;
        Events.PlayerWon += HideBall;
        Events.BallOutOfPlayField+=StartNewRound;
    }

    private void WaitAndStartRound(float seconds)
    {
        StartCoroutine(WaitToStartRoundRoutine(seconds));
    }
    IEnumerator WaitToStartRoundRoutine(float seconds){
        ball.SetActive(false);
        yield return new WaitForSeconds(seconds);
        ball.SetActive(true);
        StartNewRound();
    }

    private void OnDisable() {
        Events.WallCollision -= GoalScored;
        Events.ScoreChanged  -= UpdateUiscore;
        Events.PlayerWon -= HideBall;
        Events.BallOutOfPlayField-=StartNewRound;
    }

    private void GoalScored(WallType wallType)
    {
        ScoreGoal(wallType);
        if(gameStats.LeftPlayerScore==gameStats.ScoreToWin)
        {
            Events.PlayerWon?.Invoke(PlayerType.Left);
            return;
        }
        else if(gameStats.RightPlayerScore==gameStats.ScoreToWin){
            Events.PlayerWon?.Invoke(PlayerType.Right);
        }
        else{
            WaitAndStartRound(1f);
            AnimateScoreChange(wallType);
            //StartNewRound();
        }
    }

    private void AnimateScoreChange(WallType wallType)
    {
        if(wallType==WallType.LeftWall){
          //  ScoreHandlerAnimation();
        }
        else if(wallType==WallType.RightWall){

        }
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
    void ScoreGoal(WallType wallType){
        if(wallType==WallType.LeftWall){
            gameStats.RightPlayerScore++;
        }
        else if(wallType==WallType.RightWall){
            gameStats.LeftPlayerScore++;
        }
    }

    
    public void StartNewMatch(){
        ResetScore();
        if(ball.activeSelf==false){
            ball.SetActive(true);
        }
        StartNewRound();
    }
    public void StartNewRound()
    {
        if (ball.activeSelf == false)
        {
            return;
        }
        StopSpeedIncreasing();
        RepositionBall();
        ResetBallSpeed();
        StartGameDelayAnimation();
        StartCoroutine(increaseSpeedRoutine);
    }

    private void StopSpeedIncreasing()
    {
        StopCoroutine(increaseSpeedRoutine);
    }

    private IEnumerator IncreaseBallSpeedRoutine(){
        while(true){
            if(BallController.BallSpeed<gameStats.MaxBallSpeed){
                BallController.BallSpeed += 1;
                Debug.Log("New speed is "+ BallController.BallSpeed);
                yield return new WaitForSeconds(3);
            }
            else{
                yield return new WaitForSeconds(3);
            }
        }
    }

    private void HideBall(PlayerType player = PlayerType.NotSetted)
    {
        ball.SetActive(false);
    }

    private void StartGameDelayAnimation()
    {
        BallController.PlayFadeAnimation();
        StartCoroutine(BallController.WaitAndStartMoving());
    }


    private void ResetBallSpeed(){
        BallController.rb.velocity = gameStats.StartVelocity;
        BallController.BallSpeed = gameStats.DefaultBallSpeed;
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
    public int DefaultBallSpeed = 11;
    public Vector3 StartVelocity;
    public int ScoreToWin = 2 ;
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
            else if(value<=ScoreToWin){
                leftPlayerScore = value;
                Events.ScoreChanged?.Invoke(PlayerType.Left,leftPlayerScore);
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
            else if(value<=ScoreToWin){
                rightPlayerScore = value;
                Events.ScoreChanged?.Invoke(PlayerType.Right,rightPlayerScore);
            }
        }
    }
    
}

public static class Events{
    public static Action<PlayerType> PlayerWon;
    public static Action<WallType> WallCollision;
    public static Action<PlayerType,int> ScoreChanged;
    public static Action BallOutOfPlayField;
    
}