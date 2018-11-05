using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerEntity : Entity {
    public GameObject cdSliderInstance1;
    public GameObject cdSliderInstance2;
    public LevelManager level_manager;
    public GameObject cdSlider1;
    public GameObject cdSlider2;
    Vector3 sprite_extents;
    public bool hasNewMeleeWeapon = false;
    public float percent_health;
    public int numMeat = 0;
    Vector2 moving = new Vector2(0, 0);

    void Start()
    {
        createWeaponInstance();
        createCDSliderInstances();
        moveSpeed = DEFAULT_SPEED;
        slowing = false;
        burning = false;
        slowLast = 0f;
        sprite_extents = gameObject.GetComponent<SpriteRenderer>().sprite.bounds.extents;
        //Physics.IgnoreCollision(FindObjectsOfType<Entity>().GetComponent<Collider>(), GetComponent<Collider>());
        spawnAttractBubble();
    }

    // Update is called once per frame
    void Update()
    {
        //Debug.Log(targetVector);
        
        setTargetVector(Input.mousePosition);
        cdSliderInstance1.transform.position = Camera.main.WorldToScreenPoint(new Vector3(transform.position.x - sprite_extents.x * 1.3f, transform.position.y, 0));
        cdSliderInstance2.transform.position = Camera.main.WorldToScreenPoint(new Vector3(transform.position.x + sprite_extents.x * 1.3f, transform.position.y, 0));
    }

    public new void setTargetVector(Vector3 v)
    {
        v.x = v.x - Camera.main.WorldToScreenPoint(transform.position).x;
        v.y = v.y - Camera.main.WorldToScreenPoint(transform.position).y;
        targetVector = v;
    }


    private void OnTriggerEnter2D(Collider2D other)
    {
        //if the other is a projectile
        hitProjectile(other);

        //if the other is a Drop
        hitDrop(other);

        checkHealth();
    }

    public void hitDrop(Collider2D other)
    {
        ItemAbstract item = other.gameObject.GetComponent<ItemAbstract>();
        if (item != null)
        {
            item.onPickupAction(gameObject.GetComponent<Entity>());
            item.destroy();
        }
        // Equip pickup
        /*
        if (drop && drop.getItemToDrop() != null)
        {
            gameObject.GetComponent<Entity>().max_health += 1;
            gameObject.GetComponent<Entity>().healthPoints += 1;
            gameObject.GetComponent<Entity>().equip = drop.getItemToDrop();
            //changes skillindicator sprite
            if (CompareTag("Player"))
            {
                GetComponent<PlayerAttackPattern>().CancelInvoke();
                FindObjectOfType<SkillIndicator>().change_to(gameObject.GetComponent<Entity>().equip.gameObject.GetComponent<SpriteRenderer>().sprite);
            }
            Destroy(drop.gameObject);

            Destroy(weaponInstance);
            createWeaponInstance();
        }
        */
    }
    void createCDSliderInstances()
    {
        cdSliderInstance1 = Instantiate(cdSlider1, FindObjectOfType<Canvas>().transform.position, Quaternion.Euler(0,0,90));
        cdSliderInstance1.GetComponent<PlayerCooldownSlider>().player = gameObject;
        cdSliderInstance2 = Instantiate(cdSlider2, FindObjectOfType<Canvas>().transform.position, Quaternion.Euler(0, 0, 90));
        cdSliderInstance2.GetComponent<PlayerCooldownSlider>().player = gameObject;
    }

    public override void onDamage()
    {
    }

    public override void checkHealth()
    {
        if(healthPoints > max_health)
        {
            healthPoints = max_health;
        }
        if (healthPoints <= 0)
        {
            onDeath();
        }
        percent_health = healthPoints / max_health;
        updateSize();
    }

    public override void onDeath()
    {
        Debug.Log("player died");
        dropDrop();
        Destroy(gameObject);
    }

    private void updateSize()
    {
        float size = 0f;
        if (percent_health < .25f)
            size = .6f;
        else if (percent_health < .5f)
            size = .8f;
        else if (percent_health < .75f)
            size = .9f;
        else
            size = 1;
        transform.localScale = new Vector3(size, size, 1);
    }
}
