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
    
    private void Awake(){
        
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);            
            
        }else if (Instance != this)
        {
            Destroy(gameObject);
        }        
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }    

    public void ChangeLevelGolden(string level){

        LevelGolden = int.Parse(level);
        Debug.Log("Nível de dificuldade Golden: " + LevelGolden);

    }

    public void ChangeLevelGreen(string level){

        LevelGreen = int.Parse(level);
        Debug.Log("Nível de dificuldade Green: " + LevelGreen);

    }

    public void ChangeAIPlayer1(string activated){
        
        AIControlledPlayer1 = bool.Parse(activated);      
        //PlayerPrefs.SetString("AICONTROLLED_PLAYER1", AIControlledPlayer1.ToString());

    }

    public void ChangeAIPlayer2(string activated){
        
        AIControlledPlayer2 = bool.Parse(activated);       

    }

    public void GameOver(){
                
        GameOverCanvas.SetTrigger("GameOver");

        if (GameControl.Instance != null)
        {
            GameControl.Instance.Score += ScoreGolden;            
            GameControl.Instance.ScoreTotalText.text = "Score: " + GameControl.Instance.Score;
            if (ScoreGolden > GameControl.Instance.MaxScore)
            {
                GameControl.Instance.MaxScore = ScoreGolden;                
                PlayerPrefs.SetString("RECORD", GameControl.Instance.MaxScore.ToString());
                //var maxScore = PlayerPrefs.GetString("RECORD");
                GameControl.Instance.MaxScoreText.text = "Record: " + ScoreGolden;
            }
        }
    } 

    public void UpdateScore(Piece pe){

        int scoreDirection;        
        if (StateMachineController.Instance.CurrentlyPlaying == StateMachineController.Instance.Player1)
        {
            scoreDirection = 1;
            var positionValue = pe.Movement.PositionValue[pe.tile.pos];
            ScoreGolden += (pe.Movement.PieceWeight + positionValue) * scoreDirection;               
            ScoreTextGolden.text = "Golden Score: " + ScoreGolden;
        }
        else
        {
            scoreDirection = 1;
            var positionValue = pe.Movement.PositionValue[pe.tile.pos];
            ScoreGreen += (pe.Movement.PieceWeight + positionValue) * scoreDirection;               
            ScoreTextGreen.text = "Green Score: " + ScoreGreen;
        }        
        
        //Debug.Log("Score: " + Score);
    }

    public void ReloadScene(){

        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);

    }
}
