using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveCamera : MonoBehaviour {
	public float move_unit = 0;
	// Use this for initialization
	void Start () {
		// whichever side the door is on, the camera will move in the same direction

		// for now door will be function [ door() ]
		//moveRight();
		//x
	}
	
	// Update is called once per frame
	void Update () {
		
	}
	public void moveRight()
	{
		Vector3 pos = this.GetComponent<Transform> ().position;
		pos.x += move_unit;
		pos.z = -10;
		this.transform.position = pos;
	}

	public void moveLeft()
	{
		Vector3 pos = this.GetComponent<Transform> ().position;
		pos.x -= move_unit;
		pos.z = -10;
		this.transform.position = pos;
	}

	public void moveUp()
	{
		Vector3 pos = this.GetComponent<Transform> ().position;
		pos.y += move_unit;
		pos.z = -10;
		this.transform.position = pos;
	}

	public void moveDown()
	{
		Vector3 pos = this.GetComponent<Transform> ().position;
		pos.y -= move_unit;
		pos.z = -10;
		this.transform.position = pos;
	}

}