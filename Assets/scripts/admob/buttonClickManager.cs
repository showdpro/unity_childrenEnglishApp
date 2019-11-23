using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class buttonClickManager:MonoBehaviour {
    public AdManager adManager;
    private bool _isLandscape;

    void Start(){
        /*ScreenCtl.setPortrait();
        _isLandscape=false;*/
    }

    public void onClickRotateScreen(){
        _isLandscape=!_isLandscape;
        if(_isLandscape)ScreenCtl.setPortrait();
        else ScreenCtl.setLandscape();
    }

    public void onClickLoadInterstitial(){
        adManager.loadIntersitial();
    }

    public void onClickShowInterstital(){
        adManager.showIntersitial();
    }

    public void onDropdownValueChanged(Dropdown dropdown){
        switch(dropdown.value){
            case 0://google
                adManager.isCallGoogle=true;
                adManager.isCallGdt=false;
                break;
            case 1://gdt
                adManager.isCallGoogle=false;
                adManager.isCallGdt=true;
                break; 
            case 2://auto
                adManager.isCallGoogle=true;
                adManager.isCallGdt=true;
                break;
        }
    }
}
