using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class AttackPatternAbstract : MonoBehaviour
{
    public Entity entity;
    public bool attacking = false;

    // Use this for initialization
    void Start()
    {
        entity = gameObject.GetComponent<Entity>();
    }

    public void setEntity(Entity e)
    {
        entity = e;
    }

    public abstract void startPattern();
}
