using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class GameManager : MonoBehaviour
{
    [SerializeField] public Color[] gameColorLight = new Color[6];
    //[SerializeField] Color[] gameColorDark; Maybe i will add later

    //Grid Settings
    [SerializeField] private GameObject tilePrefabs;
    [SerializeField] private float gapRateBetween, gapRateScreen, gridDimension, yOffset; 
    public GameObject[] tiles;

    //Tile Creation Values
    [SerializeField] public int desiredNumTileCount, numTileCount, creationIndex, numberInTile, pressCounter = 0;
    MatrixGenerator mg;

    //Adding;
    public int sum, iterationValue = 3, minScoreToUpgradeIterationVal = 50, max = 9, min = 1;

    //Score and Time
    [SerializeField] private TMP_Text scoreText, timeText, scoreEndText, highScoreText;

    public float score, highScore, timer = 16, extraTime = 3f;
    public bool isGameOver = false;

    //Animations
    Animations anim;
    [SerializeField] GameObject textTime, textScore, btnPause;
    public bool startGame = false;

    private void Start(){
        Camera.main.backgroundColor = gameColorLight[0];
        tiles = new GameObject[(int)Mathf.Pow(gridDimension, 2)]; 
        mg = this.GetComponent<MatrixGenerator>(); 
        anim = this.GetComponent<Animations>();
        mg.FullMatrixInit(min, max);
        highScore = PlayerPrefs.GetInt("HighScore");
        anim.GameScreenUIRight(scoreText.gameObject, 1);
        anim.GameScreenUILeft(timeText.gameObject, 1);
        anim.GameScreenUIUp(textTime.gameObject, 0.5f);
        anim.GameScreenUIUp(textScore.gameObject, 0.5f);
        anim.GameScreenUIUp(btnPause.gameObject, 0.5f);
        StartCoroutine(GridMaker());
    }

    void Update(){
        if(!isGameOver && startGame){
            CreateTiles();
            TileOnTouch();
            ScoreAndTimeController();
        }     
    }

    public IEnumerator GridMaker(){
        float xCoord = GetPhoneBorder().x;
        float x = (xCoord*2)/(gridDimension + ((gridDimension-1)/gapRateBetween) + (2/gapRateScreen));
        int _middleTile = (int)((Mathf.Pow(gridDimension,2)-1)/2);
        int k = 0;
        for(float i = ((((gridDimension-1)/2) * (x/gapRateBetween)) + ((gridDimension-1)/2)*x); k != Mathf.Pow(gridDimension, 2) ; i -= ( x + x/gapRateBetween )){
            for(float j = ((x/gapRateScreen)+(x/2))-xCoord; j < xCoord; j+=(x+x/gapRateBetween)){
                tiles[k] = Instantiate(tilePrefabs, new Vector3(j, i+ yOffset , 0), Quaternion.Euler(0f, 0f, 0f), this.transform);
                tiles[k].name = k.ToString();
                tiles[k].GetComponent<Tile>().TileInit(gameColorLight[2], x, false);
                if(k == _middleTile){
                    sum = mg.GetMainNumber(iterationValue);
                    tiles[_middleTile].GetComponent<Tile>().UpdateTileSettings(gameColorLight[5], gameColorLight[0], 7f, sum, desiredNumTileCount+1, true);
                }
                anim.GridAnimation(tiles[k]);
                k++;
                yield return new WaitForSeconds(0.05f);
            }
        }
        startGame = true;
        yield return null;
    }

    void CreateTiles(){
        Tile scrptTile;
        while(numTileCount < desiredNumTileCount){
            scrptTile = tiles[Random.Range(0,tiles.GetLength(0))].GetComponent<Tile>();
            if(!scrptTile.isInteractable){
                if(pressCounter>0){
                    mg.UpdateMatrix(creationIndex, min, max);
                }
                scrptTile.UpdateTileSettings(gameColorLight[3], gameColorLight[0], 7f, mg.SetTileNumber(creationIndex), creationIndex, true);
                anim.TileOnTouch(scrptTile.gameObject);
                numTileCount++;
                creationIndex++;           
            }
        }
    }

    void TileOnTouch(){
        if(Input.touchCount > 0 && Input.GetTouch(0).phase == TouchPhase.Began){ 
           RaycastHit2D hitInfo = Physics2D.GetRayIntersection(Camera.main.ScreenPointToRay(Input.GetTouch(0).position));
            if(hitInfo.transform != null && hitInfo.transform.parent.gameObject == this.gameObject){
                if(hitInfo.transform.name !=  ((Mathf.Pow(gridDimension, 2)-1)/2).ToString()){
                    Tile scrptTile = hitInfo.transform.GetComponent<Tile>();
                    if(scrptTile.isInteractable){
                        int[] t = scrptTile.TileOnTouch();
                        numberInTile = t[0];
                        creationIndex = t[1];
                        sum -= numberInTile;
                        numTileCount--;
                        pressCounter++;
                        tiles[(int)((Mathf.Pow(gridDimension,2)-1)/2)].GetComponent<Tile>().UpdateTileSettings(gameColorLight[5], gameColorLight[0], 7f, sum, desiredNumTileCount+1, true);
                        anim.TileOnTouch(scrptTile.gameObject);
                    }
                }
            }
        }
    }

    public void ScoreAndTimeController(){
        if(iterationValue < (mg.depth*desiredNumTileCount)){
            if(score > minScoreToUpgradeIterationVal){
                iterationValue++;
                min++;
                max++;
                minScoreToUpgradeIterationVal += minScoreToUpgradeIterationVal/(iterationValue-3);
            }
        }
        if(sum == 0){
            score += 10*((float)iterationValue/(float)pressCounter);
            sum = mg.GetMainNumber(iterationValue);
            pressCounter = 0;
            timer += 3f;
            tiles[(int)((Mathf.Pow(gridDimension,2)-1)/2)].GetComponent<Tile>().UpdateTileSettings(gameColorLight[5], gameColorLight[0], 7f, sum, desiredNumTileCount+1, true);

        }else if(sum < 0){
            isGameOver = true;
        }
        if(timer<0){
            isGameOver = true;
        }else{
            timer -= Time.deltaTime;
        }
        if(isGameOver){
            if(score>highScore){
                PlayerPrefs.SetInt("HighScore", (int)score);
            }
            scoreEndText.text = "SCORE\n" + (int)score;
            highScoreText.text = "BEST\n" + PlayerPrefs.GetInt("HighScore");
        }else{
            scoreText.text = ((int)score).ToString();
            timeText.text = ((int)timer) + "sn";
        }
    }

    Vector3 GetPhoneBorder(){
        return Camera.main.ScreenToWorldPoint(new Vector3(Screen.width, Screen.height, 0));
    }
}
