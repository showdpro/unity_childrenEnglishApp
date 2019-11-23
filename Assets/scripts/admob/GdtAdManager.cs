using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using gdtad;

public class GdtAdManager : MonoBehaviour {
    public string appkey;
    public string intertitalID;
    private bool _isDoLoadedAutoShow=false;

    GdtAd gdtAd;

    public delegate void GdtAdEventHandler(string eventName, string msg);
    public event GdtAdEventHandler interstitialEventHandler;

    public static GdtAdManager instance {
        get{
            return GameObject.Find("GdtAdManager").GetComponent<GdtAdManager>();
        }
    }

    void Start () {
        gameObject.name="GdtAdManager";
        initAd();
    }

    private void initAd(){
        gdtAd=GdtAd.getInstance();
        if(!string.IsNullOrEmpty(intertitalID)) gdtAd.initIntertitial(appkey,intertitalID);
        gdtAd.interstitialEventHandler+=onInterstitalEvent;
    }

    private void onInterstitalEvent(string eventName,string msg){
        if(interstitialEventHandler!=null)interstitialEventHandler(eventName,msg);
        if(eventName==GdtAdEvent.onAdLoaded && _isDoLoadedAutoShow){
            _isDoLoadedAutoShow=false;
            showInterstitial();
        }
    }

    public void loadInterstitial(){
        gdtAd.loadInterstitial();
    }

    public void showInterstitial(){        
       gdtAd.showInterstitial();
    }

    public bool isInterstitialReady(){
        return gdtAd.isInterstitialReady();
    }
    
    /**如果准备插屏则显示,否则加载完再显示*/
    public void autoLoadShowInterstitial(){
        if(isInterstitialReady()){
            showInterstitial();
        }else{
            loadInterstitial();
            _isDoLoadedAutoShow=true;
        }
    }


}
