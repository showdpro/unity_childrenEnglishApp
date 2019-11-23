using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Xml;

public class MusicTopBigImageCtl : MonoBehaviour {

    void Start(){
        _image=GetComponent<Image>();
        ImageLoader imageLoader=gameObject.AddComponent<ImageLoader>();
        imageLoader.initialize(_image,_titleName,_imageURL);
    }

	public void initialize(string titleName,string imageURL,XmlElement contentXml,ContentMusicCtl contentMusicCtl){
        _titleName=titleName;
        _imageURL=imageURL;
        _contentXml=contentXml;
        _contentMusicCtl=contentMusicCtl;
    }

    public void onClick(){
        _contentMusicCtl.openContentWindow(_titleName,_contentXml);
    }
    private Image _image;
    private string _imageURL;
    private string _titleName;
    private XmlElement _contentXml;
    private ContentMusicCtl _contentMusicCtl;
    private bool _isLoaded;

}
