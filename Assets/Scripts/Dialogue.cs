using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Dialogue : MonoBehaviour
{
    public TextMeshProUGUI textDisplay;
    public string[] sentences;
    public float typingSpeed;
    private int index;
    public bool active;
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
        }
        
    }
    public void startDialogue(){
        active = true;
        textDisplay.text = string.Empty;
        index = 0;
        StartCoroutine(TypeLine());
    }
    IEnumerator TypeLine(){
        foreach(char letter in sentences[index].ToCharArray()){
            textDisplay.text += letter;
            yield return new WaitForSeconds(typingSpeed);
        }

    }
    void nextLine(){
        if(index < sentences.Length - 1){
            index++;
            textDisplay.text = string.Empty;
            StartCoroutine(TypeLine());
        }
        else{
            active = false;
            gameObject.SetActive(false);
        }
    }
}
