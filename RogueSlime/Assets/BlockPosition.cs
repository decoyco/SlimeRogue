using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlockPosition : MonoBehaviour {
    public GameObject block;
    public bool born = false;
    float width = .11f;
    float height = .11f;
    // Use this for initialization
    private void OnDrawGizmos()
    {
        Gizmos.DrawWireCube(transform.position, new Vector3(width, height));
    }
}

