using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BearAttackPattern : AttackPatternAbstract
{
    public float attackCooldown;
    public float stopDistance;
    public LayerMask mask;
    private bool isPathClear = true;

    void Update() {
        checkClearPath();

        //only invokes and if attacking cycle is down and path is clear
        if (!attacking && isPathClear)
        {
            //when charging, check if path clear again to make sure can charge
            startPattern();
        }
    }

    public void checkClearPath()
    {
        Vector3 posToShoot = entity.GetComponent<Entity>().targetVector;
        posToShoot.Normalize();

        float dist = GetComponent<CircleCollider2D>().radius + stopDistance;
        //true when something intersects ray checking only on layer 12, false when nothing
        Debug.DrawRay(transform.position, posToShoot * dist);
        RaycastHit2D hitInfo = Physics2D.Raycast(transform.position, posToShoot, dist, mask);
        if (hitInfo)
        {
            Debug.Log(hitInfo.collider.tag);
            Debug.Log(hitInfo.collider.gameObject.name);
            Debug.Log(hitInfo.collider.gameObject.layer);
            //if no wall in the way of where it is going and not already attacking, start a new charge
            if (hitInfo.collider.tag != "Wall")
            {
                isPathClear = true;
                Debug.Log("No wall in the way, not attacking");
            }
            //if wall in the way of where it is going, stop charge and reset attacking
            else if (hitInfo.collider.tag == "Wall")
            {
                isPathClear = false;
                GetComponent<Entity>().getWeaponInstance().GetComponent<ChargeAttack>().stopCharge();
                GetComponent<EnemyAnimationController>().Idle(GetComponent<Entity>().targetVector);
                attacking = false;
                Debug.Log("Wall in the way, canceling attack");
            }

        }
        //if nothing in the way
        else
        {
            isPathClear = true;
        }
    }

    //starts the invoke and locks
    public override void startPattern()
    {
        Invoke("attack_sequence", attackCooldown);
        attacking = true;
  
    }

    //invokes a attack burst and a cancel
    private void attack_sequence()
    {
        if (isPathClear)
        {
            Debug.Log("Bear attacking");
            GetComponent<Entity>().shouldTargetVectorUpdate(false);
            gameObject.GetComponent<EnemyAnimationController>().Attack(entity.targetVector);
            Invoke("doEntityAttack", .41f);
            Invoke("stop_attack_animation", 1.66f);
        }
        
    }

    public void doEntityAttack()
    {
        entity.doAttack(entity.targetVector);
    }

    private void start_attack_animation()
    {
        gameObject.GetComponent<EnemyAnimationController>().Attack(entity.targetVector);
    }

    private void stop_attack_animation()
    {
        gameObject.GetComponent<EnemyAnimationController>().Idle(entity.targetVector);
    }
}
