using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AffectedPiece
{
    public Piece Piece;
    public Tile From;
    public Tile To;
    //public bool WasMoved;
    public virtual void Undo(){
        
        Piece.tile.content = null;
        Piece.tile = From;
        From.content = Piece;
        Piece.transform.position = new Vector3(From.pos.x, From.pos.y, 0);
        Piece.gameObject.SetActive(true);
        
    }
}

public class AffectedPawn : AffectedPiece
{
    public bool ResetMovement;
    public bool WasMoved;
    public override void Undo()
    {
        base.Undo();
        Piece.WasMoved = WasMoved;
        if (ResetMovement)
        {
            var pawn = Piece as Pawn;
            Piece.Movement = pawn.SavedMovement;
        }
    }
}

public class AffectedKingRook : AffectedPiece
{
    public bool WasMoved;
    public override void Undo()
    {
        base.Undo();
        Piece.WasMoved = WasMoved;
    }
}
