/****************************************************************
* 
*  ExitIceMine.cs
*  Rui Zhu, Feb 17
* 
*  This script allows for an exit to send a player back to the Hub on contact.
*  Used by Prefab "Exit_Ice_Mine".
* 
*****************************************************************/

using UnityEngine;
using UnityEngine.SceneManagement;

public class ExitIceMine : MonoBehaviour
{
    GameObject player;

    void Start()
    {
        player = GameObject.FindWithTag("Player"); 
    }


    void Update()
    {
        // No need to update
    }

    void OnTriggerEnter2D (Collider2D other) 
    {
        if (other.gameObject.CompareTag("Player")) {
            SceneManager.LoadScene("Main_Hub");
        }
    }
}
