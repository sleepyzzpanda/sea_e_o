using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class monster_ocean : MonoBehaviour
{
    public float speed = 4.0f;
    public Transform move_point; // this will be the player's position
    public LayerMask stop_movement;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        transform.position = Vector3.MoveTowards(transform.position, move_point.position, speed * Time.deltaTime);
        // make it so that the stop_movement layermask is the only thing that can stop the monster
        
    }
}
