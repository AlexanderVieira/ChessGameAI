using System.Runtime.InteropServices;
using System.IO.MemoryMappedFiles;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class KingMovement : Movement
{
    public KingMovement()
    {
        PieceWeight = 1000;
    }

    public override List<AvailableMove> GetValidMoves()
    {        
        var moves = new List<AvailableMove>();
        moves.AddRange(UntilBlockedPath(new Vector2Int(1, 0), true, 1));
        moves.AddRange(UntilBlockedPath(new Vector2Int(-1, 0), true, 1));

        moves.AddRange(UntilBlockedPath(new Vector2Int(0, 1), true, 1));
        moves.AddRange(UntilBlockedPath(new Vector2Int(0, -1), true, 1));

        moves.AddRange(UntilBlockedPath(new Vector2Int(-1, -1), true, 1));
        moves.AddRange(UntilBlockedPath(new Vector2Int(-1, 1), true, 1));
        moves.AddRange(UntilBlockedPath(new Vector2Int(1, -1), true, 1));
        moves.AddRange(UntilBlockedPath(new Vector2Int(1, 1), true, 1));
        //SetNormalMove(moves);
        moves.AddRange(Castling());
        return moves;
    }

    private List<AvailableMove> Castling()
    {
        var availableMoves = new List<AvailableMove>();
        if (Board.Instance.SelectedPiece.GetComponent<King>().WasMoved)
        {
            return availableMoves;
        }
        var tile = CheckRook(new Vector2Int(1, 0));
        if (tile != null)
        {
            availableMoves.Add(new AvailableMove(tile.pos, MoveType.Castling));            
        }
        tile = CheckRook(new Vector2Int(-1, 0));
        if (tile != null)
        {
            availableMoves.Add(new AvailableMove(tile.pos, MoveType.Castling));            
        }
        return availableMoves;
    }

    private Tile CheckRook(Vector2Int direction)
    {
        Rook rook;
        var currentTile = GetTile(Board.Instance.SelectedPiece.tile.pos + direction);
        while (currentTile != null)
        {
            if (currentTile.content != null)
            {
                break;
            }
            currentTile = GetTile(currentTile.pos + direction);
        }

        if (currentTile == null)
        {
            return null;
        }
        rook = currentTile.content as Rook;
        if (rook == null || rook.WasMoved)
        {
            return null;
        }
        return rook.tile;
    }
}
