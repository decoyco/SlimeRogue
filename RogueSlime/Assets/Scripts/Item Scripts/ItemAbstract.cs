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
}
