using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SkillIndicator : MonoBehaviour
{
    public Image image;
    public Sprite slimeball;
    // Use this for initialization
    void Start()
    {
        //sets image to slime ball
        image.sprite = slimeball;
        image.color = Color.clear;
    }

    public void change_to(Sprite s)
    {
        image.color = Color.white;
        image.sprite = s;
    }
}
