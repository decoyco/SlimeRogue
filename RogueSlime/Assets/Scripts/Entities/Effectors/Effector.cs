using System;
using UnityEngine;

public abstract class Effector : MonoBehaviour
{
    protected Entity entity;
    protected EffectType type;
    protected float timeElapsed = 0f;
    public float timeDuration;

    public enum EffectType
    {
        None,
        Poison,
        Paralysis,
        Slow,
        Burn,
    };

    public EffectType getType()
    {
        return type;
    }

    private void Update()
    {
        if (type != EffectType.None)
        {
            //Check if item has duration
            if (timeDuration > 0)
            {
                //If duration of item is not complete, apply item effect and increment time elapsed by Time.deltaTime
                if (timeElapsed < timeDuration)
                {
                    DoEffect();
                    timeElapsed += Time.deltaTime;
                }
                //Otherwise destroy script
                else
                {
                    RevertEffect();
                    Destroy(this);
                }
            }
            //otherwise do one-time effect and destroy object
            else
            {
                DoEffect();
                Destroy(this);
            }
        }
    }

    protected void changeColor(Color c)
    {
        SpriteRenderer rend = entity.gameObject.GetComponent<SpriteRenderer>();
        rend.color = c;
    }

    //ITEM EFFECT GOES HERE
    protected abstract void DoEffect();

    //REVERT EFFECT HERE (for effects like paralysis)
    protected abstract void RevertEffect();
}
