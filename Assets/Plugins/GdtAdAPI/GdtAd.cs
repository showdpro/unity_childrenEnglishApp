namespace gdtad{
    using UnityEngine;
    using System.Runtime.InteropServices;
    public class GdtAd{
        public delegate void GdtAdEventHandler(string eventName, string msg);
        public event GdtAdEventHandler interstitialEventHandler;

        public static GdtAd _instance;
        public static GdtAd getInstance(){
            if(_instance==null){
                _instance=new GdtAd();
            }
            return _instance;
        }
        #if UNITY_EDITOR
        public void initIntertitial(string appkey, string intertitialID){
            Debug.Log("===Gdt initIntertitial");
        }
        public void loadInterstitial(){
            Debug.Log("===Gdt loadInterstitial");
        }
        public void showInterstitial(){
            Debug.Log("===Gdt showInterstitial");
        }
        public bool isInterstitialReady(){
            Debug.Log("===Gdt isInterstitialReady");
            return false;
        }
        #elif UNITY_IOS
        internal delegate void GdtAdCallBack(string adtype, string eventName, string msg);
        public void initIntertitial(string appkey, string intertitialID){
            _UniGdtInitInterstitial(appkey,intertitialID,onGdtAdEventCallBack);
        }
        public void loadInterstitial(){
            _UniGdtLoadInterstitial();
        }

        public void showInterstitial(){
            _UniGdtShowInterstitial();
        }

        public bool isInterstitialReady(){
            return _UniGdtIsInterstitialReady();
        }

        //
        //

        [DllImport("__Internal")]
        private static extern void _UniGdtInitInterstitial(string appkey, string interstitialID,GdtAdCallBack gdtCallback);
        [DllImport("__Internal")]
        private static extern void _UniGdtLoadInterstitial();
        [DllImport("__Internal")]
        private static extern void _UniGdtShowInterstitial();
        [DllImport("__Internal")]
        private static extern bool _UniGdtIsInterstitialReady();
        [MonoPInvokeCallback(typeof(GdtAdCallBack))]
        public static void onGdtAdEventCallBack(string adtype, string eventName, string msg){
            if(adtype=="interstitial"){
                if(GdtAd.getInstance().interstitialEventHandler!=null)
                    GdtAd.getInstance().interstitialEventHandler(eventName, msg);
            }
        }
        #elif UNITY_ANDROID
        //
        #endif

    }
}
