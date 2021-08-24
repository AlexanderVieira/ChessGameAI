using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KnightMovement : Movement
{
    public KnightMovement()
    {
        PieceWeight = 3;
    }

    public override List<Tile> GetValidMoves()
    {
        var moves = new List<Tile>();
        moves.AddRange(GetStraightMovement(new Vector2Int(0, 1)));
        moves.AddRange(GetStraightMovement(new Vector2Int(0, -1)));
        moves.AddRange(GetStraightMovement(new Vector2Int(1, 0)));
        moves.AddRange(GetStraightMovement(new Vector2Int(-1, 0)));
        //SetNormalMove(moves);
        return moves;
    }

    private List<Tile> GetStraightMovement(Vector2Int direction)
    {
        var moves = new List<Tile>();
        var current = Board.Instance.SelectedPiece.tile;
        var aux = GetTile(current.pos + direction * 2);
        if (aux != null)
        {
            moves.AddRange(GetCurveMovement(aux.pos, new Vector2Int(direction.y, direction.x)));
        }
        return moves;
    }

    private List<Tile> GetCurveMovement(Vector2Int pos, Vector2Int direction)
    {
        var tiles = new List<Tile>();
        var tileA = GetTile(pos + direction);
        var tileB = GetTile(pos - direction);
        if (tileA != null && (tileA.content == null || IsEnemy(tileA)))
        {
            tiles.Add(tileA);            
        }
        if (tileB != null && (tileB.content == null || IsEnemy(tileB)))
        {
            tiles.Add(tileB);
        }
        return tiles;
    }
}
