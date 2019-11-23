using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VideoViewCtl : MonoBehaviour {
    public CanvasVideoPlayerWindowCtl videoCtl;
    private float _lastTime=0;
	// Use this for initialization
	void Start () {
        
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    public void onClick(){
        
        if (Time.time - _lastTime <= 0.5f){//双击
            videoCtl.autoScreenOrRest();
        }
        _lastTime = Time.time;
    }
}
