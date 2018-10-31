using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CloneMovement : MovementPatternAbstract
{
    Ray ray;
    public float scaler = 15f;
    Vector2 posToMove;
    Vector3 relative_vector;
    bool positionSet;
    float distance;
    float minDistance = .01f;

    private void Start()
    {
        positionSet = false;
    }
    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButtonDown(1))
        {
            setPosition();
            moveClone();
            positionSet = true;
        }

        float distance_traveled = new Vector2(transform.position.x - ray.origin.x, transform.position.y - ray.origin.y).magnitude;
        if (((distance - distance_traveled) > minDistance) && positionSet)
        {
            moveClone();
        }
        else if (positionSet)
        {
            GetComponent<Rigidbody2D>().velocity = new Vector3(0, 0, 0);
            positionSet = false;
        }
    }


    private void setPosition()
    {
        posToMove = Camera.main.ScreenToWorldPoint(Input.mousePosition);
    }

    public void moveClone()
    {
        relative_vector = new Vector2(posToMove.x - transform.position.x, posToMove.y - transform.position.y);
        distance = relative_vector.magnitude;
        relative_vector.Normalize();
        ray.origin = transform.position;
        ray.direction = relative_vector;
        Vector2 moveVector = relative_vector * entity.moveSpeed;
        GetComponent<Rigidbody2D>().velocity = moveVector;
        Debug.Log(GetComponent<Rigidbody2D>().velocity.magnitude);
    }
}