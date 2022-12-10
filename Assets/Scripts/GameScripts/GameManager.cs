using System.Collections;
using UnityEngine;
using TMPro;
using DG.Tweening;

public class GameManager : MonoBehaviour
{
    [SerializeField] public Color[] gameColorLight = new Color[6];

    //Grid Settings
    [SerializeField] private GameObject tilePrefabs;
    [SerializeField] private float gapRateBetween, gapRateScreen, gridDimension, yOffset; 
    public GameObject[] tiles;

    //Tile Creation Values
    [SerializeField] public int desiredNumTileCount, numTileCount, creationIndex, numberInTile, pressCounter = 0;
    MatrixGenerator mg;
    GameScreenUtility gsu;

    //Adding;
    public int sum, iterationValue = 3, minScoreToUpgradeIterationVal = 50, max = 9, min = 1;

    //Score and Time
    [SerializeField] private TMP_Text scoreText, timeText, scoreEndText, highScoreText;

    public float score, highScore, timer = 16, extraTime = 3f;
    public bool isGameOver = false, isGameStartable = false;
    
    //Sound
    [SerializeField] private AudioSource tileSound;


    private void Start(){
        tileSound.mute = PlayerPrefs.GetInt("Mute") == 1 ? true : false; 
        Camera.main.backgroundColor = gameColorLight[0];
        if(Singleton.isFromReplay){
            Debug.Log("İt is here");
            Camera.main.gameObject.transform.DOLocalMoveX(0, 0.25f).From(-10).OnComplete(() => {
                tiles = new GameObject[(int)Mathf.Pow(gridDimension, 2)]; 
                mg = this.GetComponent<MatrixGenerator>(); 
                gsu = this.GetComponent<GameScreenUtility>(); 
                mg.FullMatrixInit(min, max);
                highScore = PlayerPrefs.GetInt("HighScore");
                StartCoroutine(GridMaker());
            });
        }else{
            Camera.main.gameObject.transform.DOLocalMoveY(0, 0.25f).From(10).SetEase(Ease.InOutBounce).OnComplete(() => {
                tiles = new GameObject[(int)Mathf.Pow(gridDimension, 2)]; 
                mg = this.GetComponent<MatrixGenerator>(); 
                gsu = this.GetComponent<GameScreenUtility>(); 
                mg.FullMatrixInit(min, max);
                highScore = PlayerPrefs.GetInt("HighScore");
                StartCoroutine(GridMaker());
            });
        }
    }

    public void Continue(){
        Camera.main.gameObject.transform.DOLocalMoveX(0, 0.25f).From(-10).OnComplete( () => {
            creationIndex = 0;
            numTileCount = 0;
            tiles = new GameObject[(int)Mathf.Pow(gridDimension, 2)];
            timer = 15f;
            isGameStartable = false;
            pressCounter = 0;
            mg.FullMatrixInit(min, max);
            highScore = PlayerPrefs.GetInt("HighScore");
            StartCoroutine(GridMaker());        
            isGameOver = false;
            gsu.isRunned = false;
        });
    }

    void Update(){
        if(!isGameOver && isGameStartable && !gsu.isOnPause){
            CreateTiles();
            TileOnTouch();
            ScoreAndTimeController();
        }
        if(isGameOver){
            ScreenClearer();
        }
    }

    void PlaySound(){
        tileSound.mute = PlayerPrefs.GetInt("Mute") == 1 ? true : false;  
        tileSound.Play();
    }

    public void ScreenClearer(){
        foreach(GameObject _tile in tiles){
            Destroy(_tile, 1f);
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
                tiles[k].transform.DOScale(new Vector3(x,x,1), 0.1f).From(0).SetEase(Ease.OutBounce);
                k++;
                yield return new WaitForSeconds(0.025f);
            }
        }
        isGameStartable = true;
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
                scrptTile.transform.DOPunchScale(new Vector3(0.05f,0.05f,0.05f), 0.5f);
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
                        scrptTile.transform.DOPunchScale(new Vector3(0.05f,0.05f,0.05f), 0.5f);
                        PlaySound();
                        int[] t = scrptTile.TileOnTouch();
                        numberInTile = t[0];
                        creationIndex = t[1];
                        sum -= numberInTile;
                        numTileCount--;
                        pressCounter++;
                        tiles[(int)((Mathf.Pow(gridDimension,2)-1)/2)].GetComponent<Tile>().UpdateTileSettings(gameColorLight[5], gameColorLight[0], 7f, sum, desiredNumTileCount+1, true);
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
