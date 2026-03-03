using UnityEngine;
using System.Collections.Generic;
using System.Collections;


public class BearMove : MonoBehaviour
{
    public bool move = true;
    private char axis = 'x';
    private float amplitude = 1f; 
    private float frequency = 1f; 

    private Vector2 startPosition;

    void Start()
    {
        if (move) {
            startPosition = transform.position;
            amplitude = Random.Range(1f, 1.5f);
            frequency = Random.Range(5f, 8f); 
            int randomAxis = Random.Range(0, 2);
            if (randomAxis == 1) axis = 'y';
        }
    }

    void Update()
    {
        if (move) {
            float offset = Mathf.Sin(Time.time * frequency) * amplitude;

            transform.position = new Vector2(
                startPosition.x + (axis == 'x' ? offset : 0),
                startPosition.y + (axis == 'y' ? offset : 0)
            );
        }
    }
}