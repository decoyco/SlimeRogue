using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SlimeDrop : ItemAbstract {
    void Start()
    {
        Invoke("stop", Random.Range(.02f, .09f));
    }
    public  override void onPickupAction(Entity other)
    {
        other.healthPoints += 1;
    }
    public void stop()
    {
        gameObject.GetComponent<Rigidbody2D>().velocity *= 0f;
    }
}
