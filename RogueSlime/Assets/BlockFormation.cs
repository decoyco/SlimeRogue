using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlockFormation : MonoBehaviour {
    float width = 1.28f;
    float height = 1.28f;
    // Use this for initialization
    void Start () {
        SpawnBlocks();
	}

    void SpawnBlocks()
    {
        foreach (Transform child in transform)
        {
            if (!child.gameObject.GetComponent<BlockPosition>().born)
            {
                child.gameObject.GetComponent<BlockPosition>().born = true;
                GameObject block = Instantiate(child.gameObject.GetComponent<BlockPosition>().block, child.transform.position, Quaternion.identity) as GameObject;
                block.transform.parent = child;
            }
        }
    }

    void OnDrawGizmos()
    {
        Gizmos.DrawWireCube(transform.position, new Vector3(width, height));
    }
}
