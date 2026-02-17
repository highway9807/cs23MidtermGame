/****************************************************************
* 
*  PlayerMove.cs
*  Rui Zhu, Feb 17
* 
*  This script allows for using WASD and arrow keys to control a player's 
*  movement. Used by GameObj "Player" in scene Ice_Mine_1.
* 
*****************************************************************/

using UnityEngine;

public class PlayerMove : MonoBehaviour
{
    public float speed = 5f; 
    private Rigidbody2D rb; 
    private Vector2 movement; 

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    // Update() runs once per user's frame, used to capture input
    void Update()
    {
        movement.x = Input.GetAxisRaw("Horizontal"); // AD or lr
        movement.y = Input.GetAxisRaw("Vertical");   // WS or ud
    }

    // FixedUpdate() runs every 20 ms, used for physics movement
    void FixedUpdate()
    {
        rb.velocity = movement * speed;
    }
}
