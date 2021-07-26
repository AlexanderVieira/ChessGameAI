using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

public class LoadState : State
{
    public override async void Enter()
    {
        Debug.Log("Initial State Loaded.");
        await Board._instance.LoadAsync();
        await LoadAllPiecesAsync();
        Machine.CurrentlyPlaying = Machine.Player2;
        Machine.ChangeTo<TurnBeginState>();
    }

    private async Task LoadAllPiecesAsync()
    {
        LoadTeamPieces(Board._instance.GoldenPieces);
        LoadTeamPieces(Board._instance.GreenPieces);
        await Task.Delay(100);
    }

    private void LoadTeamPieces(List<Piece> pieces)
    {
        foreach (var piece in pieces)
        {
            Board._instance.AddPiece(piece.transform.parent.name, piece);
        }
    }
}
