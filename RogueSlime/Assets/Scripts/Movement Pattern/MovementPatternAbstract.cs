using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovementPatternAbstract : MonoBehaviour {

    public Entity entity;

    // Use this for initialization
    void Start()
    {
        entity = gameObject.GetComponent<Entity>();
    }
}
