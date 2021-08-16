using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

public class TurnEndState : State
{
   public override async void EnterAsync()
    {
        Debug.Log("Turn End State:");
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
        var king = Board._instance.GoldenHolder.GetComponentInChildren<King>();
        if (king == null)
        {
            Debug.Log("Green Winner!");
            return true;
        }

        king = Board._instance.GreenHolder.GetComponentInChildren<King>();
        if (king == null)
        {
            Debug.Log("Golden Winner!");
            return true;
        }
        return false;

    }

    private bool CheckTeams()
    {
        var goldenPiece = Board._instance.GoldenPieces.Find((x) => x.gameObject.activeSelf == true);
        if (goldenPiece == null)
        {
            Debug.Log("Green Winner!");
            return true;
        }
        var greenPiece = Board._instance.GreenPieces.Find((x) => x.gameObject.activeSelf == true);
        if (greenPiece == null)
        {
            Debug.Log("Golden Winner!");
            return true;
        }
        return false;
    }
}
