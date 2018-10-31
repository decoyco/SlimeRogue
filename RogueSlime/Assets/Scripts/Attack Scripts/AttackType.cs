using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface AttackType {
    void attack(Vector3 v, GameObject e);
    void attack(GameObject t, GameObject e);
}
