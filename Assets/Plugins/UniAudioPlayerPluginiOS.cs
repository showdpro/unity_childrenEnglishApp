#if UNITY_IOS && !UNITY_EDITOR
using UnityEngine;
using System.Collections;
using System;
using System.Runtime.InteropServices;

public class UniAudioPlayerPlugin {
    public delegate void AudioStateChangedCallback(string state);
    public event AudioStateChangedCallback stateChangedEventHandler;

    [DllImport("__Internal")]
    private static extern void _UniAudioPlayerInitialize(AudioStateChangedCallback audioStateChangedCallback);
	[DllImport("__Internal")]
    private static extern void _UniAudioPlayerPlayUrl(string urlString);
    [DllImport("__Internal")]
    private static extern void _UniAudioPlayerPlay();
    [DllImport("__Internal")]
    private static extern void _UniAudioPlayerPause();
    [DllImport("__Internal")]
    private static extern void _UniAudioPlayerStop();
    [DllImport("__Internal")]
    private static extern void _UniAudioPlayerSeekToTime(double time);
    [DllImport("__Internal")]
    private static extern void _UniAudioPlayerSetIsLoop(bool value);
    [DllImport("__Internal")]
    private static extern bool _UniAudioPlayerIsLoop();
    [DllImport("__Internal")]
    private static extern double _UniAudioPlayerGetCurrentTime();
    [DllImport("__Internal")]
    private static extern double _UniAudioPlayerGetProgress();
    [DllImport("__Internal")]
    private static extern double _UniAudioPlayerGetDuration();
    [DllImport("__Internal")]
    private static extern float _UniAudioPlayerGetCurrentVolume();
     [DllImport("__Internal")]
    private static extern bool _UniAudioPlayerIsPlaying();
    
    public static void initialize(){
        if (Application.platform == RuntimePlatform.IPhonePlayer){
            _UniAudioPlayerInitialize(onAudioStateChangedCallback);
        }
    }

    public static void PlayUrl(string urlString){
			if (Application.platform == RuntimePlatform.IPhonePlayer){
            _UniAudioPlayerPlayUrl(urlString);
			}
    }
    
    public static void Play(){
    	if (Application.platform == RuntimePlatform.IPhonePlayer){
            _UniAudioPlayerPlay();
    	}
    }
    
    public static void Pause(){
			if (Application.platform == RuntimePlatform.IPhonePlayer){
            _UniAudioPlayerPause();
			}
   	}
   	
    public static void Stop(){
			if (Application.platform == RuntimePlatform.IPhonePlayer){
            _UniAudioPlayerStop();
			}
   	}
   	
    public static void SeekToTime(double time){
			if (Application.platform == RuntimePlatform.IPhonePlayer){
            _UniAudioPlayerSeekToTime(time);
			}
   	}
   	
    public static void SetIsLoop(bool value){
			if (Application.platform == RuntimePlatform.IPhonePlayer){
            _UniAudioPlayerSetIsLoop(value);
			}
   	}
   	
   	public static bool IsLoop(){
   		if (Application.platform == RuntimePlatform.IPhonePlayer){
            return _UniAudioPlayerIsLoop();
			}
			return false;
   	}
   	
    public static double GetCurrentTime(){
			if (Application.platform == RuntimePlatform.IPhonePlayer){
            return _UniAudioPlayerGetCurrentTime();
			}
			return 0;
   	}
   	
    public static double GetProgress(){
			if (Application.platform == RuntimePlatform.IPhonePlayer){
            return _UniAudioPlayerGetProgress();
			}
			return 0;
   	}
   	
    public static double GetDuration(){
			if (Application.platform == RuntimePlatform.IPhonePlayer){
            return _UniAudioPlayerGetDuration();
			}
			return 0;
   	}
   	
    public static float GetCurrentVolume(){
			if (Application.platform == RuntimePlatform.IPhonePlayer){
            return _UniAudioPlayerGetCurrentVolume();
			}
			return 0;
   	}
   	
   	public static bool IsPlaying(){
   		if(Application.platform==RuntimePlatform.IPhonePlayer){
            return _UniAudioPlayerIsPlaying();
   		}
   		return false;
   	}
   	
    [MonoPInvokeCallback(typeof(AudioStateChangedCallback))]
    public static void onAudioStateChangedCallback(string state){
        /*if(state=="HSU_AS_PLAYING"){

        }else if(state=="HSU_AS_PAUSED"){

        }else if(state=="HSU_AS_WAITTING"){

        }else if(state=="HSU_AS_STOPPED"){

        }else if(state=="HSU_AS_FINISHED"){

        }*/
        if(UniAudioPlayerPlugin.getInstance().stateChangedEventHandler!=null){
            UniAudioPlayerPlugin.getInstance().stateChangedEventHandler(state);
        }
    }

    public static UniAudioPlayerPlugin _instance;
    public static UniAudioPlayerPlugin getInstance(){
        if(_instance==null){
            _instance=new UniAudioPlayerPlugin();
        }
        return _instance;
    }
   	
}
#endif