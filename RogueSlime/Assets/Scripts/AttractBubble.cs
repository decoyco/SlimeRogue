using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttractBubble : MonoBehaviour {

    public bool isEnabled;
    private GameObject gameObjectToFollow;
    private bool isFollowing;

	// Use this for initialization
	void Start () {
        isEnabled = false;
	}

    // Update is called once per frame
    void Update()
    {
        if (isFollowing && gameObjectToFollow != null)
        {
            gameObject.transform.position = gameObjectToFollow.transform.position;
        }
    }

    private void OnTriggerStay2D(Collider2D other)
    {
        //if the other is a Drop
        hitDrop(other);
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        Debug.Log(other + "exited attract");
        //if the other is a Drop
        exitDrop(other);
    }

    public void hitDrop(Collider2D other)
    {
        ItemAbstract item = other.gameObject.GetComponent<ItemAbstract>();
        if (item != null)
        {
            item.moveTo(gameObject);
        }
    }

    public void exitDrop(Collider2D other)
    {
        ItemAbstract item = other.gameObject.GetComponent<ItemAbstract>();
        if (item != null)
        {
            item.stopMoveTo();
        }
    }

    public void setGameObjectToFollow(GameObject obj)
    {
        gameObjectToFollow = obj;
        isFollowing = true;
    }

    public void stopFollowing()
    {
        isFollowing = false;
        gameObjectToFollow = null;
    }

}
