using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;
using DG.Tweening;

public class MainScreenUtility : MonoBehaviour
{
    ShareButton sb;

    private void Start(){
        sb = this.GetComponent<ShareButton>();
        Debug.Log(GoogleAds.getSceneChange);
        Camera.main.gameObject.transform.DOLocalMoveY(0, 0.25f).From(-10);

    }

    private void Update() {
        ButtonOnTouch();
    }

    void ButtonOnTouch(){
        if(Input.touchCount > 0 && Input.GetTouch(0).phase == TouchPhase.Began){ 
           RaycastHit2D hitInfo = Physics2D.GetRayIntersection(Camera.main.ScreenPointToRay(Input.GetTouch(0).position));
            if(hitInfo.transform != null){
                if(hitInfo.transform.name == "Play(Button)"){
                    PlayButtonPressed();
                }
                if(hitInfo.transform.name == "Rank(Button)"){
                    //TODO Add google play rank
                    //RankButtonPressed();
                    hitInfo.transform.gameObject.GetComponent<TextMeshPro>().text = "Coming Soon";
                }
                if(hitInfo.transform.name == "Share(Button)"){
                    ShareButtonPressed();
                }
            }
        }
    }

    public void PlayButtonPressed(){
        Camera.main.gameObject.transform.DOLocalMoveY(-10, 0.25f).OnComplete(() => {
            GoogleAds.getSceneChange+=1;
            SceneManager.LoadScene(1);
        });
    }
    public void RankButtonPressed(){
        //TODO
    }
    public void ShareButtonPressed(){
        sb.OnAndroidTextSharingClick();
    }
}
