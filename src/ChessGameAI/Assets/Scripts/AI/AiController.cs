using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AIController : MonoBehaviour
{
    public static AIController Instance;
    public Ply CurrentState;
    private void Awake(){

        Instance = this;
    }

    [ContextMenu("Calculate Ply")]
    public void CalculatePly(){

        CurrentState = CreateSnapshot();
        CurrentState.Name = "Start";
        EvaluateBoard(CurrentState);
        var currentPly = CurrentState;
        currentPly.OriginPly = null;
        currentPly.FuturePlies = new List<Ply>();
        Debug.Log("Início");
    }

    //[ContextMenu("Create Snapshot")]
    public Ply CreateSnapshot(){

        var ply = new Ply();
        ply.Greens = new List<PieceEvaluation>();
        ply.Goldens = new List<PieceEvaluation>();

        foreach (var piece in Board.Instance.GoldenPieces)
        {
            if (piece.gameObject.activeSelf)
            {
                ply.Goldens.Add(CreateEvaluationPiece(piece, ply));
            }
        }

        foreach (var piece in Board.Instance.GreenPieces)
        {
            if (piece.gameObject.activeSelf)
            {
                ply.Greens.Add(CreateEvaluationPiece(piece, ply));
            }
        }
        return ply;
    }

    private PieceEvaluation CreateEvaluationPiece(Piece piece, Ply ply)
    {
        var pe = new PieceEvaluation();
        pe.Piece = piece;
        return pe;
    }

    //[ContextMenu("Evaluate Board")]
    public void EvaluateBoard(Ply ply){
       
        foreach (var pe in ply.Goldens)
        {
            EvaluatePiece(pe, ply, 1);
        }

        foreach (var pe in ply.Greens)
        {
            EvaluatePiece(pe, ply, -1);
        }

        Debug.Log("Board Score: " + ply.Score);
    }

    private void EvaluatePiece(PieceEvaluation pe, Ply ply, int scoreDirection)
    {
        Board.Instance.SelectedPiece = pe.Piece;
        var tiles = pe.Piece.Movement.GetValidMoves();
        pe.AvailableMoves =tiles.Count;
        pe.Score = pe.Piece.Movement.PieceWeight;
        ply.Score += pe.Score * scoreDirection;
    }

    [ContextMenu("Reset Board")]
    private void ResetBoard(){

        CurrentState.AffectedPieces = PieceMovementState.AffectedPieces;
        ResetBoard(CurrentState);
    }

    private void ResetBoard(Ply ply){

        foreach (var ap in ply.AffectedPieces)
        {
            ap.Piece.tile.content = null;
            ap.Piece.tile = ap.From;
            ap.From.content = ap.Piece;
            ap.Piece.transform.position = new Vector3(ap.From.pos.x, ap.From.pos.y, 0);
            ap.Piece.gameObject.SetActive(true);
        }
    }
}
