using System.Threading.Tasks;
using UnityEngine;

public class TurnEndState : State
{
   public override async void EnterAsync()
    {
        //Debug.Log("Turn End State:");
        bool gameFinished = CheckConditions();
        await Task.Delay(100);
        if (gameFinished)
        {
            Machine.ChangeTo<GameEndState>();
        }
        else
        {
            Machine.ChangeTo<TurnBeginState>();
        }
        
    }

    private bool CheckConditions()
    {
        if (CheckTeams() || CheckKing())
        {
            return true;
        }
        return false;
    }

    private bool CheckKing()
    {
        var king = Board.Instance.GoldenHolder.GetComponentInChildren<King>();
        if (king == null)
        {
            //Debug.Log("Green Winner!");
            return true;
        }

        king = Board.Instance.GreenHolder.GetComponentInChildren<King>();
        if (king == null)
        {
            //Debug.Log("Golden Winner!");
            return true;
        }
        return false;

    }

    private bool CheckTeams()
    {
        var goldenPiece = Board
                          .Instance
                          .GoldenPieces
                          .Find((x) => x.gameObject.activeSelf == true);
        if (goldenPiece == null)
        {
            Debug.Log("Green Winner!");
            return true;
        }
        var greenPiece = Board
                         .Instance
                         .GreenPieces
                         .Find((x) => x.gameObject.activeSelf == true);
        if (greenPiece == null)
        {
            Debug.Log("Golden Winner!");
            return true;
        }
        return false;
    }
}
