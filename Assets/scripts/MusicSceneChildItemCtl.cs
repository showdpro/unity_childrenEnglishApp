using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Xml;

public class MusicSceneChildItemCtl : MonoBehaviour {

	void Start () {
        ImageLoader imageLoader=gameObject.AddComponent<ImageLoader>();
        imageLoader.initialize(iconImage,_titleName,_iconURL);
	}
	
	void Update () {
		
	}

    public void initialize(string titleName,string iconURL,XmlElement contentXml,ContentMusicCtl contentMusicCtl){
        _titleName=titleName;
        titleTxt.text=titleName;
        _contentXml=contentXml;
        _contentMusicCtl=contentMusicCtl;
        _iconURL=iconURL;
    }

    public void onClick(){
        _contentMusicCtl.openContentWindow(_titleName,_contentXml);
    }

    public Image iconImage;
    public Text titleTxt;
    private ContentMusicCtl _contentMusicCtl;
    private XmlElement _contentXml;
    private string _titleName;
    private string _iconURL;
}
