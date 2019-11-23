using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.Video;

public class VideoSeekBarCtrl : MonoBehaviour,IPointerDownHandler, IPointerUpHandler, IPointerEnterHandler, IPointerExitHandler, IDragHandler{

    void Start () {
        videoPlayerWindowCtl.playEndEvent+=playEndHandler;
    }


    void Update () {
        if(!_isPress&&videoPlayerWindowCtl.isPlaying){
            float nv=(float)videoPlayer.time/getTotalTime();

            if(float.IsNaN(nv))nv=0;
            else if(nv<0)nv=0;
            else if(nv>1)nv=1;

            slider.value=nv;
        }
    }
    private float getTotalTime(){
        return (float)videoPlayer.frameCount/videoPlayer.frameRate;
    }

    public void OnPointerEnter(PointerEventData eventData){
        _isPress=true;
    }

    public void OnPointerExit(PointerEventData eventData){
        _isPress=false;
    }

    public void OnPointerDown(PointerEventData eventData){

    }

    public void OnPointerUp(PointerEventData eventData){
        videoPlayerWindowCtl.setTime(slider.value);
    }


    public void OnDrag(PointerEventData eventData){
        //易出错 
        //videoPlayerWindowCtl.setTime(slider.value);

    }

    private void playEndHandler(){
        slider.value=0;
    }

    void OnDestroy(){
        videoPlayerWindowCtl.playEndEvent-=playEndHandler;
    }

    public Slider slider;
    public VideoPlayer videoPlayer;
    public CanvasVideoPlayerWindowCtl videoPlayerWindowCtl;
    private bool _isPress=false;
}
