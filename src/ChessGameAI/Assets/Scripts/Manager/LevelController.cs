using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class LevelController : MonoBehaviour
{
    public static LevelController Instance;
    public Animator GameOverCanvas;
    public int Level;    
    public bool AIControlledPlayer1;
    public bool AIControlledPlayer2;
    public Text ScoreText;
    public int Score;        
    
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

    public void ChangeLevel(string level){

        Level = int.Parse(level);
        Debug.Log("Nível de dificuldade: " + Level);

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
            GameControl.Instance.Score += Score;
            if (Score > GameControl.Instance.MaxScore)
            {
                GameControl.Instance.MaxScore = Score;
            }
        }
    } 

    public void UpdateScore(Piece pe){

        int scoreDirection;        
        if (StateMachineController.Instance.CurrentlyPlaying == StateMachineController.Instance.Player1)
        {
            scoreDirection = 1;
        }
        else
        {
            scoreDirection = -1;
        }        
        var positionValue = pe.Movement.PositionValue[pe.tile.pos];
        Score += (pe.Movement.PieceWeight + positionValue) * scoreDirection;               
        ScoreText.text = "Pontos: " + Score;
        //Debug.Log(Score);
    }

    public void ReloadScene(){

        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);

    }
}
