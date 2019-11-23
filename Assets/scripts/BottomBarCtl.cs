using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BottomBarCtl : MonoBehaviour {

	void Start () {
        images=new Image[]{videoImage,musicImage,webGameImage};
	}
	
	void Update () {
		
	}

    public void onClickVideoBtn(){
        showBubble(0);
        contentVideoCtl.setActive(true);
        contentMusicCtl.setActive(false);
        contentGameCtl.setActive(false);
    }

    public void onClickMusicBtn(){
        showBubble(1);
        contentVideoCtl.setActive(false);
        contentMusicCtl.setActive(true);
        contentGameCtl.setActive(false);
    }

    public void onClickWebGameBtn(){
        showBubble(2);
        contentVideoCtl.setActive(false);
        contentMusicCtl.setActive(false);
        contentGameCtl.setActive(true);
    }

    private void showBubble(int id){
        for(int i=0;i<images.Length;i++){
            images[i].enabled=id==i;
        }
    }

    public ContentVideoCtl contentVideoCtl;
    public ContentMusicCtl contentMusicCtl;
    public ContentGameCtl contentGameCtl;
    public Image videoImage;
    public Image musicImage;
    public Image webGameImage;
    private Image[] images;
}
