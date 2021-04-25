using System;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class BallController : MonoBehaviour
{
    private float speed = 14.0f;
    private Rigidbody2D rb;
    private AudioSource audioSource;
    [SerializeField]
    private AudioClip [] audioClipArray;
    private Vector2 initPosition; 
    private void Awake() {
       rb=GetComponent<Rigidbody2D>();
       audioSource = GetComponent<AudioSource>();
    }
    void Start()
    {
        initPosition = new Vector2(0,0);
        StartMoving();
    }

 
    private void OnTriggerEnter2D(Collider2D other) {
        if(other.gameObject.tag=="right wall"){
            Events.WallCollision?.Invoke(WallType.RightWall);
        }
        else if(other.gameObject.tag=="left wall"){
            Events.WallCollision?.Invoke(WallType.leftWall);
        }
        if(other.gameObject.tag == "Player"){
            DirectionChangeByPlayer();
            PlayHitSound();
        }
        if(other.gameObject.tag == "border"){
            BorderTouch();
        }
    }

    private void PlayHitSound()
    {
       audioSource.PlayOneShot(GetRandomHitAudio(),1f);
    }
    private AudioClip GetRandomHitAudio(){
        return audioClipArray[UnityEngine.Random.Range(0, audioClipArray.Length)];
    }

    private void StartMoving(){
        int randomX = randomizeNumberWithException(-1,1,0);
        
        float randomY = 0;

        Vector2 direction = new Vector2(randomX, randomY);
        rb.velocity = direction.normalized * speed;
      
    }
    private void BorderTouch(){
        Vector2 direction = new Vector2(rb.velocity.x, -rb.velocity.y + UnityEngine.Random.Range(-0.1f,0.1f));
        rb.velocity = direction.normalized * speed;
       
    }
    private void DirectionChangeByPlayer(){
        Vector2 direction = new Vector2(-rb.velocity.x, UnityEngine.Random.Range(-5f,5f));
        var tempNormalized = direction.normalized * speed;
        rb.velocity = direction.normalized * speed;
      
    }

    private int randomizeNumberWithException(int beginInclusive,int endInclusive,int exception){

        int generatedNumber = UnityEngine.Random.Range(beginInclusive,endInclusive+1);
        if(generatedNumber==exception){
            generatedNumber = randomizeNumberWithException(beginInclusive,endInclusive,exception);
        }
        return generatedNumber;
    }
   
}
