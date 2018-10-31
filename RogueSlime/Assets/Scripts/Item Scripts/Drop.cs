using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Drop : MonoBehaviour {
    public GameObject item;
    private bool isRanged;

    public void setItem(GameObject s)
    {
        item = s;
    }

    public ItemAbstract getItemToDrop()
    {
        return item.GetComponent<ItemAbstract>();
    }

    public void setIsRanged(bool s)
    {
        isRanged = s;
    }

    public bool getIsRanged()
    {
        return isRanged;
    }
}
