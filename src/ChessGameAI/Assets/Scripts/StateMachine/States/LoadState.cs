using System.Collections.Generic;
using System.Threading.Tasks;

public class LoadState : State
{
    public override async void EnterAsync()
    {
        //Debug.Log("Initial State Loaded.");
        await Board.Instance.LoadAsync();
        LoadAllPiecesAsync();
        Machine.Player1.AIControlled = LevelController.Instance.AIControlledPlayer1;        
        Machine.Player2.AIControlled = LevelController.Instance.AIControlledPlayer2;
        Machine.CurrentlyPlaying = Machine.Player2;
        await Task.Delay(100);
        Machine.ChangeTo<TurnBeginState>();
    }

    private async void LoadAllPiecesAsync()
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
        //await Task.Delay(100);
        
    }
}
