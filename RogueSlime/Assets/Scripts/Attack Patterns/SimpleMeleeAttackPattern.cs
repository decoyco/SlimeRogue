using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SimpleMeleeAttackPattern : AttackPatternAbstract {
    PlayerEntity player;
    Vector3 relative_position;
    public float scaler = .3f;
    public void Start()
    {
        entity = gameObject.GetComponent<Entity>();
        player = FindObjectOfType<PlayerEntity>();
    }
    // Use this for initialization
    public override void startPattern()
    {
        gameObject.GetComponent<EnemyAnimationController>().Attack(entity.targetVector);
        entity.moveSpeed = 0;
        Invoke("doEntityAttack", .4f);
        attacking = true;
        gameObject.GetComponent<EnemyAnimationController>().attacking = true;
    }

    // Update is called once per frame
    void Update () {
        relative_position = player.transform.position - transform.position;
        Vector3 hit_range = relative_position;
        hit_range.Normalize();
        hit_range *= scaler;
        if (!gameObject.GetComponent<EnemyAnimationController>().attacking)
            gameObject.GetComponent<EnemyAnimationController>().Walk(entity.targetVector);
        if (relative_position.magnitude < hit_range.magnitude && !attacking)
            startPattern();
	}

    public void doEntityAttack()
    {
        entity.moveSpeed = entity.DEFAULT_SPEED;
        entity.doAttack(entity.targetVector);
        attacking = false;
        gameObject.GetComponent<EnemyAnimationController>().attacking = false;
    }
}
