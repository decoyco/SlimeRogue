using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class ItemAbstract : MonoBehaviour
{
    public float moveSpeed = 1;
    private bool isMoveToTriggered = false;
    private Vector3 locationToMoveTo;

    public abstract void onPickupAction(Entity other);

    public void destroy()
    {
        Destroy(gameObject);
    }

    public void moveTo(GameObject other)
    {
        //NOTE MAY NEED TO UPDATE THIS INSTEAD OF JUST GETTING ONCE
        //OR MAYBE CONTINUOUS CALLS WHEN IN RANGE ARE OK
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
            //TODO float mag = relative_position.magnitude/3;
            relative_position.Normalize();


            transform.position += relative_position * Time.deltaTime * .1f * moveSpeed; // * 1/(mag);
        }

    }

}