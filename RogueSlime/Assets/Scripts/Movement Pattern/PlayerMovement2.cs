using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement2 : MovementPatternAbstract {
    public bool acting = false;
	// Update is called once per frame
	void Update () {
        if (!acting)
        {
            float xIn = Input.GetAxis("Horizontal");
            float yIn = Input.GetAxis("Vertical");

            Vector2 move = Vector2.ClampMagnitude(new Vector2(xIn, yIn), 1);

            //Debug.Log((move * gameObject.GetComponent<Entity>().moveSpeed).ToString("F4"));
            gameObject.GetComponent<Rigidbody2D>().velocity = (move * entity.moveSpeed);
        }
    }
}
