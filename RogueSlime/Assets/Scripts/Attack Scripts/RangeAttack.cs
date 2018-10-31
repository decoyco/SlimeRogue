using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RangeAttack : MonoBehaviour, AttackType
{
    
    private Vector3 relative_position;
    private float tempMoveSpeed;
    //public float attackingMoveSpeed = 3f;

    //TODO change to single attack function
    public virtual void attack(GameObject t, GameObject e)
    {
        /*
        if (t == null)
        {
            return;
        }
        Vector3 posToShoot = t.transform.position;
        fireProjectile(projectileToSpawn, transform.parent.position, posToShoot, 0f, 0f, gameObject.GetComponent<Projectile>().shootSpeed);

        //Invoke("fireProjectile", 0.00001f);
        */
    }

    /**
    *@param Vector v position on the screen (not world position) 
    */
    public virtual void attack(Vector3 v, GameObject e)
    {
        Vector3 posToShoot = v;
        //GameObject e = transform.parent.GetComponent<Entity>().equip;
        //Invoke("fireProjectile", gameObject.GetComponent<Entity>().attackAnimationDelay);
        Vector3 spawn_location = new Vector3(transform.parent.position.x, transform.parent.position.y, transform.parent.position.z);
        fireProjectile(e, spawn_location, posToShoot, 0f, 0f, e.GetComponent<Projectile>().shootSpeed);
    }

    public virtual GameObject fireProjectile(GameObject objToSpawn, Vector3 posShootOrigin, Vector3 posToShoot, float spawnDist, float rotationOffest, float velocity)
    {
        posToShoot.Normalize();

        //make a projectile
        GameObject bullet = Instantiate(objToSpawn, (posToShoot * spawnDist) + posShootOrigin, getProjectileRotation(posToShoot, rotationOffest));

        //Sets velocity of laser equal to unit vector of enemy > player multiplied by deltaTime and laser speed
        bullet.GetComponent<Rigidbody2D>().velocity = new Vector3(posToShoot.x * Time.deltaTime * velocity, posToShoot.y * Time.deltaTime * velocity, relative_position.z);

        if (transform.parent.gameObject.layer == 9)
        { 
            bullet.GetComponent<Equip>().isSlime = true;
        }
        //correct layer
        correctLayer(bullet);

        return bullet;
    }

    public void correctLayer(GameObject bullet)
    {
        if (transform.parent.gameObject.layer == 9)
        {
            bullet.layer = 8;
        }
        else if (transform.parent.gameObject.layer == 10)
        {
            bullet.layer = 11;
        }
    }

    private Quaternion getProjectileRotation(Vector3 v, float rotationOffest)
    {
        float rot_z = Mathf.Atan2(v.y, v.x) * Mathf.Rad2Deg;
        return Quaternion.Euler(0f, 0f, rot_z + rotationOffest);
    }


}
