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
        var exits = ValidateExits(aux);
        var moveable = UntilBlockedPath(exits);
        moveable.AddRange(GetPawnAttack(direction));
        return moveable;
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

    private List<Tile> UntilBlockedPath(List<Tile> positions){

        var tilesValid = new List<Tile>();
        for (int i = 0; i < positions.Count; i++)
        {
            if (positions[i].content == null)
            {
                tilesValid.Add(positions[i]);
            }
        }
        return tilesValid;
    }

    private bool IsEnemy(Vector2Int pos, out Tile tile){

        if (Board._instance.Tiles.TryGetValue(pos, out tile))
        {
            if (tile != null && tile.content != null)
            {
                if (tile.content.transform.parent != Board._instance.SelectedPiece.transform.parent)
                {
                    return true;
                }
            }
        }
        return false;
    }

    private List<Tile> GetPawnAttack(Vector2Int direction){

        var pawAttack = new List<Tile>();
        Tile tile;
        var piece = Board._instance.SelectedPiece;
        var lefPos = new Vector2Int(piece.tile.pos.x - 1, piece.tile.pos.y + direction.y);
        var rightPos = new Vector2Int(piece.tile.pos.x + 1, piece.tile.pos.y + direction.y);
        if (IsEnemy(lefPos, out tile))
        {
            pawAttack.Add(tile);
        }
        if (IsEnemy(rightPos, out tile))
        {
            pawAttack.Add(tile);
        }
        return pawAttack;
    }
}
