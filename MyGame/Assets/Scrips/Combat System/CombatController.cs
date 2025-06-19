using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CombatController : MonoBehaviour
{
    MeleeFighter meleeFight;
    // Start is called before the first frame update
    void Start()
    {
        meleeFight = GetComponent<MeleeFighter>();
    }

    // Update is called once per frame
    void Update()//主角攻击的主入口
    {
        if (Input.GetButtonDown("Attack"))
        {
            meleeFight.TryToAttack();
        }
    }
}
