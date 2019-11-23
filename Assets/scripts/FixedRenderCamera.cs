using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[RequireComponent(typeof(Canvas))]
public class FixedRenderCamera : MonoBehaviour {

	void Start () {
        Camera uiCamera=GameObject.Find("UICamera").GetComponent<Camera>();

        Canvas canvas=GetComponent<Canvas>();
        canvas.worldCamera=uiCamera;
	}
	
	void Update () {
		
	}
}
