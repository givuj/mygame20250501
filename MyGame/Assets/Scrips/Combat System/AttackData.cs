using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[CreateAssetMenu(menuName ="Combat System/Create a new attack")]
public class AttackData : ScriptableObject//***
{
    [field: SerializeField] public string AnimName { get; private set; }
    [field: SerializeField] public AttackHitbox HitboxToUse { get; private set; }
    [field: SerializeField] public float ImpactStartime { get; private set; }
    [field: SerializeField] public float ImpactEndtime { get; private set; }


}
public enum AttackHitbox {RightFoot,LeftHand }
