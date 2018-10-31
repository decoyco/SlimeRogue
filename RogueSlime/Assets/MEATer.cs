using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MEATer : MonoBehaviour {
    public Slider slider;
    private PlayerEntity player;
	// Use this for initialization
	void Start () {
        slider.maxValue = 5;
	}

    void Update()
    {
        if(player == null)
            player = FindObjectOfType<PlayerEntity>();
        slider.value = player.GetComponent<PlayerEntity>().numMeat;
    }
}
