using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Paralysis : Effector
{
    bool onEntity;
    float defaultSpeed;
    
    private void Start()
    {
        onEntity = (gameObject.GetComponent<Entity>() != null);
        type = EffectType.Paralysis;
        defaultSpeed = gameObject.GetComponent<Entity>().moveSpeed;
        timeElapsed = 0f;
    }
    protected override void DoEffect()
    {
    }

    protected override void RevertEffect()
    {

        if (onEntity)
            gameObject.GetComponent<Entity>().moveSpeed = defaultSpeed;

    }
}
