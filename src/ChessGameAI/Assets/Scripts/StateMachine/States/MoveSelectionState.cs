using System.Threading.Tasks;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveSelectionState : State
{
    public override async void EnterAsync()
    {
        //Debug.Log("Move Selection State.");
        var movements = Board.Instance.SelectedPiece.Movement.GetValidMoves();
        Highlight.Instance.SelectTiles(movements);
        InputController.Instance.TileClicked += OnHighlightClicked;        
        InputController.Instance.ReturnClicked += ReturnClicked;
        await Task.Delay(100);
    }

    public override void Exit()
    {
        Highlight.Instance.DeSelectTiles();
        InputController.Instance.TileClicked -= OnHighlightClicked;
        InputController.Instance.ReturnClicked -= ReturnClicked;
    }

    private void ReturnClicked(object sender, object args)
    {
        Machine.ChangeTo<PieceSelectionState>();
    }

    private void OnHighlightClicked(object sender, object args)
    {
        var highlight = sender as HighlightClick;
        if (highlight == null)
        {
            return;
        }        
        Board.Instance.SelectedMove = highlight.AvailableMove;
        Machine.ChangeTo<PieceMovementState>();

    }
}
