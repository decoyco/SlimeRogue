using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EnemyHealth : MonoBehaviour {
    public GameObject enemy;
    public Slider health_slider;
    // Use this for initialization
    void Start () {
        health_slider.minValue = 0;
        health_slider.maxValue = 100f;
        health_slider.value = 100f;
        health_slider.transform.parent = FindObjectOfType<Canvas>().transform;
    }
	
	// Update is called once per frame
	void Update () {
        transform.position = Camera.main.WorldToScreenPoint(enemy.transform.position);
        Vector3 sprite_extents = enemy.GetComponent<SpriteRenderer>().sprite.bounds.extents;
        transform.position = new Vector3(transform.position.x, transform.position.y + sprite_extents.y * 500, 0);
    }

    public void UpdateHealth()
    {
        health_slider.value = (float)enemy.GetComponent<Entity>().healthPoints / enemy.GetComponent<Entity>().max_health * 100f;
        if (health_slider.value == 0)
            Destroy(gameObject);
    }
}
