using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Reflection;

public class CopyController : MonoBehaviour
{
    private PlayerEntity player;
    private GameObject right_click;
    private GameObject enemy;
    private float slider_value;
    private float increment = .005f;
    private float moveSpeed;
    private GameObject equip;

    public float recorded_value;

    // Use this for initialization
    void Start()
    {
        right_click = FindObjectOfType<RightClickSlider>().gameObject;
        player = GetComponent<PlayerEntity>();
        enemy = null;
    }

    // Update is called once per frame
    void Update()
    {
        slider_value = right_click.GetComponent<RightClickSlider>().slider.value;
        EmptyFill();
        
    }

    //FILLS SLIDER AND RETURNS SLIDER PROGRESS UPON RELEASE
    void EmptyFill()
    {

        if (Input.GetMouseButton(1) && Input.GetMouseButton(0))
        {
            if (enemy == null && right_click.GetComponent<RightClickSlider>().enemy.GetComponent<PlayerEntity>() == null)
                enemy = right_click.GetComponent<RightClickSlider>().enemy;
            else if (enemy != null)
            {
                right_click.transform.position = Camera.main.WorldToScreenPoint(enemy.transform.position);
                Vector3 new_position = (enemy.transform.position - right_click.transform.position);
                right_click.GetComponent<RightClickSlider>().circle.offset = new_position;
                right_click.GetComponent<RightClickSlider>().updateValue(slider_value + increment);
            }
        }
        else
        {
            setSliderPos2Mouse();
        }
        if (Input.GetMouseButtonUp(1) || Input.GetMouseButtonUp(0))
        {
            recorded_value = slider_value;
            right_click.GetComponent<RightClickSlider>().setZero();
            if (recorded_value == 1)
            {
                equip = enemy.GetComponent<Entity>().equip;
                player.GetComponent<PlayerEntity>().secondEquip = equip;
                player.GetComponent<PlayerEntity>().createWeaponInstance();
                FindObjectOfType<SkillIndicator>().change_to(gameObject.GetComponent<Entity>().secondEquip.gameObject.GetComponent<SpriteRenderer>().sprite);
                recorded_value = 0;
                enemy = null;
            }
            else
            {
                setSliderPos2Mouse();
                enemy = null;
            }
        }
    }

    //MOVES SLIDER TO MOUSE POSITION
    void setSliderPos2Mouse()
    {
        right_click.transform.position = Input.mousePosition;
        Vector3 new_position = Camera.main.ScreenToWorldPoint(right_click.transform.position) - right_click.transform.position;
        right_click.GetComponent<RightClickSlider>().circle.offset = new_position;
    }
}
