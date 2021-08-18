using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PawnMovement : Movement
{
    public override List<Tile> GetValidMoves()
    {        
        var direction = GetDirection();
        var moveable = GetPawnAttack(direction);
        var moves = new List<Tile>();
        int limit = 1;
        if (!Board.Instance.SelectedPiece.WasMoved)
        {
            limit = 2;
            moves = UntilBlockedPath(direction, false, limit);
            SetNormalMove(moves);
            if (moves.Count == limit)
            {
                moves[1].MoveType = MoveType.PawnDoubleMove;
            }
        }else
        {
            moves = UntilBlockedPath(direction, false, limit);
            SetNormalMove(moves);
        }
        
        moveable.AddRange(moves);
        CheckPromotion(moves);
        return moveable;
    }

    private void CheckPromotion(List<Tile> moves)
    {
        foreach (var tile in moves)
        {
            if (tile.pos.y == 0 || tile.pos.y == 7)
            {
                tile.MoveType = MoveType.Promotion;
            }
        }
    }    

    private Vector2Int GetDirection()
    {
        if (StateMachineController.Instance.CurrentlyPlaying.transform.name == "GreenPieces")
        {
            return new Vector2Int(0, -1);
        }
        return new Vector2Int(0, 1);
    }
    
    private List<Tile> GetPawnAttack(Vector2Int direction){

        var pawAttack = new List<Tile>();        
        var piece = Board.Instance.SelectedPiece;
        var lefPos = new Vector2Int(piece.tile.pos.x - 1, piece.tile.pos.y + direction.y);
        var rightPos = new Vector2Int(piece.tile.pos.x + 1, piece.tile.pos.y + direction.y);
        GetPawnAttack(GetTile(lefPos), pawAttack);
        GetPawnAttack(GetTile(rightPos), pawAttack);        
        return pawAttack;
    }

    private void GetPawnAttack(Tile tile, List<Tile> pawAttack)
    {
        if (tile == null)
        {
            return;
        }        
        
        if (IsEnemy(tile))
        {
            tile.MoveType = MoveType.Normal;
            pawAttack.Add(tile);
        }        
        else if (tile.MoveType == MoveType.EnPassant)
        {            
            pawAttack.Add(tile);
        }
    }
}
