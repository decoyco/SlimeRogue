using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class ItemAbstract : MonoBehaviour
{
    public float moveSpeed = 1.2f;
    public float acceleration = 10;
    public bool isAttractable;

    private bool isMoveToTriggered = false;
    private Vector3 locationToMoveTo;

    public abstract void onPickupAction(Entity other);

    public void Start()
    {
        if (gameObject.GetComponent<Rigidbody2D>() == null)
        {
            Debug.LogWarning(gameObject + "does not have a RigidBody2D and is an item. Add one to enable attracting and collisions");
            isAttractable = false;
        }
    }

    public void destroy()
    {
        Destroy(gameObject);
    }

    public void moveTo(GameObject other)
    {
        moveTo(other.transform.position);
    }

    public void moveTo(Vector3 v)
    {
        locationToMoveTo = v;
        isMoveToTriggered = true;
    }

    public void stopMoveTo()
    {
        isMoveToTriggered = false;
    }

    public void Update()
    {
        if (isMoveToTriggered)
        {
            Vector3 relative_position = locationToMoveTo - transform.position;
            float mag = relative_position.magnitude;
            relative_position.Normalize();

            //Note Z pos also affects this
            transform.position += relative_position * Time.deltaTime * .1f * moveSpeed * 1/(mag + 1/(acceleration * 10));
        }

    }

}