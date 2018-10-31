using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerHealth : MonoBehaviour {
    private PlayerEntity player;
    public Slider health_slider;
	// Use this for initialization
	void Start () {
        health_slider.minValue = 0;
        health_slider.maxValue = 100f;
    }
	
	// Update is called once per frame
	void Update () {
        if(player == null)
            player = FindObjectOfType<PlayerEntity>();
        health_slider.value = (float)player.healthPoints/player.max_health * 100f;
	}
}
