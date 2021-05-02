using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class BallController : MonoBehaviour
{
    GameController gameController;

    Animation anim;
    private int ballSpeed;

    public int BallSpeed{
        get{return ballSpeed;}
        set{
            if(value>gameController.gameStats.MaxBallSpeed){
                return;
            }
            else{
                ballSpeed=value;
            }
        }
    }
    public Rigidbody2D rb;
    private AudioSource audioSource;
    [SerializeField]
    private AudioClip [] audioClipArray;

    
    private void Awake() {
        anim = GetComponent<Animation>();
        rb=GetComponent<Rigidbody2D>();
        audioSource = GetComponent<AudioSource>();
        gameController = GameObject.FindGameObjectWithTag("GameController").GetComponent<GameController>();
    }

    private void OnTriggerEnter2D(Collider2D other) {
        if(other.gameObject.tag=="rightGate"){
            Events.WallCollision?.Invoke(WallType.RightWall);
        }
        else if(other.gameObject.tag=="left gate"){
            Events.WallCollision?.Invoke(WallType.LeftWall);
        }
        if(other.gameObject.tag == "Player"){
            DirectionChangeByPlayer();
            PlayHitSound();
        }
        if(other.gameObject.tag == "border"){
            BorderTouch();
        }
    }
    private void OnTriggerExit2D(Collider2D other) {
        if(other.gameObject.tag=="LevelRestriction"){
            Events.BallOutOfPlayField?.Invoke();
        }
    }
    public void PlayFadeAnimation(){
        anim.Play("startGameAnim");
    }
    public IEnumerator WaitAndStartMoving(){
        yield return new WaitForSeconds(2);
        anim.Stop();
        StartMoving();
    }
    private void PlayHitSound()
    {
       audioSource.PlayOneShot(GetRandomHitAudio(),1f);
    }
    private AudioClip GetRandomHitAudio(){
        return audioClipArray[UnityEngine.Random.Range(0, audioClipArray.Length)];
    }

   // use in gamecontroller
    public void StartMoving(){
        int randomX = randomizeNumberWithException(-1,1,0);
        
        float randomY = 0;

        Vector2 direction = new Vector2(randomX, randomY);
        rb.velocity = direction.normalized * BallSpeed;
      
    }
    private void BorderTouch(){
        Vector2 direction = new Vector2(rb.velocity.x, -rb.velocity.y);
        rb.velocity = direction.normalized * BallSpeed;
       
    }
    private void DirectionChangeByPlayer(){
        Vector2 direction = new Vector2(-rb.velocity.x, UnityEngine.Random.Range(-5f,5f));
        var tempNormalized = direction.normalized * BallSpeed;
        rb.velocity = direction.normalized * BallSpeed;
      
    }

    private int randomizeNumberWithException(int beginInclusive,int endInclusive,int exception){

        int generatedNumber = UnityEngine.Random.Range(beginInclusive,endInclusive+1);
        if(generatedNumber==exception){
            generatedNumber = randomizeNumberWithException(beginInclusive,endInclusive,exception);
        }
        return generatedNumber;
    }
   
}
