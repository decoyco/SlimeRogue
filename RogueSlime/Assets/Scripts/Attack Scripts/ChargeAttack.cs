using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChargeAttack : MonoBehaviour, AttackType
{
    private Vector3 relative_position;
    private CircleCollider2D collider;
    public float dashSpeed;
    private void Start()
    {
        collider = GetComponent<CircleCollider2D>();
        collider.enabled = false;
    }

    private void Update()
    {
        transform.position = transform.parent.position;
    }

    public void attack(GameObject t, GameObject e)
    {
    }

    /**
    *@param Vector v position on the screen (not world position) 
    */
    public virtual void attack(Vector3 v, GameObject e)
    {
        
        Vector3 posToShoot = v;
        if (transform.parent.GetComponent<PlayerEntity>() == null)
            transform.parent.GetComponent<Entity>().getWeaponInstance().layer = 11;
        else
            transform.parent.GetComponent<Entity>().getWeaponInstance().layer = 8;
        if (gameObject.layer == 8)
            dashSpeed = 150;
        charge( posToShoot);
        
    }

    public void charge( Vector3 posToShoot)
    {
        collider.enabled = true;
        if (transform.parent.GetComponent<PlayerMovement2>())
            transform.parent.GetComponent<PlayerMovement2>().acting = true;
        posToShoot.Normalize();
        transform.parent.GetComponent<Rigidbody2D>().velocity = new Vector3(posToShoot.x * Time.deltaTime * dashSpeed, posToShoot.y * Time.deltaTime * dashSpeed, relative_position.z);
        if (transform.parent.GetComponent<PlayerMovement2>() != null)
            Invoke("stopCharge", 0.2f);
    }

    public void stopCharge()
    {
        collider.enabled = false;
        //Debug.Log(transform.parent);
        if (transform.parent.GetComponent<PlayerMovement2>() != null)
            transform.parent.GetComponent<PlayerMovement2>().acting = false;
        transform.parent.GetComponent<Entity>().shouldTargetVectorUpdate(true);
        //transform.parent.GetComponent<Entity>().getWeaponInstance().layer = 13;
        transform.parent.GetComponent<Rigidbody2D>().velocity = new Vector3(0,0,0);
        transform.parent.GetComponent<AttackPatternAbstract>().attacking = false;
        
    }

}
