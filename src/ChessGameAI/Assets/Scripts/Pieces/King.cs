using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class King : Piece
{       
    protected override void Start()
    {
        base.Start();
        Movement = new KingMovement(MaxKingdom);
    }
    public override AffectedPiece CreateAffected()
    {
        var afkr = new AffectedKingRook
        {
            WasMoved = WasMoved
        };
        return afkr;
    }
}
