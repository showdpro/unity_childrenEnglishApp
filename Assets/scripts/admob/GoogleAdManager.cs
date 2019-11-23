using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using admob;

public class GoogleAdManager : MonoBehaviour {
    public string bannerID="ca-app-pub-3940256099942544/2934735716";
    public string fullID="ca-app-pub-3940256099942544/4411468910";
    public string rewardVideoID="ca-app-pub-3940256099942544/xxxxxxxxxx";
    public string nativeBannerID="ca-app-pub-3940256099942544/2562852117";
    public bool isTesting=false;

    public delegate void AdmobEventHandler(string eventName, string msg);

    public event AdmobEventHandler bannerEventHandler;
    public event AdmobEventHandler interstitialEventHandler;
    public event AdmobEventHandler rewardedVideoEventHandler;
    public event AdmobEventHandler nativeBannerEventHandler;
    Admob ad;

    public static GoogleAdManager instance {
        get{
            return GameObject.Find("GoogleAdManager").GetComponent<GoogleAdManager>();
        }
    }

    void Awake(){
        
    }
	
	void Start () {
        gameObject.name="GoogleAdManager";
        initAdmob();
	}

    private void initAdmob(){
        //isAdmobInited = true;
        ad = Admob.Instance();
        ad.bannerEventHandler += onBannerEvent;
        ad.interstitialEventHandler += onInterstitialEvent;
        ad.rewardedVideoEventHandler += onRewardedVideoEvent;
        ad.nativeBannerEventHandler += onNativeBannerEvent;
        ad.initAdmob(bannerID,fullID);
        if(isTesting)ad.setTesting(true);
        ad.setGender(AdmobGender.MALE);
        string[] keywords = { "game","crash","male game"};
        ad.setKeywords(keywords);
        Debug.Log("admob inited -------------");
    }

    private void onInterstitialEvent(string eventName, string msg){
        if(interstitialEventHandler!=null)interstitialEventHandler(eventName,msg);
    }
    private void onBannerEvent(string eventName, string msg){
        if(bannerEventHandler!=null)bannerEventHandler(eventName,msg);

    }
    private void onRewardedVideoEvent(string eventName, string msg){
        if(rewardedVideoEventHandler!=null)rewardedVideoEventHandler(eventName,msg);
    }
    private void onNativeBannerEvent(string eventName, string msg){
        if(nativeBannerEventHandler!=null)nativeBannerEventHandler(eventName,msg);
    }

    public bool isInterstitialReady(){
        return ad.isInterstitialReady();
    }
    public void loadInterstitial(){
        ad.loadInterstitial();
    }

    public void showInterstitial(){
        ad.showInterstitial();
    }

    public void showRewardVideo(){
        if (ad.isRewardedVideoReady()) ad.showRewardedVideo();
        else ad.loadRewardedVideo(rewardVideoID);
    }

    public void showbanner(AdSize size/*=AdSize.SmartBanner*/, int pos/*=AdPosition.BOTTOM_CENTER*/,int marginY=0){
        Admob.Instance().showBannerRelative(size, pos,marginY);
    }
    public void showbannerABS(AdSize size/*=AdSize.Banner*/,int x=0,int y=300){
        Admob.Instance().showBannerAbsolute(size, x, y);
    }
    public void removebanner(){
        Admob.Instance().removeBanner();
    }

    public void showNative(int pos/*=AdPosition.BOTTOM_CENTER*/,int width=320,int height=120,int marginY=0){
        Admob.Instance().showNativeBannerRelative(new AdSize(width,height), pos, marginY,nativeBannerID);
    }
    public void showNativeABS(int width=320,int height=120,int x=0,int y=0){
        Admob.Instance().showNativeBannerAbsolute(new AdSize(width,height), x, y, nativeBannerID);
    }
    public void removeNative(){
        Admob.Instance().removeNativeBanner();
    }
}
