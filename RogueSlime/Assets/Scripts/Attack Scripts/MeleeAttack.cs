using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeleeAttack : RangeAttack{
    private bool collidedWall;
    private bool dashing;
    private float tempMoveSpeed;
    private Vector3 relative_position;
    //private Vector3 posToStab;
    //private GameObject weapon;
    private Vector3 direction;
    private Vector3 dashForce;

    public float attackingMoveSpeed = 3f;
    public float weaponDistance = .08f;
    public float dashSpeed;
    public float weaponSide = 65f;

    //Slash is set manually per weapon
    public GameObject slash;

    /**
     * @param attack 
     */ 
    private void Update()
    {
        orientWeapon(gameObject.transform.parent.GetComponent<Entity>().targetVector + transform.parent.position);
    }

    public void orientWeapon(Vector3 v)
    {
        //calculate
        setRelativePosition(v);
        Quaternion rot = getWeaponRotation(v);

        //swing
        transform.rotation = rot;
        transform.position = relative_position * weaponDistance + transform.parent.position;
    }

    private void OnCollisionStay2D(Collision2D collision)
    {
        if (dashing)
        {
            if (collision.gameObject.CompareTag("Wall"))
            {
                collidedWall = true;
            }
        }
    }

    public override void attack(GameObject obj, GameObject e)
    {
        //TODO this is for enemy
        //activate when close
    }

    public override void attack(Vector3 v, GameObject e)
    {
        weaponSide = weaponSide * -1;
        Vector3 posToAttack = v;

        GameObject slash_instance = fireProjectile(slash, transform.parent.position, posToAttack, 0.05f, -90f, slash.GetComponent<Projectile>().shootSpeed);

        //Gives slash properties of weapon
        slash_instance.GetComponent<Equip>().damage = GetComponent<Equip>().damage;
        slash_instance.GetComponent<Equip>().slow = GetComponent<Equip>().slow;
        slash_instance.GetComponent<Equip>().burn = GetComponent<Equip>().burn;
    }

    // posToPoint = the position of the object to point to
    public void setRelativePosition(Vector3 posToPoint)
    {
        relative_position = posToPoint - transform.parent.position;
        relative_position.Normalize();
        //OFFSETTING POLAR ANGLE OF RELATIVE POSITION
        float angle = Mathf.Atan2(relative_position.y, relative_position.x) + weaponSide;
        relative_position = new Vector3(Mathf.Cos(angle), Mathf.Sin(angle), 0);
        relative_position.Normalize();
        //transform.position += relative_position * Time.deltaTime * .1f;
    }

    private Quaternion getWeaponRotation(Vector3 v)
    {
        //relative_position = v - transform.position;
        //relative_position.Normalize();
        float rot_z = Mathf.Atan2(relative_position.y, relative_position.x) * Mathf.Rad2Deg;
        return Quaternion.Euler(0f, 0f, rot_z - 90);
    }

    /*
    public void StopDash()
    {
        if (!collidedWall)
        {
            GetComponent<Rigidbody2D>().Sleep();
            GetComponent<Rigidbody2D>().WakeUp();
            GetComponent<PlayerEntity>().doingAction = false;
            dashing = false;
        }
        else
        {
            GetComponent<Rigidbody2D>().Sleep();
            GetComponent<Rigidbody2D>().WakeUp();
            collidedWall = false;

            GetComponent<PlayerEntity>().doingAction = false;
            dashing = false;
        }
    }
    */

}
