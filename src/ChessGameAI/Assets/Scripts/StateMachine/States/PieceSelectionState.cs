using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PieceSelectionState : State
{
    public override void EnterAsync()
    {
        Board._instance.TileClicked += PieceClicked;
    }

    public override void Exit()
    {
        Board._instance.TileClicked -= PieceClicked;
    }

    private void PieceClicked(object sender, object args)
    {
        var piece = sender as Piece;
        var player = args as Player;
        if (Machine.CurrentlyPlaying == player)
        {
            Debug.Log(piece + " was clicked.");
            Board._instance.SelectedPiece = piece;
            Machine.ChangeTo<MoveSelectionState>();            
        }
        
    }
}
