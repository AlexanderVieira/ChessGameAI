using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StateMachineController : MonoBehaviour
{
    public static StateMachineController _instance;
    State _current;
    bool _busy;
    
    void Awake(){
        _instance = this;
    }

    void Start(){
        ChangeTo<LoadState>();
    }

    public void ChangeTo<T>() where T:State {
        var state = GetState<T>();
        if(_current != state)
            ChangeState(state);
    }

    private void ChangeState(State state)
    {
        if(_busy)
            return;
        _busy = true;
        
        if (_current != null)
            _current.Exit();

        _current = state;
        if(_current != null)
            _current.Enter();

        _busy = false;
    }

    private T GetState<T>() where T : State
    {
        var target = GetComponent<T>();
        if(target == null)
            target = gameObject.AddComponent<T>();
        return target;
    }
}
