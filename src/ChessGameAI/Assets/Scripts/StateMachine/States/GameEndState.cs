using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameEndState : State
{
    public override void EnterAsync()
    {
        Debug.Log("Ended");
        LevelController.Instance.GameOver();
    }
}
