using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

public class LoadState : State
{
    public override async void EnterAsync()
    {
        Debug.Log("Initial State Loaded.");
        await Board.Instance.LoadAsync();
        await LoadAllPiecesAsync();
        Machine.CurrentlyPlaying = Machine.Player2;
        Machine.ChangeTo<TurnBeginState>();
    }

    private async Task LoadAllPiecesAsync()
    {
        LoadTeamPieces(Board.Instance.GoldenPieces);
        LoadTeamPieces(Board.Instance.GreenPieces);
        await Task.Delay(100);
    }

    private void LoadTeamPieces(List<Piece> pieces)
    {
        foreach (var piece in pieces)
        {
            Board.Instance.AddPiece(piece.transform.parent.name, piece);
        }
    }
}
