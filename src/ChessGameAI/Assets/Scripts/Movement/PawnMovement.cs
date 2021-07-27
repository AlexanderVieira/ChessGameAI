using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PawnMovement : Movement
{
    public override List<Tile> GetValidMoves()
    {
        List<Vector2Int> aux = new List<Vector2Int>();
        var direction = GetDirection();
        aux.Add(Board._instance.SelectedPiece.tile.pos + direction);
        return ValidateExits(aux);
    }

    private List<Tile> ValidateExits(List<Vector2Int> positions)
    {
        var values = new List<Tile>();
        foreach (var position in positions)
        {
            Tile tile;
            if (Board._instance.Tiles.TryGetValue(position, out tile))
            {
                values.Add(tile);
            }
        }
        return values;
    }

    private Vector2Int GetDirection()
    {
        if (StateMachineController._instance.CurrentlyPlaying.transform.name == "GreenPieces")
        {
            return new Vector2Int(0, -1);
        }
        return new Vector2Int(0, 1);
    }
}
