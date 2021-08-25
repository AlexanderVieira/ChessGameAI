using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pawn : Piece
{
   protected override void Start(){

       base.Start();
       Movement = new PawnMovement(GetDirection());
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
