using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RookMovement : Movement
{
    public RookMovement()
    {
        PieceWeight = 5;
    }

    public override List<Tile> GetValidMoves()
    {
        var moves = new List<Tile>();
        moves.AddRange(UntilBlockedPath(new Vector2Int(1, 0), true, 99));
        moves.AddRange(UntilBlockedPath(new Vector2Int(-1, 0), true, 99));
        moves.AddRange(UntilBlockedPath(new Vector2Int(0, 1), true, 99));
        moves.AddRange(UntilBlockedPath(new Vector2Int(0, -1), true, 99));
        SetNormalMove(moves);
        return moves;
    }
}
