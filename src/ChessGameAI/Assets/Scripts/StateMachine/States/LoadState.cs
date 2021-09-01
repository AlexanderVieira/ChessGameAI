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
        
        // var aiControlledPlayer1 = PlayerPrefs.GetString("AICONTROLLED_PLAYER1");
        // Machine.Player1.AIControlled = bool.Parse(aiControlledPlayer1);
        
        // var aiControlledPlayer2 = PlayerPrefs.GetString("AICONTROLLED_PLAYER2");
        // Machine.Player2.AIControlled = bool.Parse(aiControlledPlayer2);

        Machine.Player1.AIControlled = LevelController.Instance.AIControlledPlayer1;
        Debug.Log("Player1 AI: " + Machine.Player1.AIControlled);

        Machine.Player2.AIControlled = LevelController.Instance.AIControlledPlayer2;
        Debug.Log("Player2 AI: " + Machine.Player1.AIControlled);

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
