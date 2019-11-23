using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class IntoVideo : MonoBehaviour {

    void Awake(){
        ScreenCtl.setPortrait();
    }

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    public void gotoVideo(){
        
        SceneManager.LoadSceneAsync("video");
    }
}
