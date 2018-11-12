using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Entity : MonoBehaviour
{
    protected GameObject weaponInstance;
    protected GameObject secondWeaponInstance;
    protected GameObject attractBubbleInstance;

    protected GameObject hpSliderInstance;
    protected bool isRanged;
    protected bool isRanged2;
    protected bool slowing;
    protected bool burning;

    public GameObject equip;
    public GameObject secondEquip;
    public GameObject itemToDrop;
    public GameObject HP_slider;
    public GameObject attractBubble;

    public Vector3 targetVector;
    protected bool shouldTargetUpdate = true;
    public float max_health = 5;
    public float healthPoints = 5;
    public float moveSpeed;
    public float attackAnimationDelay = 0.3f;
    public float attackSpeed = .5f;
    public float DEFAULT_SPEED = 1.0f;
    public float slowLast;

    
    private int lastShotTime = 0;
    
    Color originalColor = new Color(1, 1, 1, 1);

    private void Start()
    {
        moveSpeed = DEFAULT_SPEED;
        slowing = false;
        slowLast = 0f;

        createWeaponInstance();

        if (gameObject.layer == 10) //check if enemy to spawn health slider
        {
            createSliderInstance();
        }
    }

    public virtual void setTargetVector(Vector3 v)
    {
        //targetVector = v ;
        targetVector = v - transform.position;
    }

    void Update()
    {
        if (shouldTargetUpdate)
        {
            setTargetVector(GameObject.FindObjectOfType<PlayerEntity>().gameObject.transform.position);

        }
    }

    public void doAttack(Vector3 v)
    {
        //default aim at player
        weaponInstance.GetComponent<AttackType>().attack(v, equip);
    }

    public void doSecondAttack(Vector3 v)
    {
        //default aim at player
        secondWeaponInstance.GetComponent<AttackType>().attack(v, secondEquip);
    }

    public void createWeaponInstance()
    {
        if (equip != null)
        {
            if(GetComponent<PlayerMovement2>())
                GetComponent<PlayerMovement2>().acting = false;
            Destroy(weaponInstance);
            if (equip.CompareTag("Melee") || equip.CompareTag("Slash"))
            {
                
                isRanged = false;
                weaponInstance = Instantiate(equip, transform.position, Quaternion.identity);
                weaponInstance.GetComponent<Equip>().setParentObject(this.gameObject);
                weaponInstance.transform.SetParent(transform);
                if(gameObject.GetComponent<PlayerEntity>() != null)
                    weaponInstance.layer = 8;
            }
            else
            {
                isRanged = true;
                GameObject emptyObj = new GameObject();
                emptyObj.AddComponent<RangeAttack>();

                weaponInstance = Instantiate(emptyObj, transform.position, Quaternion.identity);
                weaponInstance.transform.SetParent(transform);
                //weaponInstance.SetActive(false);
            }
            if (gameObject.layer == 9)
                weaponInstance.layer = 8;
            else if (gameObject.layer == 10)
                weaponInstance.layer = 11;
        }

        if (secondEquip != null)
        {
            if (GetComponent<PlayerMovement2>())
                GetComponent<PlayerMovement2>().acting = false;
            Destroy(secondWeaponInstance);
            if (secondEquip.CompareTag("Melee") || secondEquip.CompareTag("Slash"))
            {
                isRanged2 = false;
                secondWeaponInstance = Instantiate(secondEquip, transform.position, Quaternion.identity);
                secondWeaponInstance.GetComponent<Equip>().setParentObject(this.gameObject);
                secondWeaponInstance.transform.SetParent(transform);
            }
            else
            {
                isRanged2 = true;
                GameObject emptyObj = new GameObject();
                emptyObj.AddComponent<RangeAttack>();

                secondWeaponInstance = Instantiate(emptyObj, transform.position, Quaternion.identity);
                secondWeaponInstance.transform.SetParent(transform);
                //weaponInstance.SetActive(false);
            }
            if (gameObject.layer == 9)
                secondWeaponInstance.layer = 8;
            else if (gameObject.layer == 10)
                secondWeaponInstance.layer = 11;
        }
    }

    protected void createSliderInstance()
    {
        hpSliderInstance = Instantiate(HP_slider, FindObjectOfType<Canvas>().transform.position, Quaternion.identity);
        hpSliderInstance.GetComponent<EnemyHealth>().enemy = gameObject;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        //if the other is a projectile
        hitProjectile(other);

        //TODO drop feedback
        //if the other is a Drop
        //hitDrop(other);

        checkHealth();
    }

    public void hitProjectile(Collider2D other)
    {
        Equip e = other.gameObject.GetComponent<Equip>();
        if (e)
        {
            if (e.CompareTag("Projectile"))
            {
                healthPoints -= e.damage;
                if(other.gameObject.GetComponent<Projectile>().dropped == false)
                {
                    other.gameObject.GetComponent<Projectile>().dropped = true;
                    other.gameObject.GetComponent<Projectile>().spawn_drops();
                }
                Destroy(other.gameObject);
                onDamage();
            }
            else if (e.CompareTag("Slash"))
            {
                healthPoints -= e.damage;
                onDamage();
            }
            applyStatus(e);
        }
    }

    public virtual void onDamage()
    {
    }

    public void applyStatus(Equip e)
    {
        //effect if-statements
        if (e.slow)
        {
            gameObject.GetComponent<Renderer>().material.color = Color.grey;
            slowing = true;
            slowLast = slowLast + 1.5f;
            //turns on slow notifier
            if (CompareTag("Player"))
                FindObjectOfType<SlowIndicator>().indicate();
            isSlowed();
            Invoke("notSlowed", slowLast);
        }
        if (e.burn)
        {
            if (!burning)
            {
                burning = true;
                Color orange = new Color(1f, 0.5f, 0f, 1f);
                gameObject.GetComponent<Renderer>().material.color = orange;
                //turns on UI burned notifier
                if (CompareTag("Player"))
                    FindObjectOfType<BurnIndicator>().indicate();
                Invoke("isBurned", 0.5f);
            }
        }
        if(!e.slow&&!e.burn)
        {
            //FLASH BLANK IF NO STATUS APPLIED
            gameObject.GetComponent<Renderer>().material.color = Color.clear;
            Invoke("revertColor", .1f);
        }
    }

    

    public void revertColor()
    {
        gameObject.GetComponent<Renderer>().material.color = originalColor;
    }

    public virtual void checkHealth() {
        if(gameObject.layer == 10)
            hpSliderInstance.GetComponent<EnemyHealth>().UpdateHealth();
        if (healthPoints <= 0)
        {
            onDeath();
        }
    }

    public virtual void onDeath()
    {
        dropDrop();
        Destroy(gameObject);
    }

    public void dropDrop()
    {
        if (itemToDrop != null)
        {
            GameObject dp = Instantiate(itemToDrop, transform.position, Quaternion.identity);

            //dp.GetComponent<SpriteRenderer>().sprite = itemToDrop.GetComponent<SpriteRenderer>().sprite;
            //dp.GetComponent<Drop>().setItem(itemToDrop);

            /*
            //maybe add isRanged to non player entities later on
            if (equip.gameObject.tag == "Weapon")
                dp.GetComponent<Drop>().setIsRanged(false);
            else if (equip.gameObject.tag == "Projectile")
                dp.GetComponent<Drop>().setIsRanged(true);
                */
        }
        /*else if (projectile != null && weapon == null)
        {
            debug.Log("Dropping " + projectile);
            GameObject dp = Instantiate(dropContainer, transform.position, Quaternion.identity);
            dp.GetComponent<SpriteRenderer>().sprite = projectile.GetComponent<SpriteRenderer>().sprite;
            dp.GetComponent<Drop>().setEquip(this.projectile);
            dp.GetComponent<Drop>().setIsRanged(true);
        }*/
        else
        {
            Debug.Log("Nother to drop!");
        }

    }

    //status
    // deal the burning damage
    protected void isBurned()
    {
        healthPoints -= 1;
        checkHealth();
        float ran = Random.Range(0f, 1f);
        if (ran <= 0.45f)
        {   
            //flash clear
            gameObject.GetComponent<Renderer>().material.color = Color.clear;
            //Turns off UI burned notifier
            if (CompareTag("Player"))
                FindObjectOfType<BurnIndicator>().deindicate();
            Invoke("revertColor", .1f);
            burning = false;
        }
        else
        {
            Invoke("isBurned", 1f);
        }
    }

    // moveSpeed becomes half when slowing
    protected void isSlowed()
    {
        if (slowing == true)
        {
            moveSpeed = 0.5f * moveSpeed;
            gameObject.GetComponent<Renderer>().material.color = Color.gray;
        }
    }

    // reset the moveSpeed
    protected void notSlowed()
    {
        moveSpeed = DEFAULT_SPEED;
        Invoke("revertColor", .1f);
        //turns off slowed notifier
        if (CompareTag("Player"))
            FindObjectOfType<SlowIndicator>().deindicate();
        slowing = false;
    }

    public void spawnAttractBubble()
    {
        //Debug.Log("Spawnbubble");
        if (attractBubble.GetComponent<AttractBubble>() != null)
        {
            
            attractBubbleInstance = Instantiate(attractBubble, gameObject.transform.position, Quaternion.identity);
            attractBubbleInstance.GetComponent<AttractBubble>().setGameObjectToFollow(gameObject);
            attractBubbleInstance.GetComponent<AttractBubble>().startFollowing();
            Debug.Log(attractBubbleInstance.GetComponent<AttractBubble>().getRadius());
        }
        else
        {
            Debug.Log(gameObject + " could not spawn " + attractBubble + ": has no AttractBubble class");
        }
    }

    public bool getIsRanged()
    {
        return isRanged;
    }

    public bool getIsRanged2()
    {
        return isRanged2;
    }

    public int getLastShotTime()
    {
        return lastShotTime;
    }

    public GameObject getWeaponInstance()
    {
        return weaponInstance;
    }

    public GameObject getSecondWeaponInstance()
    {
        return secondWeaponInstance;
    }

    public void shouldTargetVectorUpdate(bool b)
    {
        shouldTargetUpdate = b;
    }
}
