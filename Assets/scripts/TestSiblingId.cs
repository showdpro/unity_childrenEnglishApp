using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TestSiblingId : MonoBehaviour {
    public Image a;
    public Image b;


	// Use this for initialization
	void Start () {
        Debug.Log(a.transform.GetSiblingIndex());
        Debug.Log(b.transform.GetSiblingIndex());
        b.transform.SetSiblingIndex(0);
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
