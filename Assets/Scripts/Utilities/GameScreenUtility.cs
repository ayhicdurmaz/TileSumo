using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameScreenUtility : MonoBehaviour
{
    [SerializeField] private GameObject[] gameObjects; //gameScreen = 0, pauseScreen = 1, gameOverScreen = 1
    GameManager gm;
    
    public void Start(){
        gm = this.GetComponent<GameManager>();
    }

    public void Update(){
        if(gm.isGameOver){
            gameObjects[2].SetActive(true);
            gameObjects[1].SetActive(true);
        }
    }

    public void PauseButtonPressed(){
        gameObjects[0].SetActive(false);
        gameObjects[1].SetActive(true);
        Time.timeScale = 0;
    }

    public void ResumeButtonPressed(){
        gameObjects[1].SetActive(false);
        gameObjects[0].SetActive(true);
        Time.timeScale = 1;
    }

    public void HomeButtonPressed(){
        SceneManager.LoadScene(0);
        Time.timeScale = 1;
    }

    public void ReplayButtonPressed(){
        SceneManager.LoadScene(1);
    }


}
