using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CloneEntity : PlayerEntity
{
    public void setColor()
    {
        Color slime_green = new Color(.557f, .792f, .322f, 1f);
        Renderer rend = GetComponent<Renderer>();
        rend.material.color = slime_green;
    }
        
    
    // Update is called once per frame
    void Update()
    {
        //Debug.Log(targetVector);
        //setTargetVector(Input.mousePosition);

    }

    public override void onDamage()
    {
        Debug.Log("hit clone");
    }

    public override void checkHealth()
    {
        if (healthPoints <= 0)
        {
            onDeath();
        }
    }

    public override void onDeath()
    {
        Debug.Log("clone died");
        dropDrop();
        Destroy(gameObject);
    }
}
