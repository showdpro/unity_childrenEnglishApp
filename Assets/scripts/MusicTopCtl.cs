using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Xml;
using UnityEngine.UI;
using DG.Tweening;
using UnityEngine.EventSystems;

[RequireComponent(typeof(ScrollRect))]
public class MusicTopCtl : MonoBehaviour,IPointerEnterHandler,IPointerExitHandler{

	void Start () {
        //Debug.Log("---count:"+_count);
        _scrollRect=GetComponent<ScrollRect>();
        //
        Invoke("tweenTo",_waitTime);
	}

    void Update(){
        if(Input.GetMouseButtonUp(0)){
            Game.instance.log("===mouseUp");
            CancelInvoke("tweenTo");
            Invoke("tweenTo",_waitTime);
        }
    }

    public void OnPointerEnter(PointerEventData eventData){
        cancelTween();
        Debug.Log("===OnPointerEnter");
    }
    public void OnPointerExit(PointerEventData eventData){
        Game.instance.log("===OnPointerExit");
        CancelInvoke("tweenTo");
        Invoke("tweenTo",_waitTime);
    }


    private void tweenTo(){
        float addVal=(float)1/(float)(_count-1);
        float endVal=_id*addVal;

        float curVal=_scrollRect.horizontalScrollbar.value;
        _tween=DOTween.To(()=>curVal,x=>curVal=x,endVal,0.8f);
        _tween.OnUpdate(()=>updateTween(curVal));
        _tween.OnComplete(tweenComplete);

        if(_dir>0){
            if(_id>=(_count-1))_dir=-1;
        }else{
            if(_id<=0)_dir=1;
        }
        _id+=_dir;
    }

    private void tweenComplete(){
        Invoke("tweenTo",_waitTime);
    }

    private void updateTween(float val){
        _scrollRect.horizontalScrollbar.value=val;
    }

    private void cancelTween(){
        if(_tween!=null)_tween.Kill();
        CancelInvoke("tweenTo");
    }

    void OnEnable(){
        if(_isDisableCancelTween){
            Invoke("tweenTo",_waitTime);
            _isDisableCancelTween=false;
        }
    }

    void OnDisable(){
        _isDisableCancelTween=true;
        cancelTween();
    }

    void OnDestroy(){
        cancelTween();
    }

    public void createChildBigImage(string titleName,string imageURL,XmlElement contentXml,ContentMusicCtl contentMusicCtl){
        MusicTopBigImageCtl bigImageCtl=Instantiate(bigImagePrefab,content,false).GetComponent<MusicTopBigImageCtl>();
        bigImageCtl.initialize(titleName,imageURL,contentXml,contentMusicCtl);
        _count++;
    }

    public GameObject bigImagePrefab;
    public RectTransform content;
    private int _count;
    private int _id=1;
    private int _dir=1;
    private float _waitTime=2;
    private Tween _tween;
    private bool _isDisableCancelTween;
    private ScrollRect _scrollRect;
}
