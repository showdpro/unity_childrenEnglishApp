using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class testGDt : MonoBehaviour {

    public GraphicRaycaster raycaster;
    public AdManager adManager;

    void Start(){
        raycaster.enabled=true;
    }

    public void onLoad(){
        Debug.Log("===onLoad");
        adManager.loadIntersitial();
    }

    public void onShow(){
        Debug.Log("===onShow");
        adManager.showIntersitial();
    }
}
