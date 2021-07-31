using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Movement
{
    public abstract List<Tile> GetValidMoves();
    protected bool IsEnemy(Tile tile){

        if (tile.content != null && tile.content.transform.parent != Board._instance.SelectedPiece.transform.parent)
        {
            return true;
        }        
        return false;
    } 

    protected Tile GetTile(Vector2Int position){

        Tile tile;
        Board._instance.Tiles.TryGetValue(position, out tile);
        return tile;
    }
    protected List<Tile> UntilBlockedPath(Vector2Int direction, bool includeBlocked, int limit){

        var moves = new List<Tile>();
        var current = Board._instance.SelectedPiece.tile;
        while (current != null && moves.Count < limit)
        {
            if (Board._instance.Tiles.TryGetValue(current.pos + direction, out current))
            {
                if (current.content == null)
                {
                    moves.Add(current);

                }else if(IsEnemy(current))
                {
                    if (includeBlocked)
                    {
                        moves.Add(current);
                    }
                    return moves;

                }else
                {
                    return moves;
                }
            }
        }
        return moves;       
    }
}
