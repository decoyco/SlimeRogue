using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Slash : Equip {
	// Use this for initialization
	void Start () {
        Invoke("die", 0.1f);
	}
    void die()
    {
        Destroy(gameObject);
    }
}
