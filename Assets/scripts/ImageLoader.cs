using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ImageLoader : MonoBehaviour {

    private Image _imageIcon;
    private string _title;
    private string _iconURL;
    private bool _isLoaded;
    private bool _isTryLoad=false;
    private bool _isErrorDelete;

    public void initialize(Image imageIcon,string title,string iconURL,bool isErrorDelete=true){
        _imageIcon=imageIcon;
        _title=title;
        _iconURL=iconURL;
        _isErrorDelete=isErrorDelete;
    }

    private void loadIcon(){
        if(_isLoaded)return;
        _isLoaded=true;
        StartCoroutine(loadImage(_iconURL));
    }

    IEnumerator loadImage(string iconURL){
        WWW www=new WWW(iconURL);
        yield return www;
        if(string.IsNullOrEmpty(www.error)){
            Texture2D tex=www.texture;
            _imageIcon.sprite=Sprite.Create(tex,new Rect(0,0,tex.width,tex.height),new Vector2(0.5f,0.5f));
        }else{
            if(_isTryLoad){
                string errorStr="Error loading icon deleted:\n";
                errorStr+="name:"+_title+"\n";
                errorStr+="iconURL:"+iconURL+"\n";
                errorStr+="==============================";
                Game.instance.log(errorStr);
                Destroy(gameObject);
            }else{
                _isTryLoad=true;
                StartCoroutine(loadImage(_iconURL));
            }
        }
    }
	
    void LateUpdate () {
        if(!_isLoaded){
            RectTransform rectt=this.GetComponent<RectTransform>();
            float w=rectt.rect.width*0.01f;
            float h=rectt.rect.height*0.01f;
            float lowerx=rectt.position.x+rectt.rect.x*0.01f;
            float lowery=rectt.position.y+rectt.rect.y*0.01f;
            float upperx=lowerx+w;
            float uppery=lowery+h;
            //Game.W=640,Game.H=960设计尺寸大小
            bool isIntoX=lowerx<(Game.W*0.5f*0.01f);
            bool isIntoY=uppery>(-Game.H*0.5f*0.01f);
            if(isIntoX&&isIntoY){
                loadIcon();
            }
        }
    }



}
