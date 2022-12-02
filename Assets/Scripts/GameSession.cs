using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;
public class GameSession : MonoBehaviour
{

    [SerializeField] int playerLives = 3;
    [SerializeField] int score = 0;
    
    [SerializeField] TextMeshProUGUI livestext;
    [SerializeField] TextMeshProUGUI scorestext;

    void Awake()
    {
        int numGameSession = FindObjectsOfType<GameSession>().Length;
        if (numGameSession >1 )
        {
            Destroy(gameObject);
        }
        else
        {
            DontDestroyOnLoad(gameObject);
        }
        
    }

    void Start(){
        livestext.text = playerLives.ToString();
        scorestext.text=score.ToString();
    }

   public void ProcessPlayerDeath(){
    if (playerLives > 1)
    {
        TakeLife();
    }
    else
    {
        ResetGameSession();
    }
   }

    public void AddScore(int pointsToAdd){
        score+=pointsToAdd;
        scorestext.text=score.ToString();

    }

     void TakeLife()
    {
        playerLives --;
        int currentSceneIndex = SceneManager.GetActiveScene().buildIndex;
        SceneManager.LoadScene(currentSceneIndex);
        livestext.text = playerLives.ToString();
    }

     void ResetGameSession()
    {
        FindObjectOfType<ScenePersist>().ResetScenePersist();
        SceneManager.LoadScene(0);
        Destroy(gameObject);
    }
}
