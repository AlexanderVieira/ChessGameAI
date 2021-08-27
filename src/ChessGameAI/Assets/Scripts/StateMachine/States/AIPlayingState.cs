using System.Threading.Tasks;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AIPlayingState : State
{
    public async override void EnterAsync()
    {
        var bestResult = await AIController.Instance.CalculatePlays();
        MakeBestPlay(bestResult);
    }

    private async void MakeBestPlay(Ply bestResult)
    {
        var currentPly = bestResult;
        for (int i = 1; i < AIController.Instance.GoalPlyDepth; i++)
        {
            currentPly = currentPly.OriginPly;
        }
        Board.Instance.SelectedPiece = currentPly.AffectedPieces[0].Piece;
        Debug.Log(currentPly.AffectedPieces[0].Piece.name);
        Board.Instance.SelectedMove = GetMoveType(currentPly);
        Debug.Log(Board.Instance.SelectedMove);
        await Task.Delay(100);
        Machine.ChangeTo<PieceMovementState>();
    }

    private AvailableMove GetMoveType(Ply currentPly)
    {
        List<PieceEvaluation> Kingdom;
        if (Machine.CurrentlyPlaying == Machine.Player1)
        {
            Kingdom = currentPly.Goldens;
        }
        else
        {
            Kingdom = currentPly.Greens;
        }
        var moves = Board.Instance.SelectedPiece.Movement.GetValidMoves();
        foreach (var move in moves)
        {
            if (move.Pos == currentPly.AffectedPieces[0].To.pos)
            {
                return move;
            }
        }
        return new AvailableMove();
    }
}
