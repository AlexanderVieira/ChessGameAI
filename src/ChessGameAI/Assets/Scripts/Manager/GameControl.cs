using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;

public class GameControl : MonoBehaviour
{
    public static GameControl Instance;    
    public int Score;    
    public int MaxScore;
    public Text ScoreTotalText;
    public Text MaxScoreText;
    public Text RecordText;


    void Awake()
    {

        if (Instance == null)
        {
            Instance = this;
            //DontDestroyOnLoad(gameObject);
        }
        else if (Instance != this)
        {
            Destroy(gameObject);
        }

        var recordText = PlayerPrefs.GetString("RECORD");
        RecordText.text = "Record: " + recordText;

    }
    
}
