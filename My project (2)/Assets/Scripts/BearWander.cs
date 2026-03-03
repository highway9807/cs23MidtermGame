using UnityEngine;

public class BearWander : MonoBehaviour
{
    public bool wander = true;

    private float radius = 5f;
    private float speed = 2f;
    private float stoppingDistance = 0.1f;

    private Vector2 startPosition;
    private Vector2 targetPosition;

    void Start()
    {
        if (wander) {
            startPosition = transform.position;
            PickNewTarget();
        }
    }

    void Update()
    {
        if (wander) MoveToTarget();
    }

    void MoveToTarget()
    {
        transform.position = Vector3.MoveTowards(
            transform.position,
            targetPosition,
            speed * Time.deltaTime
        );

        if (Vector2.Distance(transform.position, targetPosition) <= stoppingDistance)
        {
            PickNewTarget();
        }
    }

    void PickNewTarget()
    {
        Vector2 randomPoint = Random.insideUnitCircle * radius;
        targetPosition = startPosition + new Vector2(randomPoint.x, randomPoint.y);
        
        radius = Random.Range(2f, 3f);
        speed = Random.Range(2f, 4f); 
    }
}