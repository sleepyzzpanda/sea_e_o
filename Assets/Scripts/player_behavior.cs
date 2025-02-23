using System.Collections;
using System.Collections.Generic;
using UnityEngine;
// import math
using System;

public class player_behavior : MonoBehaviour
{
    // variable parking lot -----------------------
    public float speed = 5.0f;
    public Transform move_point;
    public LayerMask stop_movement, BOX;
    public Sprite sprite_normal_front, sprite_normal_back, sprite_normal_left, sprite_normal_right;
    public Sprite sprite_diving_front, sprite_diving_back, sprite_diving_left, sprite_diving_right;
    public bool is_diving;
    // ---------------------------------------------
    // Start is called before the first frame update
    void Start()
    {
        is_diving = false;
        move_point.parent = null;
    }

    // Update is called once per frame
    void Update()
    {
        // move player to move_point
        transform.position = Vector3.MoveTowards(transform.position, move_point.position, speed * Time.deltaTime);
        // if player is at move_point, player can move
        if(Vector3.Distance(transform.position, move_point.position) <= 0.05f){
            player_move();
        }
        
        
    }

    void player_move(){
        
        // if press the arrow keys, move
        if(Math.Abs(Input.GetAxisRaw("Horizontal")) == 1f){
            if(!Physics2D.OverlapCircle(move_point.position + new Vector3(Input.GetAxisRaw("Horizontal") * 0.5f, 0f, 0f), 0.15f, stop_movement) && !Physics2D.OverlapCircle(move_point.position + new Vector3(Input.GetAxisRaw("Horizontal") * 0.5f, 0f, 0f), 0.15f, BOX) )
            {
                move_point.position += new Vector3(Input.GetAxisRaw("Horizontal") * 0.5f, 0f, 0f);
                if(is_diving){
                    if(Input.GetAxisRaw("Horizontal") == 1f){
                        GetComponent<SpriteRenderer>().sprite = sprite_diving_right;
                    } else if(Input.GetAxisRaw("Horizontal") == -1f){
                        GetComponent<SpriteRenderer>().sprite = sprite_diving_left;
                    }
                } else {
                    if(Input.GetAxisRaw("Horizontal") == 1f){
                        GetComponent<SpriteRenderer>().sprite = sprite_normal_right;
                    } else if(Input.GetAxisRaw("Horizontal") == -1f){
                        GetComponent<SpriteRenderer>().sprite = sprite_normal_left;
                    }
                }
            }
        } else if(Math.Abs(Input.GetAxisRaw("Vertical")) == 1f){
            if(!Physics2D.OverlapCircle(move_point.position + new Vector3(0f, Input.GetAxisRaw("Vertical") * 0.5f, 0f), 0.15f, stop_movement) && !Physics2D.OverlapCircle(move_point.position + new Vector3(0f, Input.GetAxisRaw("Vertical") * 0.5f, 0f), 0.15f, BOX)){
                move_point.position += new Vector3(0f, Input.GetAxisRaw("Vertical") * 0.5f, 0f);
                if(is_diving){
                    if(Input.GetAxisRaw("Vertical") == 1f){
                        GetComponent<SpriteRenderer>().sprite = sprite_diving_back;
                    } else if(Input.GetAxisRaw("Vertical") == -1f){
                        GetComponent<SpriteRenderer>().sprite = sprite_diving_front;
                    }
                } else {
                    if(Input.GetAxisRaw("Vertical") == 1f){
                        GetComponent<SpriteRenderer>().sprite = sprite_normal_back;
                    } else if(Input.GetAxisRaw("Vertical") == -1f){
                        GetComponent<SpriteRenderer>().sprite = sprite_normal_front;
                    }
                }
            }
        }  
    }   
}
