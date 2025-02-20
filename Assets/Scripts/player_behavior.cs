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
    public LayerMask stop_movement;
    public GameObject sprite;
    // ---------------------------------------------
    // Start is called before the first frame update
    void Start()
    {
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
            if(!Physics2D.OverlapCircle(move_point.position + new Vector3(Input.GetAxisRaw("Horizontal") * 0.5f, 0f, 0f), 0.1f, stop_movement))
            {
                move_point.position += new Vector3(Input.GetAxisRaw("Horizontal") * 0.5f, 0f, 0f);
            }
        } else if(Math.Abs(Input.GetAxisRaw("Vertical")) == 1f){
            if(!Physics2D.OverlapCircle(move_point.position + new Vector3(0f, Input.GetAxisRaw("Vertical") * 0.5f, 0f), 0.2f, stop_movement)){
                move_point.position += new Vector3(0f, Input.GetAxisRaw("Vertical") * 0.5f, 0f);
            }
        }
        
    }

    // check collision
    void OnCollisionEnter2D(Collision2D other){
        Debug.Log("Player has collided with another object");
    }

    
}
