using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QueenMovement : Movement
{
    public QueenMovement(bool maxKingdom)
    {
        PieceWeight = 9;
        if (maxKingdom)
        {
            PositionValue = AIController.Instance.SquareTable.QueenGolden;
        }
        else
        {
            PositionValue = AIController.Instance.SquareTable.QueenGreen;
        }
    }

    public override List<AvailableMove> GetValidMoves()
    {
        var moves = new List<AvailableMove>();
        UntilBlockedPath(moves, new Vector2Int(1, 0), true, 99);
        UntilBlockedPath(moves, new Vector2Int(-1, 0), true, 99);

        UntilBlockedPath(moves, new Vector2Int(0, 1), true, 99);
        UntilBlockedPath(moves, new Vector2Int(0, -1), true, 99);

        UntilBlockedPath(moves, new Vector2Int(-1, -1), true, 99);
        UntilBlockedPath(moves, new Vector2Int(-1, 1), true, 99);
        UntilBlockedPath(moves, new Vector2Int(1, -1), true, 99);
        UntilBlockedPath(moves, new Vector2Int(1, 1), true, 99);
        
        return moves;
    }
}
