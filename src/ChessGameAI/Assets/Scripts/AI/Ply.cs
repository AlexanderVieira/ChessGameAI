using System.Globalization;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ply
{
    public int Score;    
    public List<AffectedPiece> AffectedPieces;    
    public Ply OriginPly;    
    public Ply BestFuture;
    public AvailableMove EnPassantFlag;

}
