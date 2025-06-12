using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StateMachine<T>
{
    // Start is called before the first frame update
   public State<T> CurrenState { get; private set; }
   T _owner;
   public StateMachine(T owner)
   {
        _owner = owner;
   }
   public void ChangeState(State<T> newState)//但切换到其他状态时会触发先退出当前状态在切换到其他状态
   {
        CurrenState?.Exit();
        CurrenState = newState;
        CurrenState.Enter(_owner);
   }
    public void Execute()
    {
        CurrenState?.Execute();
    }
}
