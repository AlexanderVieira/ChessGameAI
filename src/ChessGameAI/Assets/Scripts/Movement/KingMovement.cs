using System.Runtime.InteropServices;
using System.IO.MemoryMappedFiles;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class KingMovement : Movement
{
    public override List<Tile> GetValidMoves()
    {        
        var moves = new List<Tile>();
        moves.AddRange(UntilBlockedPath(new Vector2Int(1, 0), true, 1));
        moves.AddRange(UntilBlockedPath(new Vector2Int(-1, 0), true, 1));

        moves.AddRange(UntilBlockedPath(new Vector2Int(0, 1), true, 1));
        moves.AddRange(UntilBlockedPath(new Vector2Int(0, -1), true, 1));

        moves.AddRange(UntilBlockedPath(new Vector2Int(-1, -1), true, 1));
        moves.AddRange(UntilBlockedPath(new Vector2Int(-1, 1), true, 1));
        moves.AddRange(UntilBlockedPath(new Vector2Int(1, -1), true, 1));
        moves.AddRange(UntilBlockedPath(new Vector2Int(1, 1), true, 1));
        moves.AddRange(Castling());
        return moves;
    }

    private List<Tile> Castling()
    {
        var moves = new List<Tile>();
        if (Board._instance.SelectedPiece.GetComponent<King>().WasMoved)
        {
            return moves;
        }
        var tile = CheckRook(new Vector2Int(1, 0));
        if (tile != null)
        {
            moves.Add(tile);
        }
        tile = CheckRook(new Vector2Int(-1, 0));
        if (tile != null)
        {
            moves.Add(tile);
        }
        return moves;
    }

    private Tile CheckRook(Vector2Int direction)
    {
        Rook rook;
        var currentTile = GetTile(Board._instance.SelectedPiece.tile.pos + direction);
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
