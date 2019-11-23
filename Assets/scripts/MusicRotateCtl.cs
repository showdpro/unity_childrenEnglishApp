using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MusicRotateCtl : MonoBehaviour {

	void Start () {
		
	}
	
	void Update () {
        _angle+=rotateV;
        _angle%=360;
        imageTransform.localRotation=Quaternion.Euler(0,0,_angle*Mathf.Rad2Deg);
	}

    public Transform imageTransform;
    public float rotateV;

    private float _angle=0;
}
