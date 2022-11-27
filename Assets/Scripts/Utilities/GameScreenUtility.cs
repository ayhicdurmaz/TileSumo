using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using DG.Tweening;

public class GameScreenUtility : MonoBehaviour
{
    [SerializeField] private GameObject[] gameObjects; //gameScreen = 0, pauseScreen = 1, gameOverScreen = 2, HomeScreen = 3

    public bool isOnPause = false, isRunned = false;
    GameManager gm;
    
    public void Start(){
        Debug.Log(Singleton.getSceneChange);
        gm = this.GetComponent<GameManager>();
    }

    public void Update(){
        ButtonOnTouch();
        if(gm.isGameOver && !isRunned){
            Camera.main.gameObject.transform.DOLocalMoveX(-20, 0.5F);
            isRunned = true;
        }
    }

    void ButtonOnTouch(){
        if(Input.touchCount > 0 && Input.GetTouch(0).phase == TouchPhase.Began){ 
           RaycastHit2D hitInfo = Physics2D.GetRayIntersection(Camera.main.ScreenPointToRay(Input.GetTouch(0).position));
           Debug.Log(hitInfo.transform.name);
            if(hitInfo.transform != null){
                if(hitInfo.transform.name == "Continue(Button)" || hitInfo.transform.name == "WatchAd(Button)"){
                    Debug.Log("wahtcad");
                    WatchAdToContinueButtonPressed();
                }
                if(hitInfo.transform.name == "Pause(Button)"){
                    PauseButtonPressed();
                }
                if(hitInfo.transform.name == "Resume(Button)"){
                    ResumeButtonPressed();
                }
                if(hitInfo.transform.name == "Home(Button)"){
                    HomeButtonPressed();
                }
                if(hitInfo.transform.name == "Replay(Button)"){
                    ReplayButtonPressed();
                }
            }
        }
    }

    void PauseButtonPressed(){
        Camera.main.gameObject.transform.DOLocalMoveX(20, 0.5f).OnComplete(()=>{
            isOnPause = true;
            Time.timeScale = 0;
        });
    }

    void ResumeButtonPressed(){
        Time.timeScale = 1;
        Camera.main.gameObject.transform.DOLocalMoveX(0, 0.5f).OnComplete(() => {
            isOnPause = false;
        }); 
    }

    void HomeButtonPressed(){
        Camera.main.gameObject.transform.DOLocalMoveY(10, 0.25f).OnComplete(() => {
            Singleton.getSceneChange+=1;
            SceneManager.LoadScene(0);
            Time.timeScale = 1;
        });
        
    }

    void ReplayButtonPressed(){
        Singleton.isFromReplay = true;
        Camera.main.gameObject.transform.DOLocalMoveX(-10, 0.25f).OnComplete(() => {
            Singleton.getSceneChange+=1;
            SceneManager.LoadScene(1);
        });
    }

    void WatchAdToContinueButtonPressed(){
        Singleton.isFromReplay = true;
        Camera.main.gameObject.transform.DOLocalMoveX(-10, 0.25f).OnComplete(() => {
            gm.Continue();
        });
    }
}
