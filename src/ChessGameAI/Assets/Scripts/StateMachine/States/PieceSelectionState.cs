using System.Threading.Tasks;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PieceSelectionState : State
{
    public override async void EnterAsync()
    {        
        SetColliders(true);
        InputController.Instance.TileClicked += PieceClicked;
        await Task.Delay(100);
    }

    public override void Exit()
    {
        SetColliders(false);
        InputController.Instance.TileClicked -= PieceClicked;
    }

    private async void SetColliders(bool state)
    {
        foreach (var boxCollider2D in Machine.CurrentlyPlaying.GetComponentsInChildren<BoxCollider2D>())
        {
            boxCollider2D.enabled = state;
        }
        await Task.Delay(100);
    }

    private async void PieceClicked(object sender, object args)
    {
        var piece = sender as Piece;
        var player = args as Player;
        if (Machine.CurrentlyPlaying == player)
        {
            //Debug.Log(piece + " was clicked.");
            Board.Instance.SelectedPiece = piece;
            await Task.Delay(100);
            Machine.ChangeTo<MoveSelectionState>();            
        }
        
    }
}
