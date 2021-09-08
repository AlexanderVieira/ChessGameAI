using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BishopMovement : Movement
{
    public BishopMovement(bool maxKingdom)
    {
        PieceWeight = 330;
        if (maxKingdom)
        {
            PositionValue = AIController.Instance.SquareTable.BishopGolden;
        }
        else
        {
            PositionValue = AIController.Instance.SquareTable.BishopGreen;
        }
    }

    public override List<AvailableMove> GetValidMoves()
    {
        var moves = new List<AvailableMove>();        
        UntilBlockedPath(moves, new Vector2Int(-1, -1), true, 99);
        UntilBlockedPath(moves,new Vector2Int(-1, 1), true, 99);
        UntilBlockedPath(moves,new Vector2Int(1, -1), true, 99);
        UntilBlockedPath(moves,new Vector2Int(1, 1), true, 99);        
        return moves;
    }
}
