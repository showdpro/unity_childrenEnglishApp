using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using UnityEngine.EventSystems;

public class MusicSeekbarCtl : MonoBehaviour,IPointerDownHandler, IPointerUpHandler, IPointerEnterHandler, IPointerExitHandler, IDragHandler{

    void Start () {
        
    }


    void Update () {
        #if UNITY_IOS && !UNITY_EDITOR
        if(!_isPress){
            slider.value=(float)UniAudioPlayerPlugin.GetProgress();
        }
        #else
        if(!_isPress&&_isSeekComplete&&audioSource.isPlaying&&audioSource.clip!=null){
            slider.value=audioSource.time/audioSource.clip.length;
        }
        #endif
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
        setTime(slider.value);
    }


    public void OnDrag(PointerEventData eventData){
        //setTime(slider.value);
    }

    /**value=0~1*/
    private void setTime(float value){
        #if UNITY_IOS && !UNITY_EDITOR
        UniAudioPlayerPlugin.SeekToTime((double)value*UniAudioPlayerPlugin.GetDuration());
        #else
        if(audioSource.clip==null)return;
        audioSource.time=audioSource.clip.length*value;
        audioSource.Play();
        #endif
    }


    public Slider slider;
    public AudioSource audioSource;
    private bool _isSeekComplete=true;
    private bool _isPress=false;
}
