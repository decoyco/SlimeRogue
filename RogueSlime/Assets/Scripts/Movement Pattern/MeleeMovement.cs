using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Used for Enemies to move towards the player
/// </summary>
public class MeleeMovement : MovementPatternAbstract {
    public Vector3 relative_position;
	// Update is called once per frame
	void Update () {
        relative_position = GameObject.FindObjectOfType<PlayerEntity>().gameObject.transform.position - transform.position;
        relative_position.Normalize();
        /*gameObject.GetComponent<Rigidbody2D>().velocity = new Vector3(relative_position.x * Time.deltaTime * gameObject.GetComponent<Entity>().SPEED,
                                                                      relative_position.y * Time.deltaTime * gameObject.GetComponent<Entity>().SPEED,
                                                                      relative_position.z);*/
        transform.position += relative_position * Time.deltaTime * .1f * gameObject.GetComponent<Entity>().moveSpeed;
    }
}
