using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Burn : Effector {
    bool onEntity;
    float counter;
	// Use this for initialization
	void Start () {
        onEntity = (gameObject.GetComponent<Entity>() != null);
        type = EffectType.Burn;
        if(onEntity)
            entity = GetComponent<Entity>();
        counter = 0f;
        if (onEntity)
            changeColor(new Color(1,.65f,0,1));
	}

    protected override void DoEffect()
    {
        if(counter <= timeElapsed)
        {
            entity.healthPoints -= 1;
            entity.checkHealth();
            counter += 1f;
            if (Random.Range(0f, 1f) < .4f)
                timeDuration++;
        }
    }

    protected override void RevertEffect()
    {
        changeColor(new Color(1, 1, 1, 1));
    }
}
