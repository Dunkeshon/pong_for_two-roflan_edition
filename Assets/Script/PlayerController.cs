using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[RequireComponent(typeof(Rigidbody2D))]
public class PlayerController : MonoBehaviour
{   
    

    public PlayerType playerType = PlayerType.NotSetted ;
    public KeyCode inputUp;
    public KeyCode inputDown;
    private Rigidbody2D rb;

    public float speed = 8f; //5.5f;

    private float topScreenBorder = 4.25f;
    private float bottomScreenBorder = -4.25f;
    private void Awake() {
        rb = GetComponent<Rigidbody2D>();
    }
    // Start is called before the first frame update
    void Start()
    {
        SetControls();
    }

    private void Update() {
        Move();
    }
    private void Move(){
        if(Input.GetKey(inputUp)){
            rb.MovePosition(new Vector2 (transform.position.x,
                            Mathf.Clamp(transform.position.y + 1 * speed * Time.fixedDeltaTime,bottomScreenBorder,topScreenBorder)));
        }
        if(Input.GetKey(inputDown)){
            rb.MovePosition(new Vector2 (transform.position.x,
                            Mathf.Clamp(transform.position.y + -1 * speed * Time.fixedDeltaTime,bottomScreenBorder,topScreenBorder)));
        }
    } 

    private void SetControls(){
        if(playerType==PlayerType.NotSetted){
            Debug.Log("Player Type and controls not setted",gameObject);
            return;
        }
        else if(playerType==PlayerType.Left){
            inputUp = KeyCode.W;
            inputDown = KeyCode.S;
        }
        else if(playerType==PlayerType.Right){
            inputUp = KeyCode.UpArrow;
            inputDown = KeyCode.DownArrow;
        }
        else if(playerType==PlayerType.Custom){
            if(inputUp==KeyCode.None && inputDown==KeyCode.None){
                Debug.Log("Player controls not setted",gameObject);
            }
        }
    }
    
}
