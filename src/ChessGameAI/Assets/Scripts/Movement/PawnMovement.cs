using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PawnMovement : Movement
{
    private Vector2Int _direction;
    public int _promotionHeight;

    public PawnMovement(bool MaxKingdom)
    {        
        PieceWeight = 1;
        if (MaxKingdom)
        {
            _direction = new Vector2Int(0, 1);
            _promotionHeight = 7;
            PositionValue = AIController.Instance.SquareTable.PawnGolden;
        }
        else
        {
            _direction = new Vector2Int(0, -1);
            _promotionHeight = 0;
            PositionValue = AIController.Instance.SquareTable.PawnGreen;
        }
    }

    public override List<AvailableMove> GetValidMoves()
    {        
        var moveable = new List<AvailableMove>();
        var moves = new List<AvailableMove>();
        GetPawnAttack(moveable);
        int limit = 1;
        if (!Board.Instance.SelectedPiece.WasMoved)
        {
            limit = 2;
            UntilBlockedPath(moves, _direction, false, limit);            
            if (moves.Count == limit)
            {
                moves[1] = new AvailableMove(moves[1].Pos, MoveType.PawnDoubleMove);
            }
        }else
        {
            UntilBlockedPath(moves, _direction, false, limit);
            if (moves.Count > 0)
            {
                moves[0] = CheckPromotion(moves[0]);
            }            
        }        
        moveable.AddRange(moves);        
        return moveable;
    }   

    private Vector2Int GetDirection()
    {
        if (Board.Instance.SelectedPiece.transform.parent.name == "GreenPieces")
        {
            return new Vector2Int(0, -1);
        }
        return new Vector2Int(0, 1);
    }
    
    private void GetPawnAttack(List<AvailableMove> pawnAttack){
                
        var piece = Board.Instance.SelectedPiece;
        var lefPos = new Vector2Int(piece.tile.pos.x - 1, piece.tile.pos.y + _direction.y);
        var rightPos = new Vector2Int(piece.tile.pos.x + 1, piece.tile.pos.y + _direction.y);
        GetPawnAttack(GetTile(lefPos), pawnAttack);
        GetPawnAttack(GetTile(rightPos), pawnAttack);    
        
    }

    private void GetPawnAttack(Tile tile, List<AvailableMove> pawAttack)
    {
        if (tile == null)
        {
            return;
        }        
        
        if (IsEnemy(tile))
        {            
            pawAttack.Add(new AvailableMove(tile.pos, MoveType.Normal));
        }        
        else if (PieceMovementState.EnPassantFlag.MoveType == MoveType.EnPassant && PieceMovementState.EnPassantFlag.Pos == tile.pos)
        {            
            pawAttack.Add(new AvailableMove(tile.pos, MoveType.EnPassant));
        }
    }
    private AvailableMove CheckPromotion(AvailableMove availableMove){
       
        if (availableMove.Pos.y != _promotionHeight)
        {
            return availableMove;
        }
        return new AvailableMove(availableMove.Pos, MoveType.Promotion);
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
}
