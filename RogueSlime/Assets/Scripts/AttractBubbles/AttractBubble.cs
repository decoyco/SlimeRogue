using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//TODO make into interface

public class AttractBubble : MonoBehaviour {

    public bool isEnabled;

    private float defaultSize;
    private GameObject gameObjectToFollow;
    private bool isFollowing;

    // Use this for initialization
    void Start () {
        defaultSize = gameObject.GetComponent<CircleCollider2D>().radius;
    }

    // Called once every frame
    void Update()
    {
        if (isFollowing && gameObjectToFollow != null)
        {
            gameObject.transform.position = gameObjectToFollow.transform.position;
        }
    }

    /**
     * If item/drop in circle, initiates movement to center.
     * Speed/Acceleration set in ItemAbstract
     */
    private void OnTriggerStay2D(Collider2D other)
    {
        //if the other is a Drop
        hitDrop(other);
    }

    /**
     * If item/drop leaves circle.
     */
    private void OnTriggerExit2D(Collider2D other)
    {
        //Debug.Log(other + "exited attract");
        //if the other is a Drop
        exitDrop(other);
    }

    /**
     * Is other an Item/Drop check
     * NOTE: move speed is in ItemAbstract.cs May need to move here instead.
     */
    public void hitDrop(Collider2D other)
    {
        ItemAbstract item = other.gameObject.GetComponent<ItemAbstract>();
        
        if (item != null && isEnabled && item.isAttractable)
        {
            item.moveTo(gameObject);
        }
    }

    /**
     * Is other an Item/Drop check
     */
    public void exitDrop(Collider2D other)
    {
        ItemAbstract item = other.gameObject.GetComponent<ItemAbstract>();
        if (item != null && isEnabled && item.isAttractable)
        {
            item.stopMoveTo();
        }
    }

    public void setGameObjectToFollow(GameObject obj)
    {
        gameObjectToFollow = obj;
    }

    public void startFollowing()
    {
        isFollowing = true;
    }

    public void stopFollowing()
    {
        isFollowing = false;
    }

    /**
     * Expands the bubble to a specified size (large for full room)
     */
    public void setRadius(float s)
    {
        gameObject.GetComponent<CircleCollider2D>().radius = s;
    }

    /**
     * Gets the bubble's radius 
     */
    public float getRadius()
    {
        return gameObject.GetComponent<CircleCollider2D>().radius;
    }

}
