using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shredder : MonoBehaviour {
    private void OnTriggerEnter2D(Collider2D other)
    {
        Projectile projectile = other.GetComponent<Projectile>();
        if (other.gameObject.GetComponent<Projectile>().dropped == false && other.gameObject.GetComponent<Projectile>().isSlime)
        {
            other.gameObject.GetComponent<Projectile>().dropped = true;
            other.gameObject.GetComponent<Projectile>().spawn_drops();
        }
        if (projectile && projectile.shreddable)
            Destroy(other.gameObject);
    }
}
