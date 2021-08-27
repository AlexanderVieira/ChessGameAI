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
    public int GoalPlyDepth = 2;
    public AvailableMove EnPassantFlagSaved;
    private int _calculationCount;
    private float _lastTime;
    private void Awake(){
        //TO DO: Verificar se a instância é nula.
        Instance = this;
        MaxPly = new Ply
        {
            Score = 999999
        };
        MinPly = new Ply
        {
            Score = -999999
        };
    }

    [ContextMenu("Calculate Plays")]
    public async void CalculatePlays(){

        _lastTime = Time.realtimeSinceStartup;
        int minimaxDirection;
        if (StateMachineController.Instance.CurrentlyPlaying == StateMachineController.Instance.Player1)
        {
            minimaxDirection = 1;
        }
        else
        {
            minimaxDirection = -1;
        }
        EnPassantFlagSaved = PieceMovementState.EnPassantFlag;
        CurrentState = CreateSnapshot();
        CurrentState.Name = "Start";
        _calculationCount = 0;
        
        var currentPly = CurrentState;
        currentPly.OriginPly = null;
        int currentPlyDepth = 0;
        currentPly.AffectedPieces = new List<AffectedPiece>();

        Debug.Log("Start");
        var calculation = CalculatePly(currentPly, GetKingdom(currentPly, minimaxDirection), currentPlyDepth, minimaxDirection);
        await calculation;
        currentPly.BestFuture = calculation.Result;

        //Debug.LogFormat("Melhor jogada para o GoldenPiece: {0}, com score: {1}", currentPly.BestFuture.Name, currentPly.BestFuture.Score);
        Debug.Log("Calculations: " + _calculationCount);
        Debug.Log("Time: " + (Time.realtimeSinceStartup - _lastTime));
        PrintBestPly(currentPly.BestFuture);
        PieceMovementState.EnPassantFlag = EnPassantFlagSaved;
    }

    private void PrintBestPly(Ply bestFuture)
    {
        var currentPly = bestFuture;
        Debug.Log("Melhor jogada: ");
        while (currentPly.OriginPly != null)
        {
            Debug.LogFormat("Kingdom {0}: {1} -> Position: {2}", 
            currentPly.AffectedPieces[0].Piece.transform.parent.name,
            currentPly.AffectedPieces[0].Piece.name,
            currentPly.AffectedPieces[0].To.pos);
            currentPly = currentPly.OriginPly;
        }
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
            //EvaluateBoard(parentPly);
            var evalTask = Task.Run(() => EvaluateBoard(parentPly));
            await evalTask;
            return parentPly;
        }

        if (minimaxDirection == 1)
        {
            parentPly.BestFuture = MinPly;
        }
        else
        {
            parentPly.BestFuture = MaxPly;
        }
        
        foreach (var pe in Kingdom)
        {
            //Debug.Log("Analisando peças " + pe.Piece);
            foreach (var availableMove in pe.AvailableMoves)
            {
                //Debug.Log("Analisando movimentos: " + tile.pos);
                _calculationCount++;
                Board.Instance.SelectedPiece = pe.Piece;
                Board.Instance.SelectedMove = availableMove;
                //Board.Instance.SelectedHighlight = AIHighlight;
                //AIHighlight.Tile = tile;
                //AIHighlight.transform.position = new Vector3(tile.pos.x, tile.pos.y, 0);
                var tcs = new TaskCompletionSource<bool>();
                PieceMovementState.MovePiece(tcs, true, availableMove.MoveType);
                await tcs.Task;
                
                var newPly = CreateSnapshot(parentPly);
                //newPly.Name = string.Format("{0}, {1} to {2}", parentPly.Name, pe.Piece.transform.parent.name + "-" + pe.Piece.name , tile.pos);
                newPly.AffectedPieces = PieceMovementState.AffectedPieces;
                newPly.EnPassantFlag = PieceMovementState.EnPassantFlag;
                //Debug.Log("Ply Name: " + newPly.Name);
                //EvaluateBoard(newPly);
                var nextKingdom = GetKingdom(newPly,minimaxDirection * -1);
                var calculation = await CalculatePly(newPly, nextKingdom, currentPlyDepth, minimaxDirection * -1);
                //await calculation;
                parentPly.BestFuture = IsBest(parentPly.BestFuture, minimaxDirection, calculation);
                //newPly.MoveType = tile.MoveType;
                newPly.OriginPly = parentPly;
                parentPly.FuturePlies.Add(newPly);
                PieceMovementState.EnPassantFlag = parentPly.EnPassantFlag;
                ResetBoard(newPly);
            }
        }
        return parentPly.BestFuture;        
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

    private Ply CreateSnapshot(Ply parentPly){

        var ply = new Ply();
        ply.Greens = new List<PieceEvaluation>();
        ply.Goldens = new List<PieceEvaluation>();

        foreach (var pe in parentPly.Goldens)
        {
            if (pe.Piece.gameObject.activeSelf)
            {
                ply.Goldens.Add(CreateEvaluationPiece(pe.Piece, ply));
            }
        }

        foreach (var pe in parentPly.Greens)
        {
            if (pe.Piece.gameObject.activeSelf)
            {
                ply.Greens.Add(CreateEvaluationPiece(pe.Piece, ply));
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
        //pe.Score = pe.Piece.Movement.PieceWeight;
        ply.Score += pe.Piece.Movement.PieceWeight * scoreDirection;
    }

    // [ContextMenu("Reset Board")]
    // private void ResetBoard(){

    //     CurrentState.AffectedPieces = PieceMovementState.AffectedPieces;
    //     ResetBoard(CurrentState);
    // }

    private void ResetBoard(Ply ply){

        foreach (var ap in ply.AffectedPieces)
        {
            ap.Undo();
            // ap.Piece.tile.content = null;
            // ap.Piece.tile = ap.From;
            // ap.From.content = ap.Piece;
            // ap.Piece.transform.position = new Vector3(ap.From.pos.x, ap.From.pos.y, 0);
            // ap.Piece.gameObject.SetActive(true);
        }
    }
}
