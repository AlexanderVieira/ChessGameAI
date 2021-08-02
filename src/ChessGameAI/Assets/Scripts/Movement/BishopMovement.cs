using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BishopMovement : Movement
{
    public override List<Tile> GetValidMoves()
    {
        var moves = new List<Tile>();        
        moves.AddRange(UntilBlockedPath(new Vector2Int(-1, -1), true, 99));
        moves.AddRange(UntilBlockedPath(new Vector2Int(-1, 1), true, 99));
        moves.AddRange(UntilBlockedPath(new Vector2Int(1, -1), true, 99));
        moves.AddRange(UntilBlockedPath(new Vector2Int(1, 1), true, 99));
        return moves;
    }
}
