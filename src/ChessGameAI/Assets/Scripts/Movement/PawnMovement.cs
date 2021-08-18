using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PawnMovement : Movement
{
    public override List<Tile> GetValidMoves()
    {
        // List<Vector2Int> aux = new List<Vector2Int>();
        // var direction = GetDirection();
        // aux.Add(Board.Instance.SelectedPiece.tile.pos + direction);
        // var exits = ValidateExits(aux);
        // var moveable = UntilBlockedPath(exits);
        // moveable.AddRange(GetPawnAttack(direction));

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

        //List<Tile> moveable = UntilBlockedPath(direction, false, limit);
        //moveable.AddRange(GetPawnAttack(direction));
        //SetNormalMove(moveable);
        moveable.AddRange(moves);
        return moveable;
    }

    // private List<Tile> ValidateExits(List<Vector2Int> positions)
    // {
    //     var values = new List<Tile>();
    //     foreach (var position in positions)
    //     {
    //         Tile tile;
    //         if (Board.Instance.Tiles.TryGetValue(position, out tile))
    //         {
    //             values.Add(tile);
    //         }
    //     }
    //     return values;
    // }

    private Vector2Int GetDirection()
    {
        if (StateMachineController.Instance.CurrentlyPlaying.transform.name == "GreenPieces")
        {
            return new Vector2Int(0, -1);
        }
        return new Vector2Int(0, 1);
    }

    // private List<Tile> UntilBlockedPath(List<Tile> positions){

    //     var tilesValid = new List<Tile>();
    //     for (int i = 0; i < positions.Count; i++)
    //     {
    //         if (positions[i].content == null)
    //         {
    //             tilesValid.Add(positions[i]);
    //         }
    //     }
    //     return tilesValid;
    // }

    // private bool IsEnemy(Vector2Int pos, out Tile tile){

    //     if (Board.Instance.Tiles.TryGetValue(pos, out tile))
    //     {
    //         if (tile != null && tile.content != null)
    //         {
    //             if (tile.content.transform.parent != Board.Instance.SelectedPiece.transform.parent)
    //             {
    //                 return true;
    //             }
    //         }
    //     }
    //     return false;
    // }

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
