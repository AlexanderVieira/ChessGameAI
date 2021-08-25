using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BishopMovement : Movement
{
    public BishopMovement()
    {
        PieceWeight = 3;
    }

    public override List<AvailableMove> GetValidMoves()
    {
        var moves = new List<AvailableMove>();        
        moves.AddRange(UntilBlockedPath(new Vector2Int(-1, -1), true, 99));
        moves.AddRange(UntilBlockedPath(new Vector2Int(-1, 1), true, 99));
        moves.AddRange(UntilBlockedPath(new Vector2Int(1, -1), true, 99));
        moves.AddRange(UntilBlockedPath(new Vector2Int(1, 1), true, 99));
        //SetNormalMove(moves);
        return moves;
    }
}
