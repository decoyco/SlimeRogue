using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Block : MonoBehaviour {
    SpriteRenderer rend;
    public Sprite[] blocks;
	// Use this for initialization
	void Start () {
        rend = GetComponent<SpriteRenderer>();
        int ran = Random.Range(0, blocks.Length);
        rend.sprite = blocks[ran];
	}
}
