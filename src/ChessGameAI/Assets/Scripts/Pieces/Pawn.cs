using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pawn : Piece
{
    public Movement SavedMovement;
    public Movement QueenMovement;
    public Movement KnightMovement;
   protected override void Start(){

       base.Start();
       Movement = new PawnMovement(MaxKingdom);
       SavedMovement = new PawnMovement(MaxKingdom);
       QueenMovement = new QueenMovement(MaxKingdom);
       KnightMovement = new KnightMovement(MaxKingdom);       
   }    
    public override AffectedPiece CreateAffected()
    {
        var afp = new AffectedPawn
        {
            WasMoved = WasMoved
        };
        return afp;
    }

}
