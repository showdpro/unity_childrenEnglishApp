using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CanvasGamePlayerWindowCtl : MonoBehaviour {
    
	void Start () {
        canvasScaler=GetComponent<CanvasScaler>();
        canvas=GetComponent<Canvas>();

        Camera uiCamera=GameObject.Find("UICamera").GetComponent<Camera>();
        canvas.worldCamera=uiCamera;

        uniWebView.url=_contentURL;
        uniWebView.gameObject.SetActive(true);
        
		uniWebView.Load();
	}
	
	void Update () {
		
	}

    public void initialize(bool isLandscape,string contentURL,WindowCtl windowCtl){
        setScreen(isLandscape);
        if(isLandscape){
            canvasScaler.referenceResolution=new Vector2(ScreenCtl.HEIGHT,ScreenCtl.WIDTH);
            loadingUIBgImage.sprite=landscapeBg;


        }

        #if UNITY_EDITOR||UNITY_ANDROID
        if(isLandscape){
            backBtnPortrait.SetActive(false);
            backBtnLandscape.SetActive(true);
        }else{
            backBtnPortrait.SetActive(true);
            backBtnLandscape.SetActive(false);
        }
        #endif


        _contentURL=contentURL;
        _windowCtl=windowCtl;
        uniWebView.gameObject.SetActive(false);
        uniWebView.OnLoadBegin+=onLoadBegin;
        uniWebView.OnLoadComplete+=onLoadComplete;
        uniWebView.OnClickBack+=onClickBack;
        uniWebView.alpha=0.4f;
    }

    private void onLoadBegin(UniWebView webView,string loadingUrl){
        
    }

    private void onLoadComplete(UniWebView webView, bool success, string errorMessage){
        loadingUIBgImage.gameObject.SetActive(false);
    }

    private void setScreen(bool isLandscape){
        if(isLandscape){
            canvasScaler.referenceResolution=new Vector2(ScreenCtl.HEIGHT,ScreenCtl.WIDTH);
            ScreenCtl.setLandscape();
        }else{
            canvasScaler.referenceResolution=new Vector2(ScreenCtl.WIDTH,ScreenCtl.HEIGHT);
            ScreenCtl.setPortrait();
        }
    }

    private void onClickBack(){
        setScreen(false);
        _windowCtl.activeCanvasCategory();
        uniWebView.OnLoadBegin-=onLoadBegin;
        uniWebView.OnLoadComplete-=onLoadComplete;
        uniWebView.OnClickBack-=onClickBack;
		uniWebView.Stop();
        Destroy(gameObject);

        ContentGameCtl contentGameCtl=GameObject.Find("ContentManager").GetComponent<ContentGameCtl>();
        contentGameCtl.createHistoryContent();
    }

    public CanvasScaler canvasScaler;
    public Canvas canvas;
    public UniWebView uniWebView;
    public GameObject backBtnPortrait;
    public GameObject backBtnLandscape;

    public Image loadingUIBgImage;
    public Sprite portraitBg;
    public Sprite landscapeBg;

    private string _contentURL;
    private WindowCtl _windowCtl;
}
