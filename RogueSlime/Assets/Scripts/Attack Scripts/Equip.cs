using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Equip : MonoBehaviour
{
    protected Renderer rend;

    public float coolDown;
    public float damage;
    public bool isSlime = false;
    public bool slow;
    public bool burn;
    public GameObject parentObeject;
    public GameObject[] effects;

    public void setParentObject(GameObject g)
    {
        parentObeject = g;
        if (g.CompareTag("Player"))
        {
            isSlime = true;
        }
    }

    public GameObject getParentObject()
    {
        return parentObeject;
    }

    protected virtual void OnTriggerEnter2D(Collider2D other)
    {
        Entity entity = other.gameObject.GetComponent<Entity>();
        if (entity != null)
        {
            GiveEffects(other.gameObject);
        }
    }

    protected void GiveEffects(GameObject thing)
    {
        //thing.AddComponent<Paralysis>();
        //thing.GetComponent<Paralysis>().timeDuration = 5;

        foreach (GameObject effect in effects)
        {
            if (effect.GetComponent<Paralysis>() != null)
            {
                thing.AddComponent<Paralysis>();
                thing.GetComponent<Paralysis>().timeDuration = effect.GetComponent<Paralysis>().timeDuration;

            }
            else if (effect.GetComponent<Burn>() != null)
            {
                thing.AddComponent<Burn>();
                thing.GetComponent<Burn>().timeDuration = effect.GetComponent<Burn>().timeDuration;
            }
            else if (effect.GetComponent<Slow>() != null)
            {
                if (thing.GetComponent<Slow>() == null)
                {
                    thing.AddComponent<Slow>();
                    thing.GetComponent<Slow>().timeDuration = effect.GetComponent<Slow>().timeDuration;
                }

            }
        }

    }
}
