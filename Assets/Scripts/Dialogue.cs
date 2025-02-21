using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Dialogue : MonoBehaviour
{
    public TextMeshProUGUI textDisplay, name_text, prompt_text;
    public string[] sentences;
    public string[] speakers;
    public string choices;
    public float typingSpeed;
    private int index;
    public int player_choice;
    public bool active;
    public GameObject INTRA, MC;
    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
     // if press space, next line
        if(Input.GetKeyDown(KeyCode.Z)){
            if(textDisplay.text == sentences[index]){
                nextLine();
            } else{
                StopAllCoroutines();
                textDisplay.text = sentences[index];
            } 
            player_choice = 0;
            
        } else if(Input.GetKeyDown(KeyCode.X)){
            if(textDisplay.text == sentences[index]){
                nextLine();
            } else{
                StopAllCoroutines();
                textDisplay.text = sentences[index];
            } 
            player_choice = 1;
            
        }
        
    }
    public void startDialogue(){
        active = true;
        textDisplay.text = string.Empty;
        prompt_text.text = string.Empty;
        name_text.text = string.Empty;
        index = 0;
        if(speakers[index] == "Employee 15"){
                MC.SetActive(true);
                INTRA.SetActive(false);
            } else if(speakers[index] == "INTRA"){
                MC.SetActive(false);
                INTRA.SetActive(true);
            } else{
                MC.SetActive(false);
                INTRA.SetActive(false);
            }
        StartCoroutine(TypeLine());
    }
    IEnumerator TypeLine(){
        name_text.text = speakers[index];
        prompt_text.text = choices;
        foreach(char letter in sentences[index].ToCharArray()){
            textDisplay.text += letter;
            yield return new WaitForSeconds(typingSpeed);
        }

    }
    void nextLine(){
        if(index < sentences.Length - 1){
            index++;
            if(speakers[index] == "Employee 15"){
                MC.SetActive(true);
                INTRA.SetActive(false);
            } else if(speakers[index] == "INTRA"){
                MC.SetActive(false);
                INTRA.SetActive(true);
            } else{
                MC.SetActive(false);
                INTRA.SetActive(false);
            }
            textDisplay.text = string.Empty;
            StartCoroutine(TypeLine());
        }
        else{
            active = false;
            gameObject.SetActive(false);
        }
    }
}
