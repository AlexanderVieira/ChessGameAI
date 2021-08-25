using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PawnMovement : Movement
{
    public Vector2Int Direction;
    public PawnMovement(Vector2Int rcvDirection)
    {
        Direction = rcvDirection;
        PieceWeight = 1;
    }

    public override List<AvailableMove> GetValidMoves()
    {        
        var direction = GetDirection();
        var moveable = GetPawnAttack(direction);
        var moves = new List<AvailableMove>();
        int limit = 1;
        if (!Board.Instance.SelectedPiece.WasMoved)
        {
            limit = 2;
            moves = UntilBlockedPath(direction, false, limit);
            //SetNormalMove(moves);
            if (moves.Count == limit)
            {
                moves[1] = new AvailableMove(moves[1].Pos, MoveType.PawnDoubleMove);
            }
        }else
        {
            moves = UntilBlockedPath(direction, false, limit);
            if (moves.Count > 0)
            {
                moves[0] = CheckPromotion(moves[0]);
            }
            //SetNormalMove(moves);
        }
        
        moveable.AddRange(moves);
        //CheckPromotion(moves);
        return moveable;
    }

    private AvailableMove CheckPromotion(AvailableMove availableMove){

        int promotionHeight = 0;
        if (Board.Instance.SelectedPiece.MaxKingdom)
        {
            promotionHeight = 7;
        }
        if (availableMove.Pos.y != promotionHeight)
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

    private Vector2Int GetDirection()
    {
        if (Board.Instance.SelectedPiece.transform.parent.name == "GreenPieces")
        {
            return new Vector2Int(0, -1);
        }
        return new Vector2Int(0, 1);
    }
    
    private List<AvailableMove> GetPawnAttack(Vector2Int direction){

        var pawAttack = new List<AvailableMove>();        
        var piece = Board.Instance.SelectedPiece;
        var lefPos = new Vector2Int(piece.tile.pos.x - 1, piece.tile.pos.y + direction.y);
        var rightPos = new Vector2Int(piece.tile.pos.x + 1, piece.tile.pos.y + direction.y);
        GetPawnAttack(GetTile(lefPos), pawAttack);
        GetPawnAttack(GetTile(rightPos), pawAttack);        
        return pawAttack;
    }

    private void GetPawnAttack(Tile tile, List<AvailableMove> pawAttack)
    {
        if (tile == null)
        {
            return;
        }        
        
        if (IsEnemy(tile))
        {
            //tile.MoveType = MoveType.Normal;
            pawAttack.Add(new AvailableMove(tile.pos, MoveType.Normal));
        }        
        else if (PieceMovementState.EnPassantFlag.MoveType == MoveType.EnPassant && PieceMovementState.EnPassantFlag.Pos == tile.pos)
        {            
            pawAttack.Add(new AvailableMove(tile.pos, MoveType.EnPassant));
        }
    }
}
