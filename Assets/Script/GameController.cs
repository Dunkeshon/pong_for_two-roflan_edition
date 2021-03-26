using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameController : MonoBehaviour
{
    
    // Start is called before the first frame update
    void Start()
    {
        
    }
    private void OnEnable() {
        BallController.WallCollision += ScoreGoal;
    }
    private void OnDisable() {
        BallController.WallCollision -= ScoreGoal;
    }
    // Update is called once per frame
    void Update()
    {
        
    }
    void ScoreGoal(WallType wallType){
        return;
    }
}
