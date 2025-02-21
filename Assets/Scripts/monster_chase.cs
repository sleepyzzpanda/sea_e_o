using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class monster_chase : MonoBehaviour
{
    public float speed = 5.0f;
    public Sprite sprite_normal_front, sprite_normal_back, sprite_normal_left, sprite_normal_right;
    private int direction;
    public bool catch_player;
    // Start is called before the first frame update
    void Start()
    {
        direction = 0;
        catch_player = false;
        
    }

    // Update is called once per frame
    void Update()
    {
        // starting coords [x, y] = [0, 12.07]
        if(direction == 0){
            // move until y = -1.3 then change direction
            if(transform.position.y > -1.3f){
                transform.position += new Vector3(0f, -0.5f * speed * Time.deltaTime, 0f);
            } else {
                direction = 1;
            }
            // sprite change
            GetComponent<SpriteRenderer>().sprite = sprite_normal_front;
        } 
        if(direction == 1){
            // move until x = -16.15 then change direction
            if(transform.position.x > -16.15f){
                transform.position += new Vector3(-0.5f * speed * Time.deltaTime, 0f, 0f);
            } else {
                direction = 2;
            }
            // sprite change
            GetComponent<SpriteRenderer>().sprite = sprite_normal_left;
        }
        if(direction == 2){
            // move until y = -0.45 then change direction
            if(transform.position.y < -0.45f){
                transform.position += new Vector3(0f, 0.5f * speed * Time.deltaTime, 0f);
            } else {
                direction = 3;
            }
            // sprite change
            GetComponent<SpriteRenderer>().sprite = sprite_normal_back;
        }

        // if player is near monster, catch player
        if(Vector3.Distance(GameObject.Find("Player").transform.position, transform.position) <= 0.05f){
            catch_player = true;
        }
            
    }
}
