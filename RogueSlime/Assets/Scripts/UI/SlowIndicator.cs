using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SlowIndicator : MonoBehaviour {
    private Text text;
	// Use this for initialization
	void Start () {
        text = GetComponent<Text>();
        text.color = Color.clear;
	}

    public void indicate()
    {
        text.color = Color.white;
    }

    public void deindicate()
    {
        text.color = Color.clear;
    }
}
