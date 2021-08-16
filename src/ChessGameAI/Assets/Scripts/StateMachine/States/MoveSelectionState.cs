using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveSelectionState : State
{
    public override void EnterAsync()
    {
        Debug.Log("Move Selection State.");
        var movements = Board._instance.SelectedPiece.Movement.GetValidMoves();
        Highlight.Instance.SelectTiles(movements);
        InputController.Instance.TileClicked += OnHighlightClicked;        
        InputController.Instance.ReturnClicked += ReturnClicked;
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
        var v3Pos = highlight.transform.position;
        var pos = new Vector2Int((int)v3Pos.x, (int)v3Pos.y);
        Tile tileClicked = highlight.Tile;
        Debug.Log(tileClicked.pos);
        Board._instance.SelectedHighlight = highlight;
        Machine.ChangeTo<PieceMovementState>();

    }
}
