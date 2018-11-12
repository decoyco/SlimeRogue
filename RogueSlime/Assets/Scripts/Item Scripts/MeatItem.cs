using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeatItem : ItemAbstract  {

    public float healthToGain;
    public MEATer meater;

    private void Start()
    {
        meater = FindObjectOfType<MEATer>();
        checkValidSetUp();
    }

    public override void onPickupAction(Entity other)
    {

        if (other.GetComponent<PlayerEntity>() != null)
        {
            other.GetComponent<PlayerEntity>().numMeat++;
            if (other.GetComponent<PlayerEntity>().numMeat >= 5)
            {
                other.GetComponent<PlayerEntity>().numMeat = 0;
                other.GetComponent<Entity>().max_health++;
            }
        }
        healthToGain = other.max_health / 3;
        other.healthPoints += healthToGain;
    }
}
