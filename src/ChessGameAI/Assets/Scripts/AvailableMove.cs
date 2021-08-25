using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public struct AvailableMove
{
    public Vector2Int Pos;
    public MoveType MoveType;
    public AvailableMove(Vector2Int rcvPos, MoveType rcvMoveType)
    {
        Pos = rcvPos;
        MoveType = rcvMoveType;
    }

    public AvailableMove(Vector2Int rcvPos)
    {
        Pos = rcvPos;
        MoveType = MoveType.Normal;
    }
}
