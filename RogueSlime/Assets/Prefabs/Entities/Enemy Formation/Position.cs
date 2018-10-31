using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Position : MonoBehaviour {
    public GameObject[] enemy;
    public bool born = false;
    void OnDrawGizmos()
    {
        Gizmos.DrawWireSphere(transform.position, .1f);
    }
}
