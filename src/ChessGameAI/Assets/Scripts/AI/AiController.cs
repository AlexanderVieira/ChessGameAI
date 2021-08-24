using System.Threading.Tasks;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AIController : MonoBehaviour
{
    public static AIController Instance;
    public Ply CurrentState;
    public HighlightClick AIHighlight;
    public Ply MinPly;
    public Ply MaxPly;
    public int GoalPlyDepth = 4;
    private int _calculationCount;
    private void Awake(){

        Instance = this;
    }

    [ContextMenu("Calculate Plays")]
    public async void CalculatePlays(){

        int minimaxDirection = 1;
        CurrentState = CreateSnapshot();
        CurrentState.Name = "Start";
        _calculationCount = 0;
        //EvaluateBoard(CurrentState);
        var currentPly = CurrentState;
        currentPly.OriginPly = null;
        int currentPlyDepth = 0;
        currentPly.AffectedPieces = new List<AffectedPiece>();

        Debug.Log("Start");
        Task<Ply> calculation = CalculatePly(currentPly, GetKingdom(currentPly, minimaxDirection), currentPlyDepth, minimaxDirection);
        await calculation;
        currentPly.BestFuture = calculation.Result;

        Debug.LogFormat("Melhor jogada para o GoldenPiece: {0}, com score: {1}", currentPly.BestFuture.Name, currentPly.BestFuture.Score);
        Debug.Log("Calculations: " + _calculationCount);

        // currentPly.FuturePlies = new List<Ply>();
        // MinPly = new Ply()
        // { 
        //     Score = float.MaxValue
        // };
        // MaxPly = new Ply()
        // {
        //     Score = float.MinValue
        // };
        // Debug.Log("Start");
        // int currentPlyDepth = 1;
        // CalculatePly(currentPly, currentPly.Goldens, currentPlyDepth);
        // currentPlyDepth++;
        // foreach (var cfp in currentPly.FuturePlies)
        // {
        //     CalculatePly(cfp, cfp.Greens, currentPlyDepth);
        // }
        // Debug.LogFormat("Melhor jogada para o GoldenPiece: {0}", MaxPly.Name);       
        // Debug.Log("Number future Plies: " + currentPly.FuturePlies.Count);
        // currentPly.FuturePlies.Sort((x, y) => x.Score.CompareTo(y.Score));
        // Debug.Log("Would choose: " + currentPly.FuturePlies[currentPly.FuturePlies.Count - 1].Name);
    }

    private List<PieceEvaluation> GetKingdom(Ply ply, int minimaxDirection)
    {
        if (minimaxDirection == 1)
        {
            return ply.Goldens;
        }
        else
        {
            return ply.Greens;
        }
    }

    private async Task<Ply> CalculatePly(Ply parentPly, List<PieceEvaluation> Kingdom, int currentPlyDepth, int minimaxDirection)
    {
        parentPly.FuturePlies = new List<Ply>();
        currentPlyDepth++;
        if (currentPlyDepth > GoalPlyDepth)
        {
            EvaluateBoard(parentPly);
            return parentPly;
        }
        var plyceHolder = new Ply()
        {
            Score = -99999 * minimaxDirection
        };
        parentPly.BestFuture = plyceHolder;
        
        foreach (var pe in Kingdom)
        {
            Debug.Log("Analisando peças " + pe.Piece);
            foreach (var tile in pe.AvailableMoves)
            {
                Debug.Log("Analisando movimentos: " + tile.pos);
                _calculationCount++;
                Board.Instance.SelectedPiece = pe.Piece;
                Board.Instance.SelectedHighlight = AIHighlight;
                AIHighlight.Tile = tile;
                AIHighlight.transform.position = new Vector3(tile.pos.x, tile.pos.y, 0);
                var tcs = new TaskCompletionSource<bool>();
                PieceMovementState.MovePiece(tcs, true, tile.MoveType);
                await tcs.Task;
                
                var newPly = CreateSnapshot();
                newPly.Name = string.Format("{0}, {1} to {2}", parentPly.Name, pe.Piece.transform.parent.name + "-" + pe.Piece.name , tile.pos);
                newPly.AffectedPieces = PieceMovementState.AffectedPieces;
                //Debug.Log("Ply Name: " + newPly.Name);
                //EvaluateBoard(newPly);
                var calculation = CalculatePly(newPly, GetKingdom(newPly,minimaxDirection * -1), currentPlyDepth, minimaxDirection * -1);
                await calculation;
                parentPly.BestFuture = IsBest(parentPly.BestFuture, minimaxDirection, calculation.Result);

                newPly.MoveType = tile.MoveType;
                newPly.OriginPly = parentPly;
                parentPly.FuturePlies.Add(newPly);
                ResetBoard(newPly);
            }
        }
        return parentPly.BestFuture;
        // if (currentPlyDepth == GoalPlyDepth)
        // {
        //     SetMinMax(parentPly.FuturePlies);
        // }
    }

    private Ply IsBest(Ply ply, int minimaxDirection, Ply potencialBest)
    {
        if (minimaxDirection == 1)
        {
            if (potencialBest.Score > ply.Score)
            {
                return potencialBest;
            }
            return ply;
        }
        else
        {
            if (potencialBest.Score < ply.Score)
            {
                return potencialBest;
            }
            return ply;
        }
    }    

    private void SetMinMax(List<Ply> futurePlies)
    {
        foreach (var fp in futurePlies)
        {
            if (fp.Score > MaxPly.Score)
            {
                MaxPly = fp;
            }
            else if(fp.Score < MinPly.Score)
            {
                MinPly = fp;                
            }
        }
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
        Board.Instance.SelectedPiece = pe.Piece;
        pe.AvailableMoves = pe.Piece.Movement.GetValidMoves();
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
        // Board.Instance.SelectedPiece = pe.Piece;
        // pe.AvailableMoves = pe.Piece.Movement.GetValidMoves();         
        pe.Score = pe.Piece.Movement.PieceWeight;
        ply.Score += pe.Score * scoreDirection;
    }

    // [ContextMenu("Reset Board")]
    // private void ResetBoard(){

    //     CurrentState.AffectedPieces = PieceMovementState.AffectedPieces;
    //     ResetBoard(CurrentState);
    // }

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
