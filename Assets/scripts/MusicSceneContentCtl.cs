using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Xml;

public class MusicSceneContentCtl : MonoBehaviour {

	void Start () {
		
	}
	
	void Update () {
		
	}

    public RectTransform createChildItem(string sceneItemName,string iconURL,XmlElement contentXml,ContentMusicCtl contentMusicCtl){
        RectTransform rectt=Instantiate(childItemPrefab,content,false).GetComponent<RectTransform>();
        MusicSceneChildItemCtl musicSceneChildItemCtl=rectt.GetComponent<MusicSceneChildItemCtl>();
        musicSceneChildItemCtl.initialize(sceneItemName,iconURL,contentXml,contentMusicCtl);
        return rectt;
    }

    public void setTitleTip(string text){
        titleTipTxt.text=text;
    }

    public Text titleTipTxt;
    public RectTransform content;
    public GameObject childItemPrefab;
}
