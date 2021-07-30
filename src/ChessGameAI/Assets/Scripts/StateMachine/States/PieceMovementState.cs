using System.Threading.Tasks;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PieceMovementState : State
{
    public override async void EnterAsync()
    {
        Debug.Log("Piece Movement State.");
        var piece = Board._instance.SelectedPiece;
        piece.transform.position = Board._instance.SelectedHighlight.transform.position;
        piece.tile.content = null;
        piece.tile = Board._instance.SelectedHighlight.Tile;
        if (piece.tile.content != null)
        {
            var deadPiece = piece.tile.content;
            Debug.LogFormat("Peça {0} foi capturada", deadPiece.transform);
            deadPiece.gameObject.SetActive(false);
        }
        piece.tile.content = piece;
        await Task.Delay(100);
        Machine.ChangeTo<TurnEndState>();
    }
   
}
