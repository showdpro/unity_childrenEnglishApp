using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Xml;
/**音乐场景,专辑打开的内容列表窗口*/
public class CanvasMusicContentWindowCtl : MonoBehaviour {

	void Start () {
        
	}
	
	void Update () {
        
	}

    public void initialize(string titleName,XmlElement xml){
        titleTxt.text=titleName;
        _contentXml=xml;

        int id=0;
        XmlNodeList nodes=xml.ChildNodes;
        for(int i=0;i<nodes.Count;i++){
            XmlElement element=nodes[i] as XmlElement;
            if(element!=null){
                string name=element.GetAttribute("name");
                string iconURL=element.GetAttribute("iconURL");
                string contentURL=element.GetAttribute("contentURL");
                ContentIconCtl contentIconCtl=Instantiate(itemPrefab,content,false).GetComponent<ContentIconCtl>();
                contentIconCtl.initialize(titleName,id,name,iconURL,contentURL,ContentIconCtl.LinkType.MUSIC,(RectTransform)transform);
                id++;
            }
        }
    }

    public void onClickBackBtn(){
        Destroy(gameObject);

        WindowCtl windowCtl=GameObject.Find("WindowManager").GetComponent<WindowCtl>();
        windowCtl.activeCanvasCategory();

        ContentMusicCtl contentMusicCtl=GameObject.Find("ContentManager").GetComponent<ContentMusicCtl>();
        contentMusicCtl.createHistoryContent();
    }

    public XmlElement contentXml{
        get{return _contentXml;}
    }

    public RectTransform content;
    public Text titleTxt;
    public GameObject itemPrefab;
    public XmlElement _contentXml;
}
