using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SwapButtonImage : MonoBehaviour {
    public Image image;
    public Texture2D offTexture;
    public Texture2D onTexture;
    public bool isOn;

	void Start () {
        image = GetComponent<Image>();
        swapTo(isOn);
	}
	
	void Update () {
		
	}

    public void onClick(){
        isOn = !isOn;
        swapTo(isOn);
    }

    public void swapTo(bool tmpIsOn){
        isOn=tmpIsOn;
        Rect rect = image.sprite.rect;
        Vector2 pivot = image.sprite.pivot;
        Sprite sp = Sprite.Create(isOn?onTexture:offTexture,rect,pivot);
        image.sprite = sp;
    }
}
