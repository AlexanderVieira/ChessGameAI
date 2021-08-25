using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class King : Piece
{
    public override AffectedPiece CreateAffected()
    {
        var afkr = new AffectedKingRook
        {
            WasMoved = WasMoved
        };
        return afkr;
    }
    private void Awake(){
        
        Movement = new KingMovement();
                
    }
}
