using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public delegate void TileClickedEvent(object sender, object args);
public class InputController : MonoBehaviour
{
    public static InputController Instance;
    public TileClickedEvent TileClicked = delegate{};
    public TileClickedEvent ReturnClicked = delegate{};
    
    private void Awake(){

        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else if (Instance != this)
        {
            Destroy(gameObject);
        }
        
    }

    private void Update(){
        
        if (Input.GetButtonDown("Cancel"))
        {
            ReturnClicked(null, null);
        }
    }

    public void Promotion(string piece){

        StateMachineController.Instance.TaskHold.SetResult(piece);
    }
}
