using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class PlayerCooldownSlider : MonoBehaviour {
    public GameObject player;
    public Slider cd_slider;
    bool active = false;
    //float cool_down = 0f;

    private void Start()
    {
        cd_slider.minValue = 0f;
        cd_slider.maxValue = 100f;
        cd_slider.value = 100f;
        cd_slider.transform.parent = FindObjectOfType<Canvas>().transform;

    }

    public void activate(float cd)
    {
        active = true;
        cd_slider.maxValue = cd;
        cd_slider.value = 0f;
        InvokeRepeating("increment", 0f, cd*.01f);
        Invoke("deactivate", cd);
    }

    void increment()
    {
        cd_slider.value += cd_slider.maxValue / 100;
    }

    public void deactivate()
    {
        cd_slider.maxValue = 100f;
        cd_slider.value = 100f;
        active = false;
        CancelInvoke();
    }
}
