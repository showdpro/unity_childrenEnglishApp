using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LinkPageBar : MonoBehaviour {

    public ImageLoader iconLoader;
    public Text txt;
    public GameObject canvas_game_player_prefab;

    private bool _isLandscape;
    private string _linkURL;

	void Start () {
		
	}
	
	void Update () {
		
	}

    public void initialize(string iconURL,string text,bool isLandscape,string linkURL){
        txt.text=text;
        _isLandscape=isLandscape;
        _linkURL=linkURL;
        iconLoader.initialize(iconLoader.GetComponent<Image>(),"linkPageBarIcon",iconURL,false);
    }

    public void onClick(){
        WindowCtl windowCtl=GameObject.Find("WindowManager").GetComponent<WindowCtl>();
        windowCtl.deactiveCanvasCategory();//隐藏分类主界面
        CanvasGamePlayerWindowCtl gamePlayerWindowCtl=Instantiate(canvas_game_player_prefab).GetComponent<CanvasGamePlayerWindowCtl>();
        gamePlayerWindowCtl.initialize(_isLandscape,_linkURL,windowCtl);
        //
        waitScreenRotationShowInterstitial();
    }

    private void waitScreenRotationShowInterstitial(){
        Invoke("doShowInterstitial",1.5f);
    }
    private void doShowInterstitial(){
        AdManager.instance.showIntersitial();
    }


}
