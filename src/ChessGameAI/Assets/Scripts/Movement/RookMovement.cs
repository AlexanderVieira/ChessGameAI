using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RookMovement : Movement
{
    public RookMovement(bool maxKingdom)
    {
        PieceWeight = 500;
        if (maxKingdom)
        {
            PositionValue = AIController.Instance.SquareTable.RookGolden;
        }
        else
        {
            PositionValue = AIController.Instance.SquareTable.RookGreen;
        }
    }

    public override List<AvailableMove> GetValidMoves()
    {
        var moves = new List<AvailableMove>();
        UntilBlockedPath(moves, new Vector2Int(1, 0), true, 99);
        UntilBlockedPath(moves, new Vector2Int(-1, 0), true, 99);
        UntilBlockedPath(moves, new Vector2Int(0, 1), true, 99);
        UntilBlockedPath(moves, new Vector2Int(0, -1), true, 99);        
        return moves;
    }
}
