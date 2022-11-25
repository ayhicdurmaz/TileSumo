using UnityEngine;
using TMPro;

public class Tile : MonoBehaviour
{
    [SerializeField] private SpriteRenderer tileTexture;
    [SerializeField] private TMP_Text tileText;

    private GameManager gm;
    public float fontSize = 0f;    
    public int tileNumber, creationIndex;
    public bool isInteractable;


    public void TileInit(Color _tileTextureColor, float _scale, bool _isInteractable){
        gm = this.GetComponentInParent<GameManager>();
        tileTexture.color = _tileTextureColor;
        tileText.text = "";
        this.transform.localScale = new Vector3(_scale, _scale, 1f);
        isInteractable = _isInteractable;
    }

    public void UpdateTileSettings(Color _tileTextureColor, Color _tileTextColor, float _fontSize, int _tileNumber, int _creationIndex, bool _isInteractable){
        tileTexture.color = _tileTextureColor;
        tileText.color = _tileTextColor;
        tileText.fontSize = _fontSize;
        tileNumber = _tileNumber;
        tileText.text = tileNumber.ToString();
        creationIndex = _creationIndex;
        isInteractable = _isInteractable;
    }

    public int[] TileOnTouch(){
        int[] _number = {tileNumber, creationIndex};
        UpdateTileSettings(gm.gameColorLight[2], gm.gameColorLight[2], 7, 0, 0, false); // ? oyunun içinde karar verilecek
        return _number;
    }
}
