using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PieceSelectionState : State
{
    public override void EnterAsync()
    {
        SetColliders(true);
        InputController.Instance.TileClicked += PieceClicked;
    }

    public override void Exit()
    {
        SetColliders(false);
        InputController.Instance.TileClicked -= PieceClicked;
    }

    private void SetColliders(bool state)
    {
        foreach (var boxCollider2D in Machine.CurrentlyPlaying.GetComponentsInChildren<BoxCollider2D>())
        {
            boxCollider2D.enabled = state;
        }
    }

    private void PieceClicked(object sender, object args)
    {
        var piece = sender as Piece;
        var player = args as Player;
        if (Machine.CurrentlyPlaying == player)
        {
            Debug.Log(piece + " was clicked.");
            Board.Instance.SelectedPiece = piece;
            Machine.ChangeTo<MoveSelectionState>();            
        }
        
    }
}
