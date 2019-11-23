using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class testRectf : MonoBehaviour {
    private RectTransform rectt;
	// Use this for initialization
	void Start () {
        rectt=this.GetComponent<RectTransform>();
    }
	
	// Update is called once per frame
	void Update () {
        Debug.Log(rectt.childCount);
	}
}
