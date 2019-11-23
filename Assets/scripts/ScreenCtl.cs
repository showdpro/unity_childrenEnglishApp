using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class ScreenCtl : MonoBehaviour {
    private static GameObject _go;

    public static void setPortrait(){
        Debug.Log("ScreenCtl.setPortrait");
        Screen.orientation = ScreenOrientation.Portrait;
        Screen.autorotateToPortrait           = true;
        Screen.autorotateToPortraitUpsideDown = true;
        Screen.autorotateToLandscapeRight     = false;
        Screen.autorotateToLandscapeLeft      = false;
    }

    public static void setLandscape(){
        Debug.Log("ScreenCtl.setLandscape");
        Screen.orientation = ScreenOrientation.LandscapeLeft;
        Screen.autorotateToPortrait           = false;
        Screen.autorotateToPortraitUpsideDown = false;
        Screen.autorotateToLandscapeRight     = true;
        Screen.autorotateToLandscapeLeft      = true;
    }

    private static void delayOpenAutoRotation(){
        if(_go==null)_go=new GameObject("delayOpenAutoRotation");
        _go.transform.DOLocalMoveZ(0.1f,0.2f).OnComplete(new TweenCallback(openAutoRotation));
    }
    public static void openAutoRotation(){
        Screen.orientation=ScreenOrientation.AutoRotation;
    }

    public static readonly float WIDTH=640;
    public static readonly float HEIGHT=960;

}
