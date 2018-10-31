using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//NOTE:    try not to add functionality here unless absolutely needed. the main use of these equip classes is to hold values.
//         provide functionality in sperate classes (e.g. WeaponAttack) and check for inputs in the PlayerEntity class if needed.
//         this is to avoid confusion of which scripts do what. new scripts are fine too if they do a specific action, grouping of them can be down at a different time
//         these equip classes are meant to represent an object to be acted upon, not the action itself.

public class Weapon : Equip {
    //public float dashSpeed;
    private Vector3 relative_position;
    private Vector3 dashForce;
    private GameObject spawnedInstance;
    private Color slime_green;

    private void Start()
    {
        slime_green = new Color(.557f, .792f, .322f, 1f);
        rend = GetComponent<Renderer>();
        if (isSlime)
        {
            rend.material.color = slime_green;
        }
    }

    private void Update()
    {
        
        if (isSlime)
        {
            rend.material.color = slime_green;
            isSlime = false; // to stop it from constantly setting
        }
       
    }

    public void setSpawnedInstance(GameObject g)
    {
        spawnedInstance = g;
    }

    public GameObject getSpawnedInstance()
    {
        return spawnedInstance;
    }
}
