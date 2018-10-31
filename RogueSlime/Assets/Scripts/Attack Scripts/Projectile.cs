using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : Equip {
    public bool dropped = false;


    public GameObject drop;
    public bool shreddable = false;
    public int shootSpeed;
    public float despawnTime = 2.5f;
    public float hp_cost;
    Color slime_green = new Color(.557f, .792f, .322f, 1f);
    public void Start()
    {
        rend = GetComponent<Renderer>();
        make_shreddable();
        Invoke("deconstruct", despawnTime);
        if (isSlime)
        {
            rend.material.color = slime_green;
            gameObject.layer = 8;
        }
        
    }

    public void spawn_drops()
    {
        if(isSlime)
            for(int i = 0; i < hp_cost; i++)
            {
                Vector2 velocity = gameObject.GetComponent<Rigidbody2D>().velocity; //LETS CONVERT CARTESIAN VELOCITY TO POLAR FORM!
                float angle = Mathf.Atan(velocity.y / velocity.x);                  //Theta = Tan^-1(y/x)
                float magnitude = velocity.magnitude;
                angle *= Mathf.Rad2Deg;                                             //Converting theta from radians to degrees
                if (velocity.x > 0)             
                    angle -= 180;                                                   //Inversing velocity(only works this way for some reason)
                angle += Random.Range(-45f, 45f);                                   //Offsetting the angle
                angle *= Mathf.Deg2Rad;                                             //Converting to radians for cartesian conversion
                velocity.x = magnitude * Mathf.Cos(angle);                                      //x = rcos(theta) [r = 1 because normalized]
                velocity.y = magnitude * Mathf.Sin(angle);                                      //y"                                       "

                velocity *= 2f;                                                   //scaling down magnitude
                GameObject dropInstance = Instantiate(drop, gameObject.transform.position, Quaternion.identity);
                dropInstance.GetComponent<Rigidbody2D>().velocity = velocity;
            }
    }

    public void make_shreddable()
    {
        shreddable = true;
    }

    
    public void deconstruct()
    {
        Destroy(gameObject);
    }
    

}
