using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QueenMovement : Movement
{
    public QueenMovement()
    {
        PieceWeight = 9;
    }

    public override List<AvailableMove> GetValidMoves()
    {
        var moves = new List<AvailableMove>();
        moves.AddRange(UntilBlockedPath(new Vector2Int(1, 0), true, 99));
        moves.AddRange(UntilBlockedPath(new Vector2Int(-1, 0), true, 99));

        moves.AddRange(UntilBlockedPath(new Vector2Int(0, 1), true, 99));
        moves.AddRange(UntilBlockedPath(new Vector2Int(0, -1), true, 99));

        moves.AddRange(UntilBlockedPath(new Vector2Int(-1, -1), true, 99));
        moves.AddRange(UntilBlockedPath(new Vector2Int(-1, 1), true, 99));
        moves.AddRange(UntilBlockedPath(new Vector2Int(1, -1), true, 99));
        moves.AddRange(UntilBlockedPath(new Vector2Int(1, 1), true, 99));
        //SetNormalMove(moves);
        return moves;
    }
}
