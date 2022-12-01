using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;
using DG.Tweening;

public class MainScreenUtility : MonoBehaviour
{
    [SerializeField] private AudioSource buttonSound;
    private bool isMute;
    ShareButton sb;

    private void Start(){
        isMute = PlayerPrefs.GetInt("Mute") == 1 ? true : false;
        GameObject.Find("Mute(Button)").GetComponent<TextMeshPro>().text = isMute ? "unmute" : "mute";
        buttonSound.mute = isMute;
        sb = this.GetComponent<ShareButton>();
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
                if(hitInfo.transform.name == "Mute(Button)"){
                    MuteButtonPressed(hitInfo.transform.GetComponent<TextMeshPro>());
                }
            }
        }
    }

    public void PlayButtonPressed(){
        buttonSound.Play();
        Singleton.isFromReplay = false;
        Camera.main.gameObject.transform.DOLocalMoveY(-10, 0.25f).OnComplete(() => {
            Singleton.getSceneChange+=1;
            SceneManager.LoadScene(1);
        });
    }
    public void RankButtonPressed(){
        //TODO
    }
    public void ShareButtonPressed(){
        buttonSound.Play();
        sb.OnAndroidTextSharingClick();
    }
    public void MuteButtonPressed(TextMeshPro _text){
        isMute = !isMute;
        buttonSound.mute = isMute;  
        PlayerPrefs.SetInt("Mute", isMute ? 1 : 0);      
        if(PlayerPrefs.GetInt("Mute") == 1 ? true : false){
            _text.text = "unmute";
        }else{
            _text.text = "mute";
        }
    }
}
