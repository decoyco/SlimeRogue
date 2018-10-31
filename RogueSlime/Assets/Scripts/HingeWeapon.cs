using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//NOTE:    try not to add functionality here unless absolutely needed. the main use of these equip classes is to hold values.
//         provide functionality in sperate classes (e.g. WeaponAttack) and check for inputs in the PlayerEntity class if needed.
//         this is to avoid confusion of which scripts do what. new scripts are fine too if they do a specific action, grouping of them can be down at a different time
//         these equip classes are meant to represent an object to be acted upon, not the action itself.


public class HingeWeapon : Equip {

    public float distanceFromObj;
    public float heaviness;

    public float dashSpeed;
    private Vector3 relative_position;
    private Vector3 dashForce;

    private void Start()
    {
        rend = GetComponent<Renderer>();
        if (isSlime)
        {
            rend.material.color = Color.green;
        }
    }

    // Update is called once per frame
    private void Update()
    {
        if (transform.parent.gameObject.CompareTag("Player"))
        {
            isSlime = true;
        }
        else
            isSlime = false;
        if (isSlime)
        {
            rend.material.color = Color.green;
        }
    }
}
