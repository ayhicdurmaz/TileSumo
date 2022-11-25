using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class Animations : MonoBehaviour
{

    [SerializeField] Ease setEase;
    private bool animDone = false;

    private void Start(){
        DOTween.Init();
    }

    public void GridAnimation(GameObject _tile){
        _tile.transform.DOScale(new Vector3(_tile.transform.localScale.x, _tile.transform.localScale.y, 1), 0.1f).SetEase(setEase).From(new Vector3(0.3f, 0.3f, 0.3f)).OnKill( () => { animDone = true;} );

    }

    public void TileOnTouch(GameObject _tile){
        if(animDone){
            _tile.transform.DOPunchScale(new Vector3(0.1f,0.1f,0.1f), 0.5f);
        }
    }

    public void GameScreenUIRight(GameObject _ui, float _t){
        _ui.transform.DOLocalMoveX(_ui.transform.localPosition.x, _t).SetEase(Ease.InOutCubic).From(new Vector3(_ui.transform.localPosition.x - 1000, _ui.transform.localPosition.y, _ui.transform.localPosition.z));
    }

    public void GameScreenUILeft(GameObject _ui, float _t){
        _ui.transform.DOLocalMoveX(_ui.transform.localPosition.x, _t).SetEase(Ease.InOutCubic).From(new Vector3(_ui.transform.localPosition.x + 1000, _ui.transform.localPosition.y, _ui.transform.localPosition.z));
    }

    public void GameScreenUIUp(GameObject _ui, float _t){
                _ui.transform.DOLocalMoveY(_ui.transform.localPosition.y, _t).SetEase(Ease.InOutCubic).From(new Vector3(_ui.transform.localPosition.x, _ui.transform.localPosition.y + 1000, _ui.transform.localPosition.z));

    }
}
