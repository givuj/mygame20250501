using System.Collections;
using System.Collections.Generic;
using UnityEngine;
//���˿�������ȥ׷���뿪�������߲�׷
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
