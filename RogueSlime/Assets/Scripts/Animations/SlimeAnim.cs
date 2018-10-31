using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SlimeAnim : MonoBehaviour
{
    public Animator anim;

    // Use this for initialization
    void Start()
    {
        anim = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 v = Input.mousePosition;
        v.x = v.x - Camera.main.WorldToScreenPoint(transform.position).x;
        v.y = v.y - Camera.main.WorldToScreenPoint(transform.position).y;

        if (Input.GetMouseButton(0))
        {
            if (v.x > 0 && v.y > 0)
            {
                anim.Play("Attack11");
            }
            else if (v.x < 0 && v.y > 0)
            {
                anim.Play("Attack01");
            }
            else if (v.x < 0 && v.y < 0)
            {
                anim.Play("Attack00");
            }
            else if (v.x > 0 && v.y < 0)
            {
                anim.Play("Attack10");
            }
        }
        else
        {
            if (v.x > 0 && v.y > 0)
            {
                anim.Play("Idle11");
            }
            else if (v.x < 0 && v.y > 0)
            {
                anim.Play("Idle01");
            }
            else if (v.x < 0 && v.y < 0)
            {
                anim.Play("Idle00");
            }
            else if (v.x > 0 && v.y < 0)
            {
                anim.Play("Idle10");
            }
        }
    }
}
