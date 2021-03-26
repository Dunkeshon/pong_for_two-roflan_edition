using System;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class BallController : MonoBehaviour
{
    private float speed = 10.0f;
    public static Action<WallType> WallCollision;
    private Rigidbody2D rb;
    
    private Vector2 initPosition; 
    private void Awake() {
       rb=GetComponent<Rigidbody2D>();
    }
    void Start()
    {
        initPosition = new Vector2(0,0);
        StartMoving();
    }

    void Update()
    {
        
    }
    private void OnTriggerEnter2D(Collider2D other) {
        if(other.gameObject.tag=="right wall"){
            if(WallCollision!=null){
                WallCollision(WallType.RightWall);
            }
        }
        else if(other.gameObject.tag=="left wall"){
            if(WallCollision!=null){
                WallCollision(WallType.leftWall);
            }
        }
        if(other.gameObject.tag == "Player"){
            PlayerTouch();
        }
        if(other.gameObject.tag == "border"){
            BorderTouch();
        }
    }
    private void StartMoving(){
        int randomX = UnityEngine.Random.Range(-1,2);
        print("x generated: " + randomX);
        float randomY = 0;
        //float randomY = UnityEngine.Random.Range(-1f,1.1f);
        print("x generated: " + randomY);

        Vector2 direction = new Vector2(randomX*speed, randomY);
        print("Vector created"+ direction);
        rb.velocity = direction;
    }
    private void BorderTouch(){
        Vector2 direction = new Vector2(rb.velocity.x, -rb.velocity.y + UnityEngine.Random.Range(-0.1f,0.1f));
        rb.velocity = direction;
    }
    private void PlayerTouch(){
        Vector2 direction = new Vector2(-rb.velocity.x, UnityEngine.Random.Range(-2f,2.001f));
        rb.velocity = direction;
    }
   
}
