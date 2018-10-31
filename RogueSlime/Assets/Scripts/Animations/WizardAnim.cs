using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WizardAnim : MonoBehaviour {
    private PlayerEntity player;
    private Vector3 slimePos;
    private float x;
    private float y;

	// Use this for initialization
	void Start () {
        player = GameObject.FindObjectOfType<PlayerEntity>();
	}
	
	// Update is called once per frame
	void Update () {
        slimePos = player.transform.position;
        x = this.transform.position.x - slimePos.x;
        y = this.transform.position.y - slimePos.y;
        InvokeRepeating("FaceDirection", 0f, 0.5f);
    }

    void FaceDirection(){
        if(x > 0 && y > 0){
            gameObject.GetComponent<SpriteRenderer>().sprite = Resources.Load("W3", typeof(Sprite)) as Sprite;
        }
        else if (x > 0 && y < 0)
        {
            gameObject.GetComponent<SpriteRenderer>().sprite = Resources.Load("W2", typeof(Sprite)) as Sprite;
        }
        else if (x < 0 && y < 0)
        {
            gameObject.GetComponent<SpriteRenderer>().sprite = Resources.Load("W1", typeof(Sprite)) as Sprite;
        }
        else if (x < 0 && y > 0)
        {
            gameObject.GetComponent<SpriteRenderer>().sprite = Resources.Load("W4", typeof(Sprite)) as Sprite;
        }
    }
}
