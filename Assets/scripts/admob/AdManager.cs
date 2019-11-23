using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using admob;
using gdtad;

public class AdManager : MonoBehaviour {
    public bool isTesting;
    public GoogleAdManager googleAdManager;
    public GdtAdManager gdtAdManager;

    //如果两个都为true,优先调用google,如果加载google失败再调广点通,如果加载广点通失败,则又调google
    public bool isCallGoogle=true;//是否调用google广告
    public bool isCallGdt;//是否调用广点通广告

    public static AdManager instance {
        get{
            return GameObject.Find("AdManager").GetComponent<AdManager>();
        }
    }

    void Awake(){
        //不需要调用的广告把其gameObject吊销不让初始化浪费资源
        //if(!isCallGoogle)googleAdManager.gameObject.SetActive(false);
        //if(!isCallGdt)gdtAdManager.gameObject.SetActive(false);
    }

	void Start () {
        gameObject.name="AdManager";
        //
        if(isCallGoogle){
            googleAdManager.interstitialEventHandler+=onGoogleInterstitialEvent;
        }
        if(isCallGdt){
            gdtAdManager.interstitialEventHandler+=onGdtInterstitialEvent;
        }
	}
	
	void Update () {
		
	}

    private void onGoogleInterstitialEvent(string eventName, string msg){
        if(eventName==AdmobEvent.onAdFailedToLoad){
            //加载google插屏广告失败
            gdtAdManager.loadInterstitial();
        }else if(eventName==AdmobEvent.onAdClosed){
            //关闭后重新加载
            loadIntersitial();
        }
    }
    private void onGdtInterstitialEvent(string eventName, string msg){
        if(eventName==GdtAdEvent.onAdFailedToLoad){
            //加载广点通插屏广告失败
            googleAdManager.loadInterstitial();
        }else if(eventName==GdtAdEvent.onAdClosed){
            //关闭后重新加载
            loadIntersitial();
        }
    }

    public void loadIntersitial(){
        if(isCallGoogle&&isCallGdt){
            googleAdManager.loadInterstitial();
        }else{
            if(isCallGoogle)   googleAdManager.loadInterstitial();
            else if (isCallGdt)gdtAdManager.loadInterstitial();
        }
    }

    public void showIntersitial(){
        if(isCallGoogle&&isCallGdt){
            if(googleAdManager.isInterstitialReady()){
                googleAdManager.showInterstitial();
            }else if(gdtAdManager.isInterstitialReady()){
                gdtAdManager.showInterstitial();
            }else{
                loadIntersitial();
            }
        }else{
            if(isCallGoogle){
                if(googleAdManager.isInterstitialReady()){
                    googleAdManager.showInterstitial();
                }else{
                    loadIntersitial();
                }
            }else if(isCallGdt){
                if(gdtAdManager.isInterstitialReady()){
                    gdtAdManager.showInterstitial();
                }else{
                    loadIntersitial();
                }
            }
        }
    }


}
