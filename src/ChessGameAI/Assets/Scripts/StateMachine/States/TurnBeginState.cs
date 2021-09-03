using System.Threading.Tasks;
using UnityEngine;

public class TurnBeginState : State
{
    public override async void EnterAsync()
    {
        //Debug.Log("Turn Begin:");
        if (Machine.CurrentlyPlaying == Machine.Player1)
        {
            // var aiControlled = PlayerPrefs.GetString("AICONTROLLED_PLAYER2");
            // Machine.Player2.AIControlled = bool.Parse(aiControlled);
            Machine.CurrentlyPlaying = Machine.Player2;
        }
        else
        {
            // var aiControlled = PlayerPrefs.GetString("AICONTROLLED_PLAYER1");
            // Machine.Player1.AIControlled = bool.Parse(aiControlled);
            Machine.CurrentlyPlaying = Machine.Player1;            
        }
        //Debug.Log(Machine.CurrentlyPlaying + " now playing");
        await Task.Delay(100);
        if (Machine.CurrentlyPlaying.AIControlled)
        {
            Machine.ChangeTo<AIPlayingState>();
        }
        else
        {
            Machine.ChangeTo<PieceSelectionState>();            
        }        
    }
}
