using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Slow : Effector
{
    bool onEntity;
    float defaultSpeed;
    // Use this for initialization
    void Start ()
    {
        onEntity = (gameObject.GetComponent<Entity>() != null);
        type = EffectType.Slow;
        entity = gameObject.GetComponent<Entity>();
        defaultSpeed = entity.moveSpeed;
    }
    
    protected override void DoEffect()
    {
        if (onEntity)
        {
            onEntity = false;
            changeColor(new Color(.5f, .5f, .5f, 1));
            entity.moveSpeed = defaultSpeed / 2;
        }
    }

    protected override void RevertEffect()
    {
        gameObject.GetComponent<Entity>().moveSpeed = defaultSpeed;
        changeColor(Color.white);
    }
}
