using System.Threading.Tasks;

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
        //Debug.Log("GoalPlyDepth: " + AIController.Instance.GoalPlyDepth);
        for (int i = 1; i < AIController.Instance.GoalPlyDepth; i++)
        {
            currentPly = currentPly.OriginPly;
        }
        Board.Instance.SelectedPiece = currentPly.AffectedPieces[0].Piece;        
        Board.Instance.SelectedMove = await GetMoveType(currentPly);        
        await Task.Delay(100);
        Machine.ChangeTo<PieceMovementState>();
    }

    private async Task<AvailableMove> GetMoveType(Ply currentPly)
    {        
        var moves = Board.Instance.SelectedPiece.Movement.GetValidMoves();
        foreach (var move in moves)
        {
            if (move.Pos == currentPly.AffectedPieces[0].To.pos)
            {
                return move;
            }
        }
        await Task.Delay(100);
        return new AvailableMove();
    }
}
