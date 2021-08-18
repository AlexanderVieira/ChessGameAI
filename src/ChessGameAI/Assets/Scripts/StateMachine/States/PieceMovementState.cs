using System.Net.Sockets;
using System.Net.NetworkInformation;
using System.Threading.Tasks;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class PieceMovementState : State
{
    public override async void EnterAsync()
    {
        Debug.Log("Piece Movement State.");
        var moveType = Board.Instance.SelectedHighlight.Tile.MoveType;
        ClearEnPassant();
        //var tcs = new TaskCompletionSource<bool>();
        switch(moveType){

            case MoveType.Normal:
                NormalMove();
                break;
            case MoveType.Castling:
                Castling();
                break;
            case MoveType.PawnDoubleMove:
                PawnDoubleMove();
                break;
            case MoveType.EnPassant:
                EnPassant();
                break;                
        }
        await Task.Delay(100);
        Machine.ChangeTo<TurnEndState>();
    }

    private void ClearEnPassant()
    {
        ClearEnPassant(5);
        ClearEnPassant(2);
    }

    private void ClearEnPassant(int height)
    {
        var position = new Vector2Int(0, height);
        for (int i = 0; i < 7; i++)
        {
            position.x = position.x + 1;
            Board.Instance.Tiles[position].MoveType = MoveType.Normal;
        }
    }

    private void EnPassant()
    {
        var pawn = Board.Instance.SelectedPiece;
        var direction = pawn.tile.pos.y > Board.Instance.SelectedHighlight.Tile.pos.y ? new Vector2Int(0, 1) : new Vector2Int(0, -1);
        Debug.Log(Board.Instance.SelectedHighlight.Tile.pos + direction);
        var enemy = Board.Instance.Tiles[Board.Instance.SelectedHighlight.Tile.pos + direction];
        enemy.content.gameObject.SetActive(false);
        enemy.content = null;
        NormalMove();
    }

    private void PawnDoubleMove()
    {
        var pawn = Board.Instance.SelectedPiece;
        var direction = pawn.tile.pos.y > Board.Instance.SelectedHighlight.Tile.pos.y ? new Vector2Int(0, -1) : new Vector2Int(0, 1);
        Board.Instance.Tiles[pawn.tile.pos + direction].MoveType = MoveType.EnPassant;
        NormalMove();
    }

    private void Castling()
    {
        var king = Board.Instance.SelectedPiece;
        king.tile.content = null;
        var rook = Board.Instance.SelectedHighlight.Tile.content;
        rook.tile.content = null;

        var direction = rook.tile.pos - king.tile.pos;
        if (direction.x > 0)
        {
            king.tile = Board.Instance.Tiles[new Vector2Int(king.tile.pos.x + 2, king.tile.pos.y)];
            rook.tile = Board.Instance.Tiles[new Vector2Int(king.tile.pos.x - 1, rook.tile.pos.y)];
        }else
        {
            king.tile = Board.Instance.Tiles[new Vector2Int(king.tile.pos.x -2, king.tile.pos.y)];
            rook.tile = Board.Instance.Tiles[new Vector2Int(king.tile.pos.x + 1, rook.tile.pos.y)];
        }

        king.tile.content = king;
        rook.tile.content = rook;
        king.WasMoved = true;
        rook.WasMoved = true;

        king.transform.position = new Vector3(king.tile.pos.x, king.tile.pos.y, 0);
        rook.transform.position = new Vector3(rook.tile.pos.x, rook.tile.pos.y, 0);

        // LeanTween.move(king.gameObject, new Vector3(king.tile.pos.x, king.tile.pos.y, 0), 1.5f)
        //          .setOnComplete(() => 
        //          { 
        //              tcs.SetResult(true); 
        //          });
        // LeanTween.move(rook.gameObject, new Vector3(rook.tile.pos.x, rook.tile.pos.y, 0), 1.4f);
    }

    private void NormalMove(){

        var piece = Board.Instance.SelectedPiece;
        piece.transform.position = Board.Instance.SelectedHighlight.transform.position;
        piece.tile.content = null;
        piece.tile = Board.Instance.SelectedHighlight.Tile;
        if (piece.tile.content != null)
        {
            var deadPiece = piece.tile.content;
            Debug.LogFormat("The Piece {0} has been captured", deadPiece.transform.name);
            deadPiece.gameObject.SetActive(false);
        }
        piece.tile.content = piece;
        piece.WasMoved = true;

        //var timing = Vector3.Distance(piece.transform.position, Board.Instance.SelectedHighlight.transform.position) * 0.5f;
        // LeanTween.move(piece.gameObject, Board.Instance.SelectedHighlight.transform.position, timing)
        //     .setOnComplete(() => 
        //     { 
        //         tcs.SetResult(true); 
        //     }); 
    }
   
}
