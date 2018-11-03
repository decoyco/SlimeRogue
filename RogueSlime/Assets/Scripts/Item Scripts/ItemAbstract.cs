using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class ItemAbstract : MonoBehaviour
{
    public abstract void onPickupAction(Entity other);

    public void destroy()
    {
        Destroy(gameObject);
    }
    
    public void moveTo(GameObject other)
    {
        moveTo(new Vector2(other.transform.position.x, other.transform.position.y));
    }

    public void moveTo(Vector2 v)
    {
        
    }
}
