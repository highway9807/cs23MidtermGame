/****************************************************************
* 
*  MineIce.cs
*  Rui Zhu, Feb 17
* 
*  This script allows for an ice to by mined by a player. 
*  Used by Prefab "Ice".
* 
*****************************************************************/

using UnityEngine;

public class MineIce : MonoBehaviour
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

    void OnCollisionEnter2D (Collision2D other) 
    {
        if (other.gameObject.CompareTag("Player")) {
            Destroy(gameObject);
            Debug.Log(">>> Ice: I have been mined :3"); // Replace this line with cmd to update inventory in Game Handler
        }
    }
}
