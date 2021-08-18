using System.Threading.Tasks;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StateMachineController : MonoBehaviour
{
    public static StateMachineController Instance;
    public Player Player1;
    public Player Player2;
    public Player CurrentlyPlaying;
    public TaskCompletionSource<object> TaskHold;
    public GameObject PromotionPanel;
    private State _current;
    private bool _busy;
    
    private void Awake(){
        Instance = this;
    }

    private void Start(){
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
            _current.EnterAsync();

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
