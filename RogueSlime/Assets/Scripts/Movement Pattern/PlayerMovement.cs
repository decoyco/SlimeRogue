using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MovementPatternAbstract {
    
    private float xIn = 0;
    private float yIn = 0;
    public float instantScaleSpeed;
    public float heaviness;


    // Update is called once per frame
    void Update () {
        //set to default move speed if not shooting. not sure if this is needed
        //if (!gameObject.GetComponent<PlayerEntity>().doingAction)
        {
            checkInput();
            float xAccel = (Mathf.Pow(Input.GetAxis("Horizontal"), heaviness)); 
            float yAccel = (Mathf.Pow(Input.GetAxis("Vertical"), heaviness));

            //apply initial and fill remaining with acceleration ratio
            Vector2 move = Vector2.ClampMagnitude(new Vector2(xIn + ((gameObject.GetComponent<Entity>().moveSpeed - instantScaleSpeed) * xAccel),
                                                              yIn + ((gameObject.GetComponent<Entity>().moveSpeed - instantScaleSpeed) * yAccel)),
                                                  1.0f);
            //Debug.Log((move * gameObject.GetComponent<Entity>().moveSpeed).ToString("F4"));
            gameObject.GetComponent<Rigidbody2D>().velocity = (move * gameObject.GetComponent<Entity>().moveSpeed);
        }
    }

    private void checkInput()
    {
        checkUp();
        checkDown();
        checkLeft();
        checkRight();
    }

    private void checkUp()
    {
        if (Input.GetKeyDown(KeyCode.W))
            yIn += instantScaleSpeed;

        if (Input.GetKeyUp(KeyCode.W))
            yIn -= instantScaleSpeed;
    }

    private void checkDown()
    {
        if (Input.GetKeyDown(KeyCode.S))
            yIn -= instantScaleSpeed;

        if (Input.GetKeyUp(KeyCode.S))
            yIn += instantScaleSpeed;
    }

    private void checkLeft()
    {
        if (Input.GetKeyDown(KeyCode.A))
            xIn -= instantScaleSpeed;

        if (Input.GetKeyUp(KeyCode.A))
            xIn += instantScaleSpeed;
    }

    private void checkRight()
    {
        if (Input.GetKeyDown(KeyCode.D))
            xIn += instantScaleSpeed;

        if (Input.GetKeyUp(KeyCode.D))
            xIn -= instantScaleSpeed;
    }
}
