/****************************************************************
* 
*  BearAttack.cs
*  Rui Zhu, Feb 17
* 
*  This script allows for a polar bear to harm a player on contact. 
*  Used by Prefab "Polar_Bear".
* 
*****************************************************************/

using UnityEngine;

public class BearAttack : MonoBehaviour
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
            Debug.Log(">>> Bear: Gawr >:3"); // Replace this line with actual bear interactions
        }
    }
}
