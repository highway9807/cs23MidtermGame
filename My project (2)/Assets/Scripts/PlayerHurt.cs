using System.Collections.Generic;
using System.Collections;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class PlayerHurt : MonoBehaviour
{

    Color playerColor;

    void Start()
    {
        playerColor = gameObject.GetComponentInChildren<SpriteRenderer>().color;
    }

    public void OnTriggerEnter2D(Collider2D other){

        if (other.gameObject.tag == "enemyBear"){
            StartCoroutine(DeadByBear());
        }
    }

    IEnumerator DeadByBear(){
        gameObject.GetComponentInChildren<SpriteRenderer>().color = Color.white;
        yield return new WaitForSeconds(0.1f);
        gameObject.GetComponentInChildren<SpriteRenderer>().color = playerColor;
        yield return new WaitForSeconds(0.1f);
        gameObject.GetComponentInChildren<SpriteRenderer>().color = Color.white;
        yield return new WaitForSeconds(0.1f);
        gameObject.GetComponentInChildren<SpriteRenderer>().color = playerColor;
        yield return new WaitForSeconds(0.1f);
         gameObject.GetComponentInChildren<SpriteRenderer>().color = Color.white;
        yield return new WaitForSeconds(0.1f);
        gameObject.GetComponentInChildren<SpriteRenderer>().color = playerColor;
        yield return new WaitForSeconds(0.1f);
        gameObject.GetComponentInChildren<SpriteRenderer>().color = Color.white;
        yield return new WaitForSeconds(0.1f);

        SceneManager.LoadScene("EndLose");
    }


}
