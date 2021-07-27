using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pawn : Piece
{
   private void Awake(){
        Movement = new PawnMovement();
    }
}
