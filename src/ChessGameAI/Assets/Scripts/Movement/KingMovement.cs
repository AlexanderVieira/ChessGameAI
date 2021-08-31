using System.Runtime.InteropServices;
using System.IO.MemoryMappedFiles;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class KingMovement : Movement
{
    public KingMovement(bool maxKingdom)
    {
        PieceWeight = 1000;
        if (maxKingdom)
        {
            PositionValue = AIController.Instance.SquareTable.KingGolden;
        }
        else
        {
            PositionValue = AIController.Instance.SquareTable.KingGreen;
        }
    }

    public override List<AvailableMove> GetValidMoves()
    {        
        var moves = new List<AvailableMove>();
        UntilBlockedPath(moves, new Vector2Int(1, 0), true, 1);
        UntilBlockedPath(moves, new Vector2Int(-1, 0), true, 1);

        UntilBlockedPath(moves, new Vector2Int(0, 1), true, 1);
        UntilBlockedPath(moves, new Vector2Int(0, -1), true, 1);

        UntilBlockedPath(moves, new Vector2Int(-1, -1), true, 1);
        UntilBlockedPath(moves, new Vector2Int(-1, 1), true, 1);
        UntilBlockedPath(moves, new Vector2Int(1, -1), true, 1);
        UntilBlockedPath(moves, new Vector2Int(1, 1), true, 1);        
        
        Castling(moves);
        return moves;
    }

    private void Castling(List<AvailableMove> availableMoves)
    {        
        if (Board.Instance.SelectedPiece.GetComponent<King>().WasMoved)
        {
            return;
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
        return;
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
