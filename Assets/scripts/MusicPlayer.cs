using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[RequireComponent(typeof(AudioSource))]
public class MusicPlayer : MonoBehaviour {

	void Start () {
		_audioSource=GetComponent<AudioSource>();
        #if UNITY_IOS && !UNITY_EDITOR
        UniAudioPlayerPlugin.initialize();
        UniAudioPlayerPlugin.getInstance().stateChangedEventHandler+=onStateChanged;
        #endif
	}
	
	void Update () {
        #if UNITY_IOS && !UNITY_EDITOR

        #else
        if(_audioSource.clip){
            if(_audioSource.isPlaying&&!_audioSource.loop){
                if(_audioSource.time>=_audioSource.clip.length*0.999f){
                    _audioSource.time=_audioSource.clip.length;
                    _audioSource.Stop();
                }
            }
        }
        #endif
	}

    void OnDestroy(){
        #if UNITY_IOS && !UNITY_EDITOR
        UniAudioPlayerPlugin.Stop();
        #else

        #endif
    }

    private void onStateChanged(string state){
        #if UNITY_IOS && !UNITY_EDITOR
        Debug.Log("===audioPlayerState:"+state);
        _state=state;
        if(state==UniAudioStateEvent.HSU_AS_WAITTING){
            
        }
        #endif
    }

    public void playURL(string url){
        #if UNITY_IOS && !UNITY_EDITOR
        if(_state!=UniAudioStateEvent.HSU_AS_WAITTING){
            UniAudioPlayerPlugin.PlayUrl(url);
        }
        #else
        StartCoroutine(loadMusic(url));
        #endif
    }

    IEnumerator loadMusic(string url){
        WWW www=new WWW(url);
        yield return www;
        if(string.IsNullOrEmpty(www.error)){
            _audioSource.clip=www.GetAudioClip();
            _audioSource.time=0;
            _audioSource.Play();
        }else{
            Debug.Log("load: "+url+" error!");
        }
    }

    public void autoPlayOrPause(){
        #if UNITY_IOS && !UNITY_EDITOR
            if(isPlaying){
            	UniAudioPlayerPlugin.Pause();
            }else{
            	UniAudioPlayerPlugin.Play();
            }
        #else
        if(isPlaying){
            _audioSource.Pause();
        }else{
            float progress=_audioSource.time/_audioSource.clip.length;
            if(progress>0.9999f){//播放完成,从头播放
                _audioSource.time=0;
            }
            _audioSource.Play();
        }
        #endif
    }

    public bool isPlaying{
        get{
        	#if UNITY_IOS && !UNITY_EDITOR
        	return UniAudioPlayerPlugin.IsPlaying();
        	#else
        	return _audioSource.isPlaying;
        	#endif
        }
    }

    public int curClipLength{
        get{
        	#if UNITY_IOS && !UNITY_EDITOR
        	return (int)UniAudioPlayerPlugin.GetDuration();
        	#else
          int timeLen=_audioSource.clip!=null?(int)_audioSource.clip.length:0;
          return timeLen;
          #endif
        }
    }
		
    public int curTime{
        get{
        	#if UNITY_IOS && !UNITY_EDITOR
        	return (int)(UniAudioPlayerPlugin.GetProgress()*UniAudioPlayerPlugin.GetDuration());
        	#else
        	return (int)_audioSource.time;
        	#endif
        }
    }
		
    public bool loop{
        #if UNITY_IOS && !UNITY_EDITOR
        set{UniAudioPlayerPlugin.SetIsLoop(value);}
        get{return UniAudioPlayerPlugin.IsLoop();}
        #else
        set{_audioSource.loop=value;}
        get{return _audioSource.loop;}
        #endif
    }

    private AudioSource _audioSource;
    private string _state;
}
