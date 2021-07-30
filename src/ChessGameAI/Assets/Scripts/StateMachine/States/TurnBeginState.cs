using System.Threading.Tasks;
using UnityEngine;

public class TurnBeginState : State
{
    public override async void EnterAsync()
    {
        Debug.Log("Turn Begin:");
        if (Machine.CurrentlyPlaying == Machine.Player1)
        {
            Machine.CurrentlyPlaying = Machine.Player2;
        }
        else
        {
            Machine.CurrentlyPlaying = Machine.Player1;            
        }
        Debug.Log(Machine.CurrentlyPlaying + " now playing");
        await Task.Delay(100);
        Machine.ChangeTo<PieceSelectionState>();
    }
}
