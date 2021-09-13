using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class LevelController : MonoBehaviour
{
    public static LevelController Instance;
    public Animator GameOverCanvas;
    public int LevelGolden;
    public int LevelGreen;
    public bool AIControlledPlayer1;
    public bool AIControlledPlayer2;
    public Text ScoreTextGolden;
    public Text ScoreTextGreen;
    public int ScoreGolden;
    public int ScoreGreen;

    private void Awake()
    {

        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);

        }
        else if (Instance != this)
        {
            Destroy(gameObject);
        }
    }    

    public void ChangeLevelGolden(string level)
    {
        LevelGolden = int.Parse(level);
        //Debug.Log("Nível de dificuldade Golden: " + LevelGolden);
    }

    public void ChangeLevelGreen(string level)
    {
        LevelGreen = int.Parse(level);
        //Debug.Log("Nível de dificuldade Green: " + LevelGreen);
    }

    public void ChangeAIPlayer1(string activated)
    {
        AIControlledPlayer1 = bool.Parse(activated);
    }

    public void ChangeAIPlayer2(string activated)
    {
        AIControlledPlayer2 = bool.Parse(activated);
    }

    public void GameOver()
    {

        GameOverCanvas.SetTrigger("GameOver");

        if (GameControl.Instance != null)
        {
            var maxScore = PlayerPrefs.GetString("RECORD");

            if (StateMachineController.Instance.CurrentlyPlaying == StateMachineController.Instance.Player1)
            {
                GameControl.Instance.Score = ScoreGolden;
            }
            else
            {
                GameControl.Instance.Score = ScoreGreen;
            }

            GameControl.Instance.ScoreTotalText.text = "Score: " + GameControl.Instance.Score;
            
            if (ScoreGolden > int.Parse(maxScore))
            {                
                PersistScore(ScoreGolden);                
            }
            else if (ScoreGreen > int.Parse(maxScore))
            {                
                PersistScore(ScoreGreen);                
            }
            else
            {                
                GameControl.Instance.MaxScoreText.text = "Record: " + maxScore;
            }

        }
    }

    private void PersistScore(int KingdomScore){

        GameControl.Instance.MaxScore = KingdomScore;
        PlayerPrefs.SetString("RECORD", GameControl.Instance.MaxScore.ToString());                
        GameControl.Instance.MaxScoreText.text = "Record: " + GameControl.Instance.MaxScore;

    }

    public void UpdateScore(Piece pe)
    {
        int scoreDirection = 1;
        if (StateMachineController.Instance.CurrentlyPlaying == StateMachineController.Instance.Player1)
        {
            var kingdomName = "Golden Score: ";
            CalculateScore(pe, kingdomName, ref ScoreGolden, scoreDirection, ref ScoreTextGolden);            
        }
        else
        {
            var kingdomName = "Green Score: ";
            CalculateScore(pe, kingdomName, ref ScoreGreen, scoreDirection, ref ScoreTextGreen);            
        }       
    }

    private void CalculateScore(Piece pe, string kingdomName, 
                                ref int kingdomScore, int scoreDirection, ref Text ScoreText){

        var positionValue = pe.Movement.PositionValue[pe.tile.pos];
        kingdomScore += (pe.Movement.PieceWeight + positionValue) * scoreDirection;
        ScoreText.text = kingdomName + kingdomScore;
    }

    public void ReloadScene()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
}
