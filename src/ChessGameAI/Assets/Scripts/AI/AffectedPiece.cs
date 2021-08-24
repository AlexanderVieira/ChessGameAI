using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AffectedPiece
{
    public Piece Piece;
    public Tile From;
    public Tile to;
    public virtual void Undo(){
        
        Piece.tile.content = null;
        Piece.tile = From;
        From.content = Piece;
        
    }
}
