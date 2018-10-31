using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GoblinAnimation : MonoBehaviour {
    public Vector3 relative_position;
    private Animator anim;
    private AttackPatternAbstract pattern;
    // Update is called once per frame
    private void Start()
    {
        anim = GetComponent<Animator>();
        pattern = GetComponent<AttackPatternAbstract>();
    }
    void Update()
    {
        relative_position = GameObject.FindObjectOfType<PlayerEntity>().gameObject.transform.position - transform.position;
        relative_position.Normalize();
        if(!pattern.attacking)
        if (relative_position.x > 0 && relative_position.y > 0)
            anim.Play("Goblin_Walk11");
        if (relative_position.x > 0 && relative_position.y < 0)
            anim.Play("Goblin_Walk10");
        if (relative_position.x < 0 && relative_position.y > 0)
            anim.Play("Goblin_Walk01");
        if (relative_position.x < 0 && relative_position.y < 0)
            anim.Play("Goblin_Walk00");
    }
}
