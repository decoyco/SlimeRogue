using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
//THIS CLASS HANDLES RIGHT CLICK SLIDER VISUALS AND ALSO STORES VALUE OF PREGRESS PERCENTAGE
public class RightClickSlider : MonoBehaviour {
    public CircleCollider2D circle;
    public Slider slider;
    public GameObject enemy;
    

	void Start () {
        slider.value = 0;
        slider.enabled = false;
	}
	
	// Update is called once per frame
	void Update () {
        /*
        transform.position = Input.mousePosition;
        Vector3 new_position = Camera.main.ScreenToWorldPoint(transform.position) - transform.position;
        circle.offset = new_position;
        if(Input.GetMouseButton(1) && active)
        {
            if (slider.value == 1)
                slider.value = 0;
            slider.value += increment;
        }
        if (Input.GetMouseButtonUp(1))
        {
            recorded_value = slider.value;
            slider.value = 0;
        }
        */
    }

    private void OnTriggerStay2D(Collider2D col)
    {
        if(col.gameObject.GetComponent<Entity>() != null)
            enemy = col.gameObject;  
    }

    private void OnTriggerExit2D(Collider2D col)
    {
        enemy = null;
    }

    public void setZero()
    {
        slider.value = 0;
    }

    public void updateValue(float v)
    {
        slider.value = v;
        Debug.Log(slider.value);
    }
}
