using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SimpleShootAttackPattern : AttackPatternAbstract
{

    void Start()
    {
        setEntity(gameObject.GetComponent<Entity>());
        Invoke("startPattern", Random.Range(0.5f, 1.5f));
    }

    //starts the invoke
    public override void startPattern()
    {
        
        Invoke("attack_sequence", .1f);
        attacking = true;
    }

    //invokes a looping attack burst and a cancel
    private void attack_sequence()
    {
        InvokeRepeating("doEntityAttack", gameObject.GetComponent<Entity>().attackSpeed, entity.attackAnimationDelay);
        Invoke("stop_attacking", Random.Range(3f, 5f));
    }

    //cancels attack burst invoke and restarts
    private void stop_attacking()
    {
        CancelInvoke();
        gameObject.GetComponent<EnemyAnimationController>().Idle(entity.targetVector);
        attacking = false;
        Invoke("startPattern", Random.Range(2f, 3f));
    }


    public void doEntityAttack()
    {
        entity.doAttack(entity.targetVector);
        gameObject.GetComponent<EnemyAnimationController>().Attack(entity.targetVector);
    }

}
