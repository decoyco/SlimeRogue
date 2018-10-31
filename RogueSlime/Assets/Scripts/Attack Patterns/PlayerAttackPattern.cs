using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAttackPattern : AttackPatternAbstract
{
    public GameObject reserve_equip;

    bool last_stand = false;
    bool onCoolDown1 = false; //FIRST ATTACK COOLDOWN
    bool onCoolDown2 = false; //SECOND ATTACK COOLDOWN
    bool onMoveDown1 = false; //MoveSpeed slow for move 1
    bool onMoveDown2 = false; //MoveSpeed slow for move 2

    void Update()
    {
        startPattern();
        if(gameObject.GetComponent<PlayerEntity>().healthPoints == 1 && !last_stand)
        {
            last_stand = true;
            GameObject temp = reserve_equip;
            reserve_equip = gameObject.GetComponent<PlayerEntity>().equip;
            gameObject.GetComponent<PlayerEntity>().equip = temp;
            gameObject.GetComponent<PlayerEntity>().createWeaponInstance();
        }
        else if(gameObject.GetComponent<PlayerEntity>().healthPoints > 1 && last_stand)
        {
            last_stand = false;
            GameObject temp = reserve_equip;
            reserve_equip = gameObject.GetComponent<PlayerEntity>().equip;
            gameObject.GetComponent<PlayerEntity>().equip = temp;
            gameObject.GetComponent<PlayerEntity>().createWeaponInstance();
        }
    }

    public override void startPattern()
    {
        /* --------------------outdated-------------------
        if (Input.GetMouseButtonUp(0))
        {
            CancelInvoke();
            entity.moveSpeed = entity.DEFAULT_SPEED;
        }

        if (Input.GetMouseButtonUp(1))
        {
            CancelInvoke();
            entity.moveSpeed = entity.DEFAULT_SPEED;
        }

        if(Input.GetMouseButton(1) && Input.GetMouseButton(0))
        {
            CancelInvoke();
            entity.moveSpeed = entity.DEFAULT_SPEED;
        }
        ------------------------outdated-------------------*/

        //left click
        if ((Input.GetMouseButton(0) && !Input.GetMouseButton(1)) && !onCoolDown1) //&& equip != null 
        {
            //Debug.Log(Input.mousePosition);
            if (entity.getIsRanged() && Time.time - entity.getLastShotTime() >= entity.attackSpeed)
            {
                
                if (entity.gameObject.GetComponent<PlayerEntity>().healthPoints > entity.GetComponent<PlayerEntity>().equip.GetComponent<Projectile>().hp_cost)
                {
                    doEntityAttack();
                    entity.gameObject.GetComponent<PlayerEntity>().healthPoints -= entity.GetComponent<PlayerEntity>().equip.GetComponent<Projectile>().hp_cost;
                    entity.GetComponent<PlayerEntity>().checkHealth();
                    entity.moveSpeed /= 3;
                    onCoolDown1 = true;
                    onMoveDown1 = true;
                    Invoke("resetCoolDown1", entity.GetComponent<Entity>().equip.GetComponent<Equip>().coolDown);
                    Invoke("resetMoveDown1", entity.GetComponent<Entity>().equip.GetComponent<Equip>().coolDown / 3);
                    entity.GetComponent<PlayerEntity>().cdSliderInstance1.GetComponent<PlayerCooldownSlider>().activate(entity.GetComponent<Entity>().equip.GetComponent<Equip>().coolDown);
                }
            }
            else if (!entity.getIsRanged())
            {
                doEntityAttack();
                entity.moveSpeed /= 2;
                onCoolDown1 = true;
                onMoveDown1 = true;
                Invoke("resetCoolDown1", entity.GetComponent<Entity>().equip.GetComponent<Equip>().coolDown);
                Invoke("resetMoveDown1", entity.GetComponent<Entity>().equip.GetComponent<Equip>().coolDown / 3);
                entity.GetComponent<PlayerEntity>().cdSliderInstance1.GetComponent<PlayerCooldownSlider>().activate(entity.GetComponent<Entity>().equip.GetComponent<Equip>().coolDown);
            }
        }

        //right click
        if ((Input.GetMouseButton(1) && !Input.GetMouseButton(0)) && !onCoolDown2 && entity.secondEquip.ToString() != "EmptyEquip (UnityEngine.GameObject)")
        {
            
            if (entity.getIsRanged2() && Time.time - entity.getLastShotTime() >= entity.attackSpeed)
            {
                
                if (entity.gameObject.GetComponent<PlayerEntity>().healthPoints > entity.GetComponent<PlayerEntity>().secondEquip.GetComponent<Projectile>().hp_cost)
                {
                    doEntitySecondAttack();
                    entity.gameObject.GetComponent<PlayerEntity>().healthPoints -= entity.GetComponent<PlayerEntity>().secondEquip.GetComponent<Projectile>().hp_cost;
                    entity.GetComponent<PlayerEntity>().checkHealth();
                    entity.moveSpeed /= 3;
                    onCoolDown2 = true;
                    onMoveDown2 = true;
                    Invoke("resetCoolDown2", entity.GetComponent<Entity>().secondEquip.GetComponent<Equip>().coolDown);
                    Invoke("resetMoveDown2", entity.GetComponent<Entity>().secondEquip.GetComponent<Equip>().coolDown / 2);
                    entity.GetComponent<PlayerEntity>().cdSliderInstance2.GetComponent<PlayerCooldownSlider>().activate(entity.GetComponent<Entity>().secondEquip.GetComponent<Equip>().coolDown);
                }
            }
            else if (!entity.getIsRanged2())
            {
                doEntitySecondAttack();
                entity.moveSpeed /= 2;
                onCoolDown2 = true;
                onMoveDown2 = true;
                Invoke("resetCoolDown2", entity.GetComponent<Entity>().secondEquip.GetComponent<Equip>().coolDown);
                Invoke("resetMoveDown2", entity.GetComponent<Entity>().secondEquip.GetComponent<Equip>().coolDown / 2);
                entity.GetComponent<PlayerEntity>().cdSliderInstance2.GetComponent<PlayerCooldownSlider>().activate(entity.GetComponent<Entity>().secondEquip.GetComponent<Equip>().coolDown);
            }
        }
    }

    public void doEntityAttack()
    {
        entity.doAttack(entity.targetVector);
    }

    public void doEntitySecondAttack()
    {
        entity.doSecondAttack(entity.targetVector);
    }

    void resetCoolDown1()
    {
        onCoolDown1 = false;
    }

    void resetCoolDown2()
    {
        onCoolDown2 = false;
    }

    void resetMoveDown1()
    {
        if (!onMoveDown2)
            entity.moveSpeed = entity.DEFAULT_SPEED;
        onMoveDown1 = false;
    }

    void resetMoveDown2()
    {
        if (!onMoveDown1)
            entity.moveSpeed = entity.DEFAULT_SPEED;
        onMoveDown2 = false;
    }
}
