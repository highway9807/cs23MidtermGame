using UnityEngine;
using UnityEngine.SceneManagement;

public class ExitFurnaceRoom : MonoBehaviour
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
