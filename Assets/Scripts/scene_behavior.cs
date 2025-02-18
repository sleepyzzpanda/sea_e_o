using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class scene_behavior : MonoBehaviour
{
    public GameObject dialogue_box;
    public GameObject player;
    public GameObject dialogue_test;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        // check if player is near dialogue box
        if(Vector3.Distance(player.transform.position, dialogue_test.transform.position) <= 0.5f && dialogue_box.GetComponent<Dialogue>().active == false){
            // if input key "Z" is pressed, show dialogue box
            if(Input.GetKeyDown(KeyCode.Z)){
                dialogue_box.SetActive(true);
                dialogue_box.GetComponent<Dialogue>().startDialogue();
            }
        }
        // if dialogue box is active, player cannot move
        if(dialogue_box.GetComponent<Dialogue>().active == true){
            player.GetComponent<player_behavior>().enabled = false;
        }
        // if dialogue box is not active, player can move
        if(dialogue_box.GetComponent<Dialogue>().active == false){
            player.GetComponent<player_behavior>().enabled = true;
        }
        
    }
}
