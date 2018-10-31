using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAnimationController : MonoBehaviour {
    public Animator anim;
    public bool attacking;
    // Use this for initialization
    void Start () {
        anim = GetComponent<Animator>();
        attacking = false;
    }

    public void Idle(Vector3 v)
    {
        if (v.x < 0 && v.y < 0)
        {
            anim.Play("Idle00");
        }
        if (v.x < 0 && v.y > 0)
        {
            anim.Play("Idle01");
        }
        if (v.x > 0 && v.y < 0)
        {
            anim.Play("Idle10");
        }
        if (v.x > 0 && v.y > 0)
        {
            anim.Play("Idle11");
        }
    }

    public void Walk(Vector3 v)
    {
        if (v.x < 0 && v.y < 0)
        {
            anim.Play("Walk00");
        }
        if (v.x < 0 && v.y > 0)
        {
            anim.Play("Walk01");
        }
        if (v.x > 0 && v.y < 0)
        {
            anim.Play("Walk10");
        }
        if (v.x > 0 && v.y > 0)
        {
            anim.Play("Walk11");
        }
    }

    public void Attack(Vector3 v)
    {
        if (v.x < 0 && v.y < 0)
        {
            anim.Play("Attack00");
        }
        if (v.x < 0 && v.y > 0)
        {
            anim.Play("Attack01");
        }
        if (v.x > 0 && v.y < 0)
        {
            anim.Play("Attack10");
        }
        if (v.x > 0 && v.y > 0)
        {
            anim.Play("Attack11");
        }
    }
}
