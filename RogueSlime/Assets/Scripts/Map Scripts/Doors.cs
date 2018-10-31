using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Doors : MonoBehaviour
{
    protected MoveCamera MainCamera;
    protected bool locked = false;
    public EnemySpawner mobs;
    public float move_unit = 1f;

    private PlayerEntity player;
    private float top, bottom, leftmost, rightmost;
    // Use this for initialization
    private void Awake()
    {
       locked = false;
    }
    void Start()
    {
        MainCamera = GameObject.FindObjectOfType<MoveCamera>();
        player = FindObjectOfType<PlayerEntity>();
        /*
        top = mobs.transform.position.y + 0.6f;
        bottom = mobs.transform.position.y - 0.6f;
        leftmost = mobs.transform.position.x - 0.6f;
        rightmost = mobs.transform.position.x + 0.6f;
        */
        locked = false;
        changeSprite();
    }

    /*------------------------------------------------inefficient---------------------------------------------
    void Update()
    {
        if (player.transform.position.y < top && player.transform.position.y > bottom && player.transform.position.x > leftmost && player.transform.position.x < rightmost && mobs != null)
        {
            checkMobs();
        }
        else if(mobs)
            locked = true;
        changeSprite();
    }
    */

    protected virtual void checkMobs()
    {
        if (mobs != null)
        {
            if (mobs.spawned)
            {
                if (!mobs.EnemiesAreDead())
                    locked = true;
                if (mobs.EnemiesAreDead())
                {
                    Destroy(mobs.gameObject);
                    locked = false;
                }
            }
        }
        else
            locked = false;
    }

    protected virtual void changeSprite()
    {
        if (locked)
            gameObject.GetComponent<SpriteRenderer>().sprite = Resources.Load("cavedoor_closed", typeof(Sprite)) as Sprite;
        else
            gameObject.GetComponent<SpriteRenderer>().sprite = Resources.Load("cavedoor_open", typeof(Sprite)) as Sprite;
    }

    private void OnTriggerEnter2D(Collider2D trigger)
    {
        PlayerMovement2 p = trigger.GetComponent<PlayerMovement2>();
        if (p && !locked)
        {
            //Moves Camera and player on entering door
            if (transform.rotation.z == -0.7071068f)
            {
                MainCamera.moveRight();
                trigger.transform.position = new Vector3 (trigger.transform.position.x + move_unit,
                                                          trigger.transform.position.y,
                                                          trigger.transform.position.z);
            }
            if (transform.rotation.z == 0)
            {
                MainCamera.moveUp();
                trigger.transform.position = new Vector3(trigger.transform.position.x,
                                                          trigger.transform.position.y + move_unit,
                                                          trigger.transform.position.z);
            }
            if (transform.rotation.z == 1)
            {
                MainCamera.moveDown();
                trigger.transform.position = new Vector3(trigger.transform.position.x,
                                                          trigger.transform.position.y - move_unit,
                                                          trigger.transform.position.z);
            }
            if (transform.rotation.z == .7071068f)
            {
                MainCamera.moveLeft();
                trigger.transform.position = new Vector3(trigger.transform.position.x - move_unit,
                                                          trigger.transform.position.y,
                                                          trigger.transform.position.z);
            }
            
             locked = true;
             Invoke("unlockDoors", 0.3f);
        }
    }

    public void unlockDoors()
    {
        locked = false;
        changeSprite();
    }

    public void lockDoors()
    {
        locked = true;
        changeSprite();
    }
    //teleports character to destination

}
