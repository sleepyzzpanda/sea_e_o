using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class box_behavior : MonoBehaviour
{
    public float speed = 10.0f;
    public Transform move_point;
    public LayerMask stop_movement;
    private GameObject player;
   
    // Start is called before the first frame update
    void Start()
    {
        move_point.parent = null;
        player = GameObject.Find("Player");
        speed = 10.0f;
    }

    // Update is called once per frame
    void Update()
    {
        // // if player is near item, add item to inventory
        if(Vector3.Distance(player.transform.position, transform.position) <= 1.25f){
            if(Input.GetKeyDown(KeyCode.Z) && Vector3.Distance(move_point.position, transform.position) <= 0.05f){
                // if player is to the left move box to the right
                if(player.transform.position.x < transform.position.x){
                    box_move("right");
                } else if(player.transform.position.x > transform.position.x){
                    box_move("left");
                } 
                if(player.transform.position.y > transform.position.y){
                    box_move("down");
                } else if(player.transform.position.y < transform.position.y){
                    box_move("up");
                }
            }
            // move box to move_point
            transform.position = Vector3.MoveTowards(transform.position, move_point.position, speed * Time.deltaTime);
        }

    }

    void box_move(string direction){
        // move move_point to the direction
        if(direction == "right"){
            if(!Physics2D.OverlapCircle(move_point.position + new Vector3(0.5f, 0f, 0f), 0.1f, stop_movement))
            {
                move_point.position += new Vector3(0.5f, 0f, 0f);
            }
        } else if(direction == "left"){
            if(!Physics2D.OverlapCircle(move_point.position + new Vector3(-0.5f, 0f, 0f), 0.1f, stop_movement))
            {
                move_point.position += new Vector3(-0.5f, 0f, 0f);
            }
        } else if(direction == "down"){
            if(!Physics2D.OverlapCircle(move_point.position + new Vector3(0f, -0.5f, 0f), 0.1f, stop_movement))
            {
                move_point.position += new Vector3(0f, -0.5f, 0f);
            }
        } else if(direction == "up"){
            if(!Physics2D.OverlapCircle(move_point.position + new Vector3(0f, 1.0f, 0f), 0.1f, stop_movement))
            {
                move_point.position += new Vector3(0f, 1.0f, 0f);
            }
        }
    }
}
