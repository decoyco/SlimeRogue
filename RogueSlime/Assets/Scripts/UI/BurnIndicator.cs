using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BurnIndicator : MonoBehaviour {
    private Text text;
    Color orange = new Color(1f, 0.5f, 0f, 1f);
    public void Start()
    {
        text = GetComponent<Text>();
        text.color = Color.clear;
    }
    public void indicate()
    {
        text.color = orange;
    }
    public void deindicate()
    {
        text.color = Color.clear;
    }
}
