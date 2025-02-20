using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class level_move : MonoBehaviour
{
    public int sceneBuildIndex;
    public string sceneName;

    public void OnCollision2D(Collider2D other)
    {

        if(other.CompareTag("Player") && !other.isTrigger)
        {
            // print confirmation to console
            Debug.Log("Player has entered the level move trigger");
            // load the next scene
            SceneManager.LoadScene(sceneName);
        }
    }
}
