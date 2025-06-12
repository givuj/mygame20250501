using System.Collections;
using System.Collections.Generic;
using UnityEngine;
//敌人看到主角去追，离开敌人视线不追
public class VisionSensor : MonoBehaviour
{
    // Start is called before the first frame update
    [SerializeField] EnemyController enemy;
    private void OnTriggerEnter(Collider other)
    {
        var fighter = other.GetComponent<MeleeFighter>();
        if(fighter!=null)
        {
            enemy.TargetsInRange.Add(fighter);
        }
    }
    private void OnTriggerExit(Collider other)
    {
        var fighter = other.GetComponent<MeleeFighter>();
        if (fighter != null)
        {
            enemy.TargetsInRange.Remove(fighter);
        }

    }

}
