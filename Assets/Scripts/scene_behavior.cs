using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class scene_behavior : MonoBehaviour
{
    public GameObject dialogue_box;
    public GameObject player, rooms;
    public GameObject chasing_monster, chasing_monster_ocean;

    // for containment room
    public GameObject monster, computer;

    // for main lab
    public GameObject lab_computer, chem_mixer, chem_identifier, broken_usb;

    // for equipment room
    public GameObject con_suit, flashlight, harpoon, moonpool_flag, circuit_panel, AIroomDoor;

    public GameObject tutorial, monster_sample, monster_sample_2;
    private int scene_index, player_choice;
    private bool scene_flag, sample_collected;
    private bool c_room_done, main_lab_done, equipment_room_done;

    // for c_room_done
    private bool s8_done, s11_done;

    // for main_lab_done
    bool m1_done, m2_done, m3_done, m4_done;

    // for equipment_room_done
    bool con_suit_done, flashlight_done, harpoon_done;

    public GameObject[] upperHall_boxes;
    public GameObject[] AIroom_boxes;
    public GameObject light_object, light2, startScreen, deadScreen, objective_screen;
    public TextMeshProUGUI objective_text;
    public GameObject boxTutTrigger;
    private bool box_tut_done;

    // Start is called before the first frame update
    void Start()
    {
        // set all bools to false
        scene_flag = false;
        c_room_done = false;
        main_lab_done = false;
        s8_done = false;
        s11_done = false;
        m1_done = false;
        m2_done = false;
        m3_done = false;
        m4_done = false;
        con_suit_done = false;
        flashlight_done = false;
        harpoon_done = false;
        sample_collected = false;
        box_tut_done = false;

        // ai room door inactive
        AIroomDoor.SetActive(false);
        // set monster sample to inactive
        monster_sample.SetActive(false);
        monster_sample_2.SetActive(false);
        // monster chase inactive
        chasing_monster.SetActive(false);
        // light object inactive
        light_object.SetActive(false);
        light2.SetActive(false);
        // obective screen inactive
        objective_screen.SetActive(false);
        // set scene index to -1
        scene_index = -1;
        scene_flag = true;
    }

    // Update is called once per frame
    void Update()
    {
        if(scene_index == -1){
            startScreen.SetActive(true);
            if(Input.GetKeyDown(KeyCode.X)){
                startScreen.SetActive(false);
                scene_index = 0;
            }
        }
        switch(scene_index){
            case 0: // start scene
                if(scene_flag == true){
                    scene_0();
                    scene_flag = false;
                } else {
                    player_choice = dialogue_box.GetComponent<Dialogue>().player_choice;
                    scene_index = 1;
                }
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
                // obj active
                objective_screen.SetActive(true);
                objective_text.text = "-> Go to the tutorial";
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
                // obj active
                objective_screen.SetActive(true);
                objective_text.text = "-> Explore containment room \n-> Explore main lab";
                // check if player is close to box tut trigger
                if(Vector3.Distance(player.transform.position, boxTutTrigger.transform.position) <= 1.5f && dialogue_box.GetComponent<Dialogue>().active == false && box_tut_done == false){
                    string[] lines = new string[1] {"It seems some boxes are blocking off this area. Press Z to move them."};
                    string[] speakers = new string[1] {"INTRA"};
                    string choices = "(Z) - Continue";
                    dialogue_box.GetComponent<Dialogue>().choices = choices;
                    dialogue_box.GetComponent<Dialogue>().speakers = speakers;
                    dialogue_box.GetComponent<Dialogue>().sentences = lines;
                    dialogue_box.SetActive(true);
                    dialogue_box.GetComponent<Dialogue>().startDialogue();
                    box_tut_done = true;
                }
                // if player tries to go upper hall, show dialogue
                if(Physics2D.OverlapCircle(player.transform.position, 0.5f, LayerMask.GetMask("UpperHallFlag")) && dialogue_box.GetComponent<Dialogue>().active == false){
                    // INTRA dialogue
                    string[] lines = new string[1] {"It seems some boxes are blocking off this area. Lets come back later"};
                    string[] speakers = new string[1] {"Employee 15"};
                    string choices = "(Z) - Continue";
                    dialogue_box.GetComponent<Dialogue>().choices = choices;
                    dialogue_box.GetComponent<Dialogue>().speakers = speakers;
                    dialogue_box.GetComponent<Dialogue>().sentences = lines;
                    dialogue_box.SetActive(true);
                    dialogue_box.GetComponent<Dialogue>().startDialogue();
                }
                // if player enters containment room do the monster roar and go to scene 7
                if(Physics2D.OverlapCircle(player.transform.position, 0.5f, LayerMask.GetMask("ContainmentRoomFlag"))){
                    // play monster roar
                    // play some dialoge
                    string [] lines = new string[2] {"[You hear a loud roar from the containment room]", "[puzzle for getting into room here]"};
                    string [] speakers = new string[2] {"", ""};
                    string choices = "(Z) - Continue";
                    dialogue_box.GetComponent<Dialogue>().choices = choices;
                    dialogue_box.GetComponent<Dialogue>().speakers = speakers;
                    dialogue_box.GetComponent<Dialogue>().sentences = lines;
                    dialogue_box.SetActive(true);
                    dialogue_box.GetComponent<Dialogue>().startDialogue();
                    scene_index = 7;
                } else if(Physics2D.OverlapCircle(player.transform.position, 0.5f, LayerMask.GetMask("MainLabFlag"))){
                    // if player enters main lab, go to scene 13
                    scene_index = 13;
                }               
                break;
            case 7: // containment room
                // monitoring screen = scene 8
                if(Vector3.Distance(player.transform.position, computer.transform.position) <= 0.5f && dialogue_box.GetComponent<Dialogue>().active == false){
                    // if input key "Z" is pressed, show dialogue box
                    if(Input.GetKeyDown(KeyCode.Z)){
                        dialogue_box.SetActive(true);
                        // start tutorial
                        string[] lines = computer.GetComponent<dialogue_item>().sentences;
                        string[] speakers = computer.GetComponent<dialogue_item>().speakers;
                        string choices = computer.GetComponent<dialogue_item>().choices;
                        dialogue_box.GetComponent<Dialogue>().choices = choices;
                        dialogue_box.GetComponent<Dialogue>().speakers = speakers;
                        dialogue_box.GetComponent<Dialogue>().sentences = lines;
                        dialogue_box.SetActive(true);
                        dialogue_box.GetComponent<Dialogue>().startDialogue();
                        // set scene index to 5
                        scene_flag = true;
                        scene_index = 8;
                    }
                }
                // monster interaction = scene X
                if(Vector3.Distance(player.transform.position, monster.transform.position) <= 0.5f && dialogue_box.GetComponent<Dialogue>().active == false){
                    // if input key "Z" is pressed, show dialogue box
                    if(Input.GetKeyDown(KeyCode.Z)){
                        dialogue_box.SetActive(true);
                        // start tutorial
                        string[] lines = monster.GetComponent<dialogue_item>().sentences;
                        string[] speakers = monster.GetComponent<dialogue_item>().speakers;
                        string choices = monster.GetComponent<dialogue_item>().choices;
                        dialogue_box.GetComponent<Dialogue>().choices = choices;
                        dialogue_box.GetComponent<Dialogue>().speakers = speakers;
                        dialogue_box.GetComponent<Dialogue>().sentences = lines;
                        dialogue_box.SetActive(true);
                        dialogue_box.GetComponent<Dialogue>().startDialogue();
                        // set scene index to 5
                        scene_flag = true;
                        scene_index = 11;
                    }
                }
                break;
            case 8: // monitoring screen dialoge
                if(dialogue_box.GetComponent<Dialogue>().active == true){
                        player.GetComponent<player_behavior>().enabled = false;
                } else{
                    if(scene_flag == true){
                        scene_8();
                        scene_flag = false;
                    } else {
                        player_choice = dialogue_box.GetComponent<Dialogue>().player_choice;
                        if(player_choice == 0){
                            // yes, ask intra
                            scene_index = 9;
                            scene_flag = true;
                        } else if(player_choice == 1){
                            // no, cancel
                            scene_index = 10;
                            scene_flag = false;
                        }
                    }
                }
                // set a s8_done flag
                s8_done = true;
                break;
            case 9:
                // talk to INTRA
                if(dialogue_box.GetComponent<Dialogue>().active == true){
                        player.GetComponent<player_behavior>().enabled = false;
                } else{
                    if(scene_flag == true){
                        scene_9();
                        scene_flag = false;
                    } else {
                        // player can move
                        player.GetComponent<player_behavior>().enabled = true;
                        scene_index = 10;
                    }
                }
                break;
            case 10: 
                // no active cutscene
                player.GetComponent<player_behavior>().enabled = true;
                // check if containment room is done
                if(s8_done == true && s11_done == true){
                    c_room_done = true;
                } else {
                    // back to scene 7:
                    scene_index = 7;
                    return;
                }
                if(main_lab_done == false && c_room_done == true){
                    // obj = go to main lab
                    objective_screen.SetActive(true);
                    objective_text.text = "-> Explore Main Lab";
                    // go to main lab
                    scene_index = 13;
                }
                if(main_lab_done == true && c_room_done == false){
                    // obj = go to containment room
                    objective_screen.SetActive(true);
                    objective_text.text = "-> Explore Containment Room";
                    // go to containment room
                    scene_index = 7;
                }
                if(main_lab_done == true && c_room_done == true){
                    // obj = exit room
                    objective_screen.SetActive(true);
                    objective_text.text = "-> Exit the room";
                    // if touching the containemnt room flag or main hall flag
                    if(Physics2D.OverlapCircle(player.transform.position, 0.5f, LayerMask.GetMask("ContainmentRoomFlag"))){
                        // cut scene 14
                        scene_flag = true;
                        scene_index = 14;
                    } else if(Physics2D.OverlapCircle(player.transform.position, 0.5f, LayerMask.GetMask("MainLabFlag"))){
                        // cut scene 14
                        scene_flag = true;
                        scene_index = 14;
                    }
                }
                break;

            case 11:
                // monster interaction
                if(dialogue_box.GetComponent<Dialogue>().active == true){
                        player.GetComponent<player_behavior>().enabled = false;
                } else{
                    if(scene_flag == true){
                        player_choice = dialogue_box.GetComponent<Dialogue>().player_choice;
                        if(player_choice == 0){
                            // yes, communicate
                            scene_index = 12;
                            scene_flag = true;
                        } else if(player_choice == 1){
                            // no, cancel
                            scene_index = 10;
                            scene_flag = false;
                        }
                    } else {
                        // player can move
                        player.GetComponent<player_behavior>().enabled = true;
                        scene_index = 10;
                    }
                }
                // set a s11_done flag
                s11_done = true;
                break;
            case 12:
                // communicate with monster
                if(dialogue_box.GetComponent<Dialogue>().active == true){
                        player.GetComponent<player_behavior>().enabled = false;
                } else{
                    if(scene_flag == true){
                        scene_12();
                        scene_flag = false;
                    } else {
                        // player can move
                        player.GetComponent<player_behavior>().enabled = true;
                        scene_index = 10;
                    }
                }
                break;
            case 13: // main lab scene
                // check if player is near chem_identifier
                if(Vector3.Distance(player.transform.position, chem_identifier.transform.position) <= 0.5f && dialogue_box.GetComponent<Dialogue>().active == false){
                    // if input key "Z" is pressed, show dialogue box
                    if(Input.GetKeyDown(KeyCode.Z)){
                        dialogue_box.SetActive(true);
                        // start tutorial
                        string[] lines = chem_identifier.GetComponent<dialogue_item>().sentences;
                        string[] speakers = chem_identifier.GetComponent<dialogue_item>().speakers;
                        string choices = chem_identifier.GetComponent<dialogue_item>().choices;
                        dialogue_box.GetComponent<Dialogue>().choices = choices;
                        dialogue_box.GetComponent<Dialogue>().speakers = speakers;
                        dialogue_box.GetComponent<Dialogue>().sentences = lines;
                        dialogue_box.SetActive(true);
                        dialogue_box.GetComponent<Dialogue>().startDialogue();
                        m1_done = true;
                    }
                }
                // check if player is near chem_mixer
                if(Vector3.Distance(player.transform.position, chem_mixer.transform.position) <= 0.5f && dialogue_box.GetComponent<Dialogue>().active == false){
                    // if input key "Z" is pressed, show dialogue box
                    if(Input.GetKeyDown(KeyCode.Z)){
                        dialogue_box.SetActive(true);
                        // start tutorial
                        string[] lines = chem_mixer.GetComponent<dialogue_item>().sentences;
                        string[] speakers = chem_mixer.GetComponent<dialogue_item>().speakers;
                        string choices = chem_mixer.GetComponent<dialogue_item>().choices;
                        dialogue_box.GetComponent<Dialogue>().choices = choices;
                        dialogue_box.GetComponent<Dialogue>().speakers = speakers;
                        dialogue_box.GetComponent<Dialogue>().sentences = lines;
                        dialogue_box.SetActive(true);
                        dialogue_box.GetComponent<Dialogue>().startDialogue();
                        m2_done = true;
                    }
                }
                // check if player is near lab_computer
                if(Vector3.Distance(player.transform.position, lab_computer.transform.position) <= 0.5f && dialogue_box.GetComponent<Dialogue>().active == false){
                    // if input key "Z" is pressed, show dialogue box
                    if(Input.GetKeyDown(KeyCode.Z)){
                        dialogue_box.SetActive(true);
                        // start tutorial
                        string[] lines = lab_computer.GetComponent<dialogue_item>().sentences;
                        string[] speakers = lab_computer.GetComponent<dialogue_item>().speakers;
                        string choices = lab_computer.GetComponent<dialogue_item>().choices;
                        dialogue_box.GetComponent<Dialogue>().choices = choices;
                        dialogue_box.GetComponent<Dialogue>().speakers = speakers;
                        dialogue_box.GetComponent<Dialogue>().sentences = lines;
                        dialogue_box.SetActive(true);
                        dialogue_box.GetComponent<Dialogue>().startDialogue();
                        m3_done = true;
                    }
                }
                // check if player is near broken_usb
                if(Vector3.Distance(player.transform.position, broken_usb.transform.position) <= 0.5f && dialogue_box.GetComponent<Dialogue>().active == false){
                    // if input key "Z" is pressed, show dialogue box
                    if(Input.GetKeyDown(KeyCode.Z)){
                        dialogue_box.SetActive(true);
                        // start tutorial
                        string[] lines = broken_usb.GetComponent<dialogue_item>().sentences;
                        string[] speakers = broken_usb.GetComponent<dialogue_item>().speakers;
                        string choices = broken_usb.GetComponent<dialogue_item>().choices;
                        dialogue_box.GetComponent<Dialogue>().choices = choices;
                        dialogue_box.GetComponent<Dialogue>().speakers = speakers;
                        dialogue_box.GetComponent<Dialogue>().sentences = lines;
                        dialogue_box.SetActive(true);
                        dialogue_box.GetComponent<Dialogue>().startDialogue();
                        m4_done = true;
                    }
                }
                if(m1_done == true && m2_done == true && m3_done == true && m4_done == true){
                    if(c_room_done != true){
                        // obj = go to containment room
                        objective_screen.SetActive(true);
                        objective_text.text = "-> Explore Containment Room";
                    } 
                    main_lab_done = true;
                    // set usb item to inactive
                    broken_usb.SetActive(false);
                    // go to scene 10;
                    scene_index = 10;
                }
                break;
            case 14: // cutscene
                // deactivate the upper hall boxes
                foreach(GameObject box in upperHall_boxes){
                    box.SetActive(false);
                }
                // deactivate the AI room boxes
                foreach(GameObject box in AIroom_boxes){
                    box.SetActive(false);
                }
                if(dialogue_box.GetComponent<Dialogue>().active == true){
                        player.GetComponent<player_behavior>().enabled = false;
                } else{
                    if(scene_flag == true){
                        scene_14();
                        scene_flag = false;
                    } else {
                        // player can move
                        player.GetComponent<player_behavior>().enabled = true;
                        scene_index = 15;
                    }
                }
                break;
            case 15: // start rising action
                // obj = go to equiment room
                objective_screen.SetActive(true);
                objective_text.text = "-> Go to equipment room";
                // set light object to active
                light_object.SetActive(true);
                // if player enters upper hall
                if(Physics2D.OverlapCircle(player.transform.position, 0.5f, LayerMask.GetMask("UpperHallFlag")) && dialogue_box.GetComponent<Dialogue>().active == false){
                    // INTRA dialogue
                    string[] lines = new string[2] {"Welcome to Upper Halls.", 
                    "Below are the security doors. They will close if the station detects a threat."};
                    string[] speakers = new string[2] {"INTRA", "INTRA"};
                    string choices = "(Z) - Continue";
                    dialogue_box.GetComponent<Dialogue>().choices = choices;
                    dialogue_box.GetComponent<Dialogue>().speakers = speakers;
                    dialogue_box.GetComponent<Dialogue>().sentences = lines;
                    dialogue_box.SetActive(true);
                    dialogue_box.GetComponent<Dialogue>().startDialogue();
                    // go to scene 16
                    scene_index = 16;
                }
                break;
            case 16: // upper hall exploration
                // if AI room
                if(Physics2D.OverlapCircle(player.transform.position, 0.5f, LayerMask.GetMask("AIRoomFlag")) && dialogue_box.GetComponent<Dialogue>().active == false){
                    // INTRA dialogue
                    string[] lines = new string[1] {"This is the AI room. Equipment room is the other way"};
                    string[] speakers = new string[1] {"INTRA"};
                    string choices = "(Z) - Continue";
                    dialogue_box.GetComponent<Dialogue>().choices = choices;
                    dialogue_box.GetComponent<Dialogue>().speakers = speakers;
                    dialogue_box.GetComponent<Dialogue>().sentences = lines;
                    dialogue_box.SetActive(true);
                    dialogue_box.GetComponent<Dialogue>().startDialogue();
                }
                // if Locked Room
                if(Physics2D.OverlapCircle(player.transform.position, 0.5f, LayerMask.GetMask("LockedRoomFlag")) && dialogue_box.GetComponent<Dialogue>().active == false){
                    // INTRA dialogue
                    string[] lines = new string[1] {"Apologies Employee 15, your clearance is not high enough to enter this room."};
                    string[] speakers = new string[1] {"INTRA"};
                    string choices = "(Z) - Continue";
                    dialogue_box.GetComponent<Dialogue>().choices = choices;
                    dialogue_box.GetComponent<Dialogue>().speakers = speakers;
                    dialogue_box.GetComponent<Dialogue>().sentences = lines;
                    dialogue_box.SetActive(true);
                    dialogue_box.GetComponent<Dialogue>().startDialogue();
                }
                // if equipment room
                if(Physics2D.OverlapCircle(player.transform.position, 0.5f, LayerMask.GetMask("EquipmentRoomFlag")) && dialogue_box.GetComponent<Dialogue>().active == false){
                    // INTRA dialogue
                    // obj = get equipment
                    objective_screen.SetActive(true);
                    objective_text.text = "-> Get equipment";
                    string[] lines = new string[1] {"Before you proceed, you must equip a containment suit."};
                    string[] speakers = new string[1] {"INTRA"};
                    string choices = "(Z) - Continue";
                    dialogue_box.GetComponent<Dialogue>().choices = choices;
                    dialogue_box.GetComponent<Dialogue>().speakers = speakers;
                    dialogue_box.GetComponent<Dialogue>().sentences = lines;
                    dialogue_box.SetActive(true);
                    dialogue_box.GetComponent<Dialogue>().startDialogue();
                    // go to scene 17
                    scene_index = 17;
                }
                break;
            case 17: // equipment room
                // need to collect suit, harpoon and flashlight
                // set moonpool flag item to inactive
                moonpool_flag.SetActive(false);
                // if player is near containment suit
                if(Vector3.Distance(player.transform.position, con_suit.transform.position) <= 0.5f && dialogue_box.GetComponent<Dialogue>().active == false){
                    // if input key "Z" is pressed collect suit
                    if(Input.GetKeyDown(KeyCode.Z)){
                        con_suit_done = true;
                        con_suit.SetActive(false);
                    }
                }
                // if player is near harpoon
                if(Vector3.Distance(player.transform.position, harpoon.transform.position) <= 0.5f && dialogue_box.GetComponent<Dialogue>().active == false){
                    // if input key "Z" is pressed collect harpoon
                    if(Input.GetKeyDown(KeyCode.Z)){
                        harpoon_done = true;
                        harpoon.SetActive(false);
                    }
                }

                // if player is near flashlight
                if(Vector3.Distance(player.transform.position, flashlight.transform.position) <= 0.5f && dialogue_box.GetComponent<Dialogue>().active == false){
                    // if input key "Z" is pressed collect flashlight
                    if(Input.GetKeyDown(KeyCode.Z)){
                        flashlight_done = true;
                        flashlight.SetActive(false);
                    }
                }

                // if all items are collected
                if(con_suit_done == true && harpoon_done == true && flashlight_done == true){
                    // INTRA dialogue
                    string[] lines = new string[1] {"Opening Moon Pool doors. The circuit panels are on the right."};
                    string[] speakers = new string[1] {"INTRA"};
                    string choices = "(Z) - Continue";
                    dialogue_box.GetComponent<Dialogue>().choices = choices;
                    dialogue_box.GetComponent<Dialogue>().speakers = speakers;
                    dialogue_box.GetComponent<Dialogue>().sentences = lines;
                    dialogue_box.SetActive(true);
                    dialogue_box.GetComponent<Dialogue>().startDialogue();
                    // set moonpool item to active
                    moonpool_flag.SetActive(true);
                    // go to scene 18
                    scene_index = 18;
                }
                break;
            case 18: // moon pool
                // obj = go to moon pool
                objective_screen.SetActive(true);
                objective_text.text = "-> Interact with Moon Pool";
                // if player is on moonpool flag
                if(Physics2D.OverlapCircle(player.transform.position, 0.5f, LayerMask.GetMask("MoonPoolFlag")) && dialogue_box.GetComponent<Dialogue>().active == false){
                    if(Input.GetKeyDown(KeyCode.Z)){
                        // teleport this bish
                        player.transform.position = new Vector3(-6.8f, 33.798f, -1f);
                        // move player movepoint
                        player.GetComponent<player_behavior>().move_point.position = new Vector3(-6.8f, 33.798f, -1f);
                        scene_index = 19;
                    }
                }

                break;
            case 19: // moon pool
                // player is_diving is true
                player.GetComponent<player_behavior>().is_diving = true;
                // obj = go to circuit panel
                objective_screen.SetActive(true);
                objective_text.text = "-> Interact with Circuit Panel";
                // if player near circuit panel trigger dialoge
                if(Vector3.Distance(player.transform.position, circuit_panel.transform.position) <= 0.5f && dialogue_box.GetComponent<Dialogue>().active == false){
                    // if input key "Z" is pressed collect suit
                    if(Input.GetKeyDown(KeyCode.Z)){
                        // circuit dialog
                        string[] lines = circuit_panel.GetComponent<dialogue_item>().sentences;
                        string[] speakers = circuit_panel.GetComponent<dialogue_item>().speakers;
                        string choices = circuit_panel.GetComponent<dialogue_item>().choices;
                        dialogue_box.GetComponent<Dialogue>().choices = choices;
                        dialogue_box.GetComponent<Dialogue>().speakers = speakers;
                        dialogue_box.GetComponent<Dialogue>().sentences = lines;
                        dialogue_box.SetActive(true);
                        dialogue_box.GetComponent<Dialogue>().startDialogue();
                        // go to scene 20 - cutscene
                        scene_flag = true;
                        scene_index = 20;
                        // turn light object off
                        light_object.SetActive(false);
                    }
                    // check for overlap with monster
                    if(Physics2D.OverlapCircle(player.transform.position, 0.5f, LayerMask.GetMask("Monster")) && dialogue_box.GetComponent<Dialogue>().active == false){
                        scene_flag = true;
                        // go to scene 27
                        scene_index = 27;
                    }
                }
                break;
            case 20: // cutscene
                // activate AI room boxes
                foreach(GameObject box in AIroom_boxes){
                    box.SetActive(true);
                }
                if(dialogue_box.GetComponent<Dialogue>().active == true){
                        player.GetComponent<player_behavior>().enabled = false;
                } else{
                    if(scene_flag == true){
                        scene_20();
                        scene_flag = false;
                    } else {
                        // activate ocean monster
                        chasing_monster_ocean.SetActive(true);
                        // obj = escape to door
                        objective_screen.SetActive(true);
                        objective_text.text = "-> Escape to the door";
                        // player can move
                        player.GetComponent<player_behavior>().enabled = true;
                        // if player is on OceanExit and presses Z
                        if(Physics2D.OverlapCircle(player.transform.position, 0.5f, LayerMask.GetMask("OceanExit")) && dialogue_box.GetComponent<Dialogue>().active == false){
                            // if input key "Z" is pressed collect suit
                            if(Input.GetKeyDown(KeyCode.Z)){
                                // teleport player to upper hall
                                player.transform.position = new Vector3(-0.02f, 8.2f, -1f);
                                // move player movepoint
                                player.GetComponent<player_behavior>().move_point.position = new Vector3(-0.02f, 8.2f, -1f);
                                scene_flag = true;
                                // player is_diving is false
                                player.GetComponent<player_behavior>().is_diving = false;
                                // set monster to inactive
                                chasing_monster_ocean.SetActive(false);
                                // go to scene 21
                                scene_index = 21;
                            }
                        }
                    }
                } 
                break;
            case 21: // rising action
                // move some boxes here
                // monster starts chasing
                chasing_monster.SetActive(true);
                // obj = get to AI room
                objective_screen.SetActive(true);
                objective_text.text = "-> Get to AI Room (Upper Hall - Left)";
                // player needs to get to AI room
                if(Physics2D.OverlapCircle(player.transform.position, 0.5f, LayerMask.GetMask("AIRoomFlag")) && dialogue_box.GetComponent<Dialogue>().active == false){
                    // ai room door active
                    AIroomDoor.SetActive(true);
                    if(scene_flag == true){
                        scene_21();
                        scene_flag = false;
                    } else {
                        player_choice = dialogue_box.GetComponent<Dialogue>().player_choice;
                        if(player_choice == 0 && dialogue_box.GetComponent<Dialogue>().active == false){
                            // yes, kill it
                            // go to scene 22
                            scene_flag = true;
                            scene_index = 22;
                        } else if(player_choice == 1 && dialogue_box.GetComponent<Dialogue>().active == false){
                            // no, blind it
                            // go to scene 25
                            scene_flag = true;
                            scene_index = 25;
                        }
                    }   
                }
                if(Physics2D.OverlapCircle(player.transform.position, 0.5f, LayerMask.GetMask("Monster")) && dialogue_box.GetComponent<Dialogue>().active == false){
                    scene_flag = true;
                    // go to scene 27
                    scene_index = 27;
                }
                break;

            case 22: // kill monster
                if(scene_flag == true){
                    // player can't move
                    player.GetComponent<player_behavior>().enabled = false;
                    // kill monster
                    scene_22();
                    // go to scene 23
                    scene_flag = false;
                } else {
                    // obj = collect sample
                    objective_screen.SetActive(true);
                    objective_text.text = "-> Collect monster sample";
                    // AI room door inactive
                    AIroomDoor.SetActive(false);
                    // player can move
                    player.GetComponent<player_behavior>().enabled = true;
                    // set monster sample to active
                    monster_sample.SetActive(true);
                    chasing_monster.SetActive(false);
                    // go to scene 23
                    scene_index = 23;
                }
                break;
            case 23: // collect the sample
                // if player is near monster sample
                if(Vector3.Distance(player.transform.position, monster_sample.transform.position) <= 0.5f && dialogue_box.GetComponent<Dialogue>().active == false){
                    // if input key "Z" is pressed collect sample
                    if(Input.GetKeyDown(KeyCode.Z)){
                        // set monster sample to inactive
                        monster_sample.SetActive(false);
                        sample_collected = true;
                        // go to scene 24
                        scene_index = 24;
                    }
                }
                break;

            case 24: // go to main lab
                // obj = go to main lab
                objective_screen.SetActive(true);
                objective_text.text = "-> Go to Chemical Mixer [Main Lab]";
                // player needs to interact with chem mixer
                // modify the chem mixer dialogue
                chem_mixer.GetComponent<dialogue_item>().sentences = new string[3] {"[You mix the sample with some chemicals]", 
                "[You have successfully created the chemical]", 
                "Excellent work Employee 15. With this we can finally contain the monsters."};
                chem_mixer.GetComponent<dialogue_item>().speakers = new string[3] {"Chem Mixer", "Chem Mixer", "INTRA"};
                chem_mixer.GetComponent<dialogue_item>().choices = "(Z) - Continue";
                // if player is near chem_mixer
                if(Vector3.Distance(player.transform.position, chem_mixer.transform.position) <= 0.5f && dialogue_box.GetComponent<Dialogue>().active == false){
                    // if input key "Z" is pressed collect suit
                    if(Input.GetKeyDown(KeyCode.Z) && sample_collected == true){
                        // obj = to be continued
                        objective_screen.SetActive(true);
                        objective_text.text = "-> To be continued...";
                        // trigger chem mixer dialogue
                        string[] lines = chem_mixer.GetComponent<dialogue_item>().sentences;
                        string[] speakers = chem_mixer.GetComponent<dialogue_item>().speakers;
                        string choices = chem_mixer.GetComponent<dialogue_item>().choices;
                        dialogue_box.GetComponent<Dialogue>().choices = choices;
                        dialogue_box.GetComponent<Dialogue>().speakers = speakers;
                        dialogue_box.GetComponent<Dialogue>().sentences = lines;
                        dialogue_box.SetActive(true);
                        dialogue_box.GetComponent<Dialogue>().startDialogue();
                        scene_index = 28;
                    }
                }
                break;
            case 25: // blind monster
                if(scene_flag == true){
                    // player can't move
                    player.GetComponent<player_behavior>().enabled = false;
                    scene_25();
                    scene_flag = false;
                } else {
                    // AI room door inactive
                    AIroomDoor.SetActive(false);
                    // player can move
                    player.GetComponent<player_behavior>().enabled = true;
                    chasing_monster.SetActive(false);
                    //go to scene 26
                    scene_index = 26;
                }
                break;
            case 26: // go to main lab
                // if monster sample ! active
                if(monster_sample_2.activeSelf == false){
                    // obj = go to main lab
                    objective_screen.SetActive(true);
                    objective_text.text = "-> Go to Chemical Identifier [Main Lab]";
                } else {
                    // obj = collect monster sample
                    objective_screen.SetActive(true);
                    objective_text.text = "-> Collect monster sample";
                }
                
                
                // player needs to interact with chem_identifier
                // modify the chemical identifier dialogue
                chem_identifier.GetComponent<dialogue_item>().sentences = new string[3] {"[You tinker with some parts in the machine]", "[Sample can be collected now]", "Ok now Employee 15. Use the sample with the chemical mixer. Lets see if we can get to the bottom of this"};
                chem_identifier.GetComponent<dialogue_item>().speakers = new string[3] {"Chemical Identifier", "Chemical Identifier", "INTRA"};
                chem_identifier.GetComponent<dialogue_item>().choices = "(Z) - Continue";
                // if player is near chem_identifier
                if(Vector3.Distance(player.transform.position, chem_identifier.transform.position) <= 0.5f && dialogue_box.GetComponent<Dialogue>().active == false){
                    // if input key "Z" is pressed collect suit
                    if(Input.GetKeyDown(KeyCode.Z)){
                        // trigger chemical identifier dialogue
                        string[] lines = chem_identifier.GetComponent<dialogue_item>().sentences;
                        string[] speakers = chem_identifier.GetComponent<dialogue_item>().speakers;
                        string choices = chem_identifier.GetComponent<dialogue_item>().choices;
                        dialogue_box.GetComponent<Dialogue>().choices = choices;
                        dialogue_box.GetComponent<Dialogue>().speakers = speakers;
                        dialogue_box.GetComponent<Dialogue>().sentences = lines;
                        dialogue_box.SetActive(true);
                        dialogue_box.GetComponent<Dialogue>().startDialogue();
                        // set monster sample to active
                        monster_sample_2.SetActive(true);
                    }
                }
                // if player is near sample
                if(Vector3.Distance(player.transform.position, monster_sample_2.transform.position) <= 0.5f && dialogue_box.GetComponent<Dialogue>().active == false){
                    // if input key "Z" is pressed collect suit
                    if(Input.GetKeyDown(KeyCode.Z)){
                        // set monster sample to inactive
                        monster_sample_2.SetActive(false);
                        sample_collected = true;
                        // go to scene 24
                        scene_index = 24;
                    }
                }
                break;
            case 27: // caught by monster
                if(scene_flag == true){
                    // player can't move
                    player.GetComponent<player_behavior>().enabled = false;
                    scene_27();
                    scene_flag = false;
                } else {
                    // black screen
                    deadScreen.SetActive(true);
                }
                break;
            case 28: // end of demo 
                // obj = to be continued
                objective_screen.SetActive(true);
                objective_text.text = "-> To be continued...";
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
        "This area of the ocean is owned by SEO. You are to ensure all systems are maintained to support ongoing operations by keeping the area secure.", 
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

    void scene_8(){
        string[] lines = new string[1] {"<Ask INTRA about containement field?>"};
        string[] speakers = new string[1] {"Employee 15"};
        string choices = "(Z) Yes, (X) No";
        dialogue_box.GetComponent<Dialogue>().choices = choices;
        dialogue_box.GetComponent<Dialogue>().speakers = speakers;
        dialogue_box.GetComponent<Dialogue>().sentences = lines;
        dialogue_box.SetActive(true);
        dialogue_box.GetComponent<Dialogue>().startDialogue();
    }

    void scene_9(){
        string[] lines = new string[3] {"INTRA? Can the containment be fixed?", 
        "Not at the moment", 
        "The tools needed are not here. I have alerted the company already. No worries, it will hold long enough."};
        string[] speakers = new string[3] {"Employee 15", "INTRA", "INTRA"};
        string choices = "(Z) Continue";
        dialogue_box.GetComponent<Dialogue>().choices = choices;
        dialogue_box.GetComponent<Dialogue>().speakers = speakers;
        dialogue_box.GetComponent<Dialogue>().sentences = lines;
        dialogue_box.SetActive(true);
        dialogue_box.GetComponent<Dialogue>().startDialogue();
    }

    void scene_12(){
        string[] lines = new string[3] {"Fascinating. Can you understand me?", "*indecipherable*", "I'll try again another time" };
        string[] speakers = new string[3] {"Employee 15", "Monster", "Employee 15"};
        string choices = "(Z) Continue";
        dialogue_box.GetComponent<Dialogue>().choices = choices;
        dialogue_box.GetComponent<Dialogue>().speakers = speakers;
        dialogue_box.GetComponent<Dialogue>().sentences = lines;
        dialogue_box.SetActive(true);
        dialogue_box.GetComponent<Dialogue>().startDialogue();
    }

    void scene_14(){
        lights_flicker();
        string[] lines = new string[8] {"INTRA, why are the lights flickering?", 
        "The lights outside the station are close to failing.", "Is that bad?",
        "The lights dissuade any nearby life from approaching or damaging the station.",
        "Does our monster friend in the Containment Room count as nearby life?",
        "Origins of the monster are unknown. Perhaps.",
        "(sigh) Right, I’ll get on that.",
        "Of course. All necessary equipment can be found in the Equipment Room."};
        string[] speakers = new string[8] {"Employee 15", "INTRA", "Employee 15", "INTRA", "Employee 15", "INTRA", "Employee 15", "INTRA"};
        string choices = "(Z) Continue";
        dialogue_box.GetComponent<Dialogue>().choices = choices;
        dialogue_box.GetComponent<Dialogue>().speakers = speakers;
        dialogue_box.GetComponent<Dialogue>().sentences = lines;
        dialogue_box.SetActive(true);
        dialogue_box.GetComponent<Dialogue>().startDialogue();
    }

    void scene_20(){
        string[] lines = new string[5] {"[a monster appears out of nowhere]", 
        "Swim back to the station!", "[The monster surges towards you]", 
        "I’ve closed the security door! Run!", "<Then the only safe room right now is the AI room.. I need to go back through the doors above>"};
        string[] speakers = new string[5] {"", "INTRA", "", "INTRA", "Employee 15"};
        string choices = "(Z) Continue";
        dialogue_box.GetComponent<Dialogue>().choices = choices;
        dialogue_box.GetComponent<Dialogue>().speakers = speakers;
        dialogue_box.GetComponent<Dialogue>().sentences = lines;
        dialogue_box.SetActive(true);
        dialogue_box.GetComponent<Dialogue>().startDialogue();
    }

    void scene_21(){
        string[] lines = new string[1] {"[The monster is on the other side of the door. You have 2 choices: 1. Shoot it with your Harpoon, 2. Blind it with your flashlight]"};
        string[] speakers = new string[1] {""};
        string choices = "(Z) Kill it with Harpoon, (X) Blind it with Flashlight";
        dialogue_box.GetComponent<Dialogue>().choices = choices;
        dialogue_box.GetComponent<Dialogue>().speakers = speakers;
        dialogue_box.GetComponent<Dialogue>().sentences = lines;
        dialogue_box.SetActive(true);
        dialogue_box.GetComponent<Dialogue>().startDialogue();
    }

    void scene_22(){
        string[] lines = new string[6] {"Strike it quickly!", 
        "[INTRA opens the door and you shoot the monster]", 
        "Good job, its dead now.", 
        "[A chemical sample is left behind]", 
        "Perfect! We can use this sample to further our research.", 
        "Employee 15, can you take this to the main lab? I believe this is just what we need to eliminate the monsters quickly"};
        string[] speakers = new string[6] {"INTRA", "", "INTRA", "", "INTRA", "INTRA"};
        string choices = "(Z) Continue";
        dialogue_box.GetComponent<Dialogue>().choices = choices;
        dialogue_box.GetComponent<Dialogue>().speakers = speakers;
        dialogue_box.GetComponent<Dialogue>().sentences = lines;
        dialogue_box.SetActive(true);
        dialogue_box.GetComponent<Dialogue>().startDialogue();
    }

    void scene_25(){
        string[] lines = new string[7] {"[INTRA opens the door and you blind the monster]", 
        "[the monster escapes fearfully into the vents]", 
        "Well that’s disappointing. A sample is required to further your research eventually.",
        "That sample could've been just what we needed to eliminate the monsters quickly.", 
        "Sorry, I couldn’t kill it. Can I get a sample from the monster in the containment room instead?", 
        "Maybe. But it requires a tool you currently don't have.",
        "The main lab should have the parts needed to build it however."};
        string[] speakers = new string[7] {"", "", "INTRA", "INTRA", "Employee 15", "INTRA", "INTRA"};
        string choices = "(Z) Continue";
        dialogue_box.GetComponent<Dialogue>().choices = choices;
        dialogue_box.GetComponent<Dialogue>().speakers = speakers;
        dialogue_box.GetComponent<Dialogue>().sentences = lines;
        dialogue_box.SetActive(true);
        dialogue_box.GetComponent<Dialogue>().startDialogue();
    }

    void scene_27(){
        string[] lines = new string[1] {"[The monster catches you and you are killed]"};
        string[] speakers = new string[1] {""}; 
        string choices = "(Z) Continue";
        dialogue_box.GetComponent<Dialogue>().choices = choices;
        dialogue_box.GetComponent<Dialogue>().speakers = speakers;
        dialogue_box.GetComponent<Dialogue>().sentences = lines;
        dialogue_box.SetActive(true);
        dialogue_box.GetComponent<Dialogue>().startDialogue();
    }
    void lights_flicker(){
        // turn light object off and on
        light2.SetActive(true);
        if(light_object.activeSelf == true){
            light_object.SetActive(false);
        } else {
            light_object.SetActive(true);
        }
    }
}

