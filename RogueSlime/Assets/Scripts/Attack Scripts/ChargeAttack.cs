using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChargeAttack : MonoBehaviour, AttackType
{
    private Vector3 relative_position;

    private void Start()
    {
        transform.parent.GetComponent<Entity>().getWeaponInstance().layer = 13;
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
        
        charge(transform.parent.GetComponent<Entity>().equip, posToShoot);

    }

    public void charge(GameObject e, Vector3 posToShoot)
    {
        if(transform.parent.GetComponent<PlayerMovement2>())
            transform.parent.GetComponent<PlayerMovement2>().acting = true;
        posToShoot.Normalize();
        transform.parent.GetComponent<Rigidbody2D>().velocity = new Vector3(posToShoot.x * Time.deltaTime * e.GetComponent<Weapon>().dashSpeed, posToShoot.y * Time.deltaTime * e.GetComponent<Weapon>().dashSpeed, relative_position.z);
        if (transform.parent.GetComponent<PlayerMovement2>() != null)
            Invoke("stopCharge", 0.2f);
    }

    public void stopCharge()
    {
        //Debug.Log(transform.parent);
        if (transform.parent.GetComponent<PlayerMovement2>() != null)
            transform.parent.GetComponent<PlayerMovement2>().acting = false;
        transform.parent.GetComponent<Entity>().shouldTargetVectorUpdate(true);
        //transform.parent.GetComponent<Entity>().getWeaponInstance().layer = 13;
        transform.parent.GetComponent<Rigidbody2D>().velocity = new Vector3(0,0,0);
        transform.parent.GetComponent<AttackPatternAbstract>().attacking = false;
        
    }

}
