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
}
