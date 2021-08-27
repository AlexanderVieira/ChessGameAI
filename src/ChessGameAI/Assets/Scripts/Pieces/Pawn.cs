using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pawn : Piece
{
    public Movement SavedMovement;
    public Movement QueenMovement = new QueenMovement();
    public Movement KnightMovement = new KnightMovement();
   protected override void Start(){

       base.Start();
       Movement = new PawnMovement(MaxKingdom);
       SavedMovement = new PawnMovement(MaxKingdom);
       //Movement = new PawnMovement(GetDirection());
       //SavedMovement = new PawnMovement(GetDirection());
   }
   public override AffectedPiece CreateAffected()
    {
        var afp = new AffectedPawn
        {
            WasMoved = WasMoved
        };
        return afp;
    }

    private Vector2Int GetDirection()
    {
        if (MaxKingdom)
        {
            return new Vector2Int(0, 1);
        }
        return new Vector2Int(0, -1);
    }
}
