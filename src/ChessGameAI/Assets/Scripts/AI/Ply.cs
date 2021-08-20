using System.Globalization;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ply
{
    public string Name;
    public List<PieceEvaluation> Goldens;
    public List<PieceEvaluation> Greens;
    public List<AffectedPiece> AffectedPieces;
    public float Score;
    public MoveType MoveType;
    public Ply OriginPly;
    public List<Ply> FuturePlies;

}
