using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class State<T>:MonoBehaviour
{
    // Start is called before the first frame update
    public virtual void Enter(T owner) { }
    public virtual void Execute() { }
    public virtual void Exit() { }
}
