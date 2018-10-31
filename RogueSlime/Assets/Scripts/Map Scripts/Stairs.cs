using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Stairs : Doors
{
    protected static bool stairsLocked = true;


    private void OnTriggerEnter2D(Collider2D trigger)
    {
        PlayerEntity player = trigger.GetComponent<PlayerEntity>();
        if (player && !locked)
        {
            LevelManager level_manager = FindObjectOfType<LevelManager>();
            level_manager.LoadLevel("You Win");
        }
    }

    protected override void changeSprite()
    {
        if (locked)
            gameObject.GetComponent<SpriteRenderer>().sprite = Resources.Load("cavedoor_closed", typeof(Sprite)) as Sprite;
        else
            gameObject.GetComponent<SpriteRenderer>().sprite = Resources.Load("cavedoor_nextlvl", typeof(Sprite)) as Sprite;
    }
    /*
    protected override void checkMobs()
    {
        if (mobs)
        {
            if (mobs.spawned)
            {
                if (!mobs.EnemiesAreDead())
                    changeSprite();
                    stairsLocked = true;
                if (mobs.EnemiesAreDead())
                {
                    Destroy(mobs.gameObject);
                    changeSprite();
                    stairsLocked = false;
                }
            }
        }
    }
    */

}
