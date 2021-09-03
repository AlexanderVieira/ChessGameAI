using System.Threading.Tasks;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AIController : MonoBehaviour
{
    public static AIController Instance;    
    public Ply MinPly;
    public Ply MaxPly;
    public int GoalPlyDepth;
    public AvailableMove EnPassantFlagSaved;
    public PieceSquareTable SquareTable = new PieceSquareTable();
    private int _calculationCount;
    private float _lastTime;
    private const int ALPHA = -1000000;
    private const int BETA = 1000000;
    private void Awake(){
        
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else if (Instance != this)
        {
            Destroy(gameObject);
        }        
        
        MaxPly = new Ply
        {
            Score = 999999
        };
        MinPly = new Ply
        {
            Score = -999999
        };
        SquareTable.SetDictionaries();

    }

    [ContextMenu("Calculate Plays")]
    public async Task<Ply> CalculatePlays(){

        //_lastTime = Time.realtimeSinceStartup;
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
        _calculationCount = 0;        
        var currentPly = new Ply();
        currentPly.OriginPly = null;
        int currentPlyDepth = 0;
        currentPly.AffectedPieces = new List<AffectedPiece>();       
        currentPly.BestFuture = await CalculatePly(currentPly, ALPHA, BETA, 
                                       currentPlyDepth, minimaxDirection);
        
        //Debug.LogFormat("Melhor jogada para o GoldenPiece: {0}, com score: {1}", currentPly.BestFuture.Name, currentPly.BestFuture.Score);
        //Debug.Log("Calculations: " + _calculationCount);
        //Debug.Log("Time: " + (Time.realtimeSinceStartup - _lastTime));
        //PrintBestPly(currentPly.BestFuture);
        PieceMovementState.EnPassantFlag = EnPassantFlagSaved;
        return currentPly.BestFuture;
    }

    private void PrintBestPly(Ply bestFuture)
    {
        var currentPly = bestFuture;
        //Debug.Log("Melhor jogada: ");
        while (currentPly.OriginPly != null)
        {
            Debug.LogFormat("Kingdom {0}: {1} -> Position: {2}", 
            currentPly.AffectedPieces[0].Piece.transform.parent.name,
            currentPly.AffectedPieces[0].Piece.name,
            currentPly.AffectedPieces[0].To.pos);
            currentPly = currentPly.OriginPly;
        }
    }   

    private async Task<Ply> CalculatePly(Ply parentPly, int alpha, int beta, 
                                         int currentPlyDepth, int minimaxDirection)
    {              
        currentPlyDepth++;
        GoalPlyDepth = LevelController.Instance.Level;
        if (currentPlyDepth > GoalPlyDepth)
        {
            EvaluateBoard(parentPly);
            //var evalTask = Task.Run(() => EvaluateBoard(parentPly));
            //await evalTask;
            return parentPly;
        }

        List<Piece> kingdom;

        if (minimaxDirection == 1)
        {
            kingdom = Board.Instance.GoldenPieces;
            parentPly.BestFuture = MinPly;
        }
        else
        {
            kingdom = Board.Instance.GreenPieces;
            parentPly.BestFuture = MaxPly;
        }
        
        for (int i = 0; i < kingdom.Count; i++)       
        {            
            Board.Instance.SelectedPiece = kingdom[i];
            foreach (var availableMove in kingdom[i].Movement.GetValidMoves())
            {                
                _calculationCount++;
                Board.Instance.SelectedPiece = kingdom[i];
                Board.Instance.SelectedMove = availableMove;               
                var tcs = new TaskCompletionSource<bool>();
                PieceMovementState.MovePiece(tcs, true, availableMove.MoveType);
                await tcs.Task;               
                //newPly.Name = string.Format("{0}, {1} to {2}", parentPly.Name, pe.Piece.transform.parent.name + "-" + pe.Piece.name , tile.pos);
                var newPly = new Ply();
                newPly.AffectedPieces = PieceMovementState.AffectedPieces;
                newPly.EnPassantFlag = PieceMovementState.EnPassantFlag;               
                var calculation = await CalculatePly(newPly, alpha, beta, 
                                                     currentPlyDepth, minimaxDirection * -1);                
                parentPly.BestFuture = IsBest(parentPly.BestFuture, 
                                              minimaxDirection, calculation, 
                                              ref alpha, ref beta);                
                newPly.OriginPly = parentPly;                
                PieceMovementState.EnPassantFlag = parentPly.EnPassantFlag;
                ResetBoard(newPly);
                if (beta <= alpha)
                {
                    return parentPly.BestFuture;
                }
            }
        }
        return parentPly.BestFuture;        
    }

    private Ply IsBest(Ply ply, int minimaxDirection, Ply potencialBest, 
                       ref int alpha, ref int beta)
    {
        var best = ply;
        if (minimaxDirection == 1)
        {
            if (potencialBest.Score > ply.Score)
            {
                best = potencialBest;
            }
            alpha = Mathf.Max(alpha, best.Score);
        }
        else
        {
            if (potencialBest.Score < ply.Score)
            {
                best = potencialBest;
            }
            beta = Mathf.Min(beta, best.Score);
        }
        return best;
    }    

    //[ContextMenu("Evaluate Board")]
    public void EvaluateBoard(Ply ply){
       
        foreach (var pe in Board.Instance.GoldenPieces)
        {
            EvaluatePiece(pe, ply, 1);
        }

        foreach (var pe in Board.Instance.GreenPieces)
        {
            EvaluatePiece(pe, ply, -1);
        }
        //Debug.Log("Board Score: " + ply.Score);
    }

    private void EvaluatePiece(Piece pe, Ply ply, int scoreDirection)
    {        
        var positionValue = pe.Movement.PositionValue[pe.tile.pos];
        ply.Score += (pe.Movement.PieceWeight + positionValue) * scoreDirection;
    }    

    private void ResetBoard(Ply ply){

        foreach (var ap in ply.AffectedPieces)
        {
            ap.Undo();            
        }
    }
}
