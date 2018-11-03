using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttractBubble : MonoBehaviour {

    public bool isEnabled;

	// Use this for initialization
	void Start () {
        isEnabled = false;
	}

    private void OnTriggerEnter2D(Collider2D other)
    {
        //if the other is a Drop
        hitDrop(other);
    }

    public void hitDrop(Collider2D other)
    {
        ItemAbstract item = other.gameObject.GetComponent<ItemAbstract>();
        if (item != null)
        {
            item.moveTo(gameObject);
        }
    }

    /*
	// Update is called once per frame
	void Update () {
		
	}
    */

}
