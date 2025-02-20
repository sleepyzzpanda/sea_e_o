using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class scene_behavior : MonoBehaviour
{
    public GameObject dialogue_box;
    public GameObject player, rooms;

    public GameObject tutorial;
    // public GameObject dialogue_test;
    private int scene_index, player_choice;
    private bool scene_flag;

    // Start is called before the first frame update
    void Start()
    {
        scene_index = 1;
        scene_0();
    }

    // Update is called once per frame
    void Update()
    {
        // // check if player is near dialogue box
        // if(Vector3.Distance(player.transform.position, dialogue_test.transform.position) <= 0.5f && dialogue_box.GetComponent<Dialogue>().active == false){
        //     // if input key "Z" is pressed, show dialogue box
        //     if(Input.GetKeyDown(KeyCode.Z)){
        //         dialogue_box.SetActive(true);
        //         dialogue_box.GetComponent<Dialogue>().startDialogue();
        //     }
        // }
        // // if dialogue box is active, player cannot move
        // if(dialogue_box.GetComponent<Dialogue>().active == true){
        //     player.GetComponent<player_behavior>().enabled = false;
        // }
        // // if dialogue box is not active, player can move
        // if(dialogue_box.GetComponent<Dialogue>().active == false){
        //     player.GetComponent<player_behavior>().enabled = true;
        // }
        switch(scene_index){
            case 0: // no active cutscene
                // enable player movement
                player.GetComponent<player_behavior>().enabled = true;
                break;
            case 1: // intro scene
                if(dialogue_box.GetComponent<Dialogue>().active == true){
                    player.GetComponent<player_behavior>().enabled = false;
                } else{
                    player_choice = dialogue_box.GetComponent<Dialogue>().player_choice;
                    if(player_choice == 0){
                        scene_index = 2;
                        scene_flag = true;
                    } else if(player_choice == 1){
                        scene_index = 3;
                        scene_flag = true;
                    }
                }
                break;
            case 2: // picked yes
                if(dialogue_box.GetComponent<Dialogue>().active == true){
                    player.GetComponent<player_behavior>().enabled = false;
                } else{
                    if(scene_flag == true){
                        scene_2();
                        scene_flag = false;
                    } else {
                        player.GetComponent<player_behavior>().enabled = true;
                        scene_index = 4; // goes to no active cutscene
                    }
                }
                break;
            case 3: // picked no
                if(dialogue_box.GetComponent<Dialogue>().active == true){
                    player.GetComponent<player_behavior>().enabled = false;
                } else{
                    if(scene_flag == true){
                        scene_3();
                        scene_flag = false;
                    } else {
                        scene_flag = true;
                        scene_index = 2; // goes to intro in case 2
                    }
                }
                break;
            case 4: // no active cutscene - tutorial
                // check if player is near tutorial
                if(Vector3.Distance(player.transform.position, tutorial.transform.position) <= 0.5f && dialogue_box.GetComponent<Dialogue>().active == false){
                    // if input key "Z" is pressed, show dialogue box
                    if(Input.GetKeyDown(KeyCode.Z)){
                        dialogue_box.SetActive(true);
                        // start tutorial
                        string[] lines = tutorial.GetComponent<dialogue_item>().sentences;
                        string[] speakers = tutorial.GetComponent<dialogue_item>().speakers;
                        string choices = tutorial.GetComponent<dialogue_item>().choices;
                        dialogue_box.GetComponent<Dialogue>().choices = choices;
                        dialogue_box.GetComponent<Dialogue>().speakers = speakers;
                        dialogue_box.GetComponent<Dialogue>().sentences = lines;
                        dialogue_box.SetActive(true);
                        dialogue_box.GetComponent<Dialogue>().startDialogue();
                        // set scene index to 5
                        scene_flag = true;
                        scene_index = 5;
                    }
                }
                break;
            case 5: // after tutorial
                if(dialogue_box.GetComponent<Dialogue>().active == true){
                        player.GetComponent<player_behavior>().enabled = false;
                } else{
                    if(scene_flag == true){
                        scene_5();
                        scene_flag = false;
                    } else {
                        player.GetComponent<player_behavior>().enabled = true;
                        scene_index = 6; // goes to 1st objective
                    }
                }
                break;
            case 6: // 1st objective
                break;
        }
        
    }

    void scene_0(){
        // set player sprite and rooms to inactive
        rooms.SetActive(false);
        string[] lines = new string[1] {"Are you awake yet?"};
        string[] speakers = new string[1] {"????"};
        string choices = "(Z) Yes, (X) No";
        dialogue_box.GetComponent<Dialogue>().choices = choices;
        dialogue_box.GetComponent<Dialogue>().speakers = speakers;
        dialogue_box.GetComponent<Dialogue>().sentences = lines;
        dialogue_box.SetActive(true);
        dialogue_box.GetComponent<Dialogue>().startDialogue();
    }

    void scene_2(){
        // set player sprite and rooms to active
        rooms.SetActive(true);
        string[] lines = new string[6] {"Who is this? How did I get here?", "Welcome to SEOs underwater station!", 
        "Apologies for the confusion, it’s standard procedure to keep our location a secret. Please state your ID number", 
        "Employee 15. Who are you?", "I am INTRA, your integrated robotic assistant installed in this station.", 
        "Please check the document on the desk for further instructions. Press z to interact."};
        string[] speakers = new string[6] {"Employee 15", "????", "????", "Employee 15", "INTRA", "INTRA"};
        string choices = "(Z) - Continue";
        dialogue_box.GetComponent<Dialogue>().choices = choices;
        dialogue_box.GetComponent<Dialogue>().speakers = speakers;
        dialogue_box.GetComponent<Dialogue>().sentences = lines;
        dialogue_box.SetActive(true);
        dialogue_box.GetComponent<Dialogue>().startDialogue();
    }

    void scene_3(){
        rooms.SetActive(false);
        string[] lines = new string[1] {"You need to wake up"};
        string[] speakers = new string[1] {"????"};
        string choices = "(Z) Continue";
        dialogue_box.GetComponent<Dialogue>().choices = choices;
        dialogue_box.GetComponent<Dialogue>().speakers = speakers;
        dialogue_box.GetComponent<Dialogue>().sentences = lines;
        dialogue_box.SetActive(true);
        dialogue_box.GetComponent<Dialogue>().startDialogue();
    }

    void scene_5(){
        string[] lines = new string[9] {"What have you been told so far?", 
        "I was told the research here was confidential, and that I would learn the rest of the details when I arrived.", 
        "Of course, what would you like to know?", "What is my mission?", 
        "This area of the ocean is owned by SE’O. You are to ensure all systems are maintained to support ongoing operations by keeping the area secure.", 
        "There are a few rooms you can explore", "1. The Main Lab (Check available equipement)", 
        "2. The Containment Room (see creature you are studying)", "Thank you Intra. This all sounds exciting!"};
        string[] speakers = new string[9] {"INTRA", "Employee 15", "INTRA", "Employee 15", "INTRA", "INTRA", "INTRA", "INTRA", "Employee 15"};
        string choices = "(Z) Continue";
        dialogue_box.GetComponent<Dialogue>().choices = choices;
        dialogue_box.GetComponent<Dialogue>().speakers = speakers;
        dialogue_box.GetComponent<Dialogue>().sentences = lines;
        dialogue_box.SetActive(true);
        dialogue_box.GetComponent<Dialogue>().startDialogue();
    }
}
