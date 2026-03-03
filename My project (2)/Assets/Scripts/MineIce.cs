/****************************************************************
* 
*  MineIce.cs
*  Rui Zhu, Feb 17
* 
*  This script allows for an ice to by mined by a player on contact. 
*  Used by Prefab "Ice".
* 
*****************************************************************/

using UnityEngine;
using System.Collections.Generic;
using System.Collections;

public class MineIce : MonoBehaviour
{
    private GameObject player;

    public  GameObject progressBar;
    private GameObject progressBarInstance;
    private Transform barFill; // The actual bar

    public float miningTime = 2f;
    private float timer;
    private bool inContact = false;
    private float heightOffset = 0.6f;

    void Start()
    {
        player = GameObject.FindWithTag("Player"); 
    }

    void Update()
    {
        if (inContact)
        {
            timer += Time.deltaTime;

            float progress = 1f - (timer / miningTime);

            if (progressBarInstance != null)
                progressBarInstance.transform.localScale = new Vector2(progress, 1f);

            if (timer >= miningTime)
            {
                Destroy(progressBarInstance);
                Destroy(gameObject);
            }
        }
    }

    void OnCollisionEnter2D(Collision2D other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            inContact = true;
            timer = 0f;

            if (progressBarInstance == null)
            {
                SpawnProgressBar();
            }
        }
    }

    void OnCollisionExit2D(Collision2D other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            inContact = false;
            timer = 0f;

            if (progressBarInstance != null)
            {
                Destroy(progressBarInstance);
            }
        }
    }

    void SpawnProgressBar()
    {
        Vector2 spawnPosition = new Vector2(
            transform.position.x,
            transform.position.y + heightOffset
        );

        progressBarInstance = Instantiate(progressBar, spawnPosition, Quaternion.identity);
    }
}
