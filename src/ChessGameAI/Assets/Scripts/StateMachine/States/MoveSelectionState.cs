using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveSelectionState : State
{
    public override void Enter()
    {
        Debug.Log("Move Selection State.");
        var movements = Board._instance.SelectedPiece.Movement.GetValidMoves();
        foreach (var tile in movements)
        {
            Debug.Log(tile.pos);
        }
    }
}
