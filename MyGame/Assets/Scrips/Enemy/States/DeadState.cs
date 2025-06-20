using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeadState : State<EnemyController>
{
    // Start is called before the first frame update
    public override void Enter(EnemyController owner)
    {
        owner.visionSensor.gameObject.SetActive(false);
        EnemyManager.i.RemoveEnemyInRange(owner);
        owner.NavAgent.enabled = false;
        owner.CharacterController.enabled = false;
    }
}
