using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Xml;
using UnityEngine.UI;

public class ContentMusicCtl : ContentCtl {

    override protected void Start () {
        base.Start();
		
	}

    void Update () {
        
	}

    private void init(){
        XmlDocument xml=Game.instance.getXmlDoc(Game.XmlName.MUSIC);
        XmlNodeList nodes=xml.SelectSingleNode("rootNode").ChildNodes;
        for(int i=0;i<nodes.Count;i++){
            XmlElement category=nodes[i] as XmlElement;
            if(category!=null){
                string kingBookID=category.GetAttribute("kingBookID");
                if(kingBookID=="topBigImage"){
                    createTopContent(category);
                    createHistoryContent();
                }else if(kingBookID=="scene"){
                    createSceneContent(category);
                }else if(kingBookID=="album"){
                    createAlbum(category);
                }
            }

        }
    }

    private void createTopContent(XmlElement xmlElement){
        RectTransform rectt=Instantiate(topPrefab,rootRectt,false).GetComponent<RectTransform>();
        _topContentRectt=rectt;
        _instances.Add(rectt.gameObject);
        MusicTopCtl musicTopCtl=rectt.GetComponent<MusicTopCtl>();

        XmlNodeList nodes=xmlElement.ChildNodes;
        for(int i=0;i<nodes.Count;i++){
            XmlElement bigImage=nodes[i] as XmlElement;
            if(bigImage!=null){
                string name=bigImage.GetAttribute("name");
                string bigImageURL=bigImage.GetAttribute("imageURL");
                //创建顶部项
                musicTopCtl.createChildBigImage(name,bigImageURL,bigImage,this);
            }
        }
    }

    public void createHistoryContent(){
        string xmlName=Game.MUSIC_HISTORY;
        if(Game.instance.localMan.isExist(xmlName)){

            string xmlText=Game.instance.localMan.readLocalText(xmlName);
            XmlDocument xml=new XmlDocument();
            xml.LoadXml(xmlText);
            XmlNode root=xml.SelectSingleNode("rootNode");

            if(_historyContentRectt==null){
                //创建标题卡
                RectTransform historyRectt=instanceHistoryPrefab();
                historyRectt.SetSiblingIndex(_topContentRectt.GetSiblingIndex()+1);//
                _historyContentRectt=historyRectt.Find("Scroll View/Viewport/Content").GetComponent<RectTransform>();
            }else{
                FuncUtil.destroyChilds(_historyContentRectt,true);
            }
            XmlNodeList childNodes=root.SelectNodes("item");
            for(int i=0;i<childNodes.Count;i++){
                XmlElement item=childNodes[i] as XmlElement;
                if(item!=null){
                    string categoryName=item.GetAttribute("categoryTitle");
                    string itemName=item.GetAttribute("name");
                    string iconURL=item.GetAttribute("iconURL");
                    string contentURL=item.GetAttribute("contentURL");

                    XmlElement categoryXml=getCategoryXmlWithName(categoryName,Game.XmlName.MUSIC);
                    ContentIconCtl iconCtl=Instantiate(historyContentIconPrefab,_historyContentRectt,false).GetComponent<ContentIconCtl>();
                    iconCtl.initialize(categoryName,0,itemName,iconURL,contentURL,ContentIconCtl.LinkType.MUSIC_DO_PLAY_HISTORY,categoryXml);
                }
            }
        }
    }

    private void createSceneContent(XmlElement xmlElement){
        RectTransform rectt=Instantiate(sceneContentPrefab,rootRectt,false).GetComponent<RectTransform>();
        _instances.Add(rectt.gameObject);

        //创建标题卡
        string categoryName=xmlElement.GetAttribute("name");
        MusicSceneContentCtl sceneContentCtl=rectt.GetComponent<MusicSceneContentCtl>();
        sceneContentCtl.setTitleTip(categoryName);

        XmlNodeList nodes=xmlElement.ChildNodes;
        for(int i=0;i<nodes.Count;i++){
            XmlElement sceneItem=nodes[i] as XmlElement;
            if(sceneItem!=null){
                string sceneItemName=sceneItem.GetAttribute("name");
                string iconURL=sceneItem.GetAttribute("iconURL");
                //创建场景项
                sceneContentCtl.createChildItem(sceneItemName,iconURL,sceneItem,this);
            }
        }
    }

    private void createAlbum(XmlElement xmlElement){
        //创建标题卡
        string categoryName=xmlElement.GetAttribute("name");
        RectTransform titleTipRectt=Instantiate(albumTitleTipPrefab,rootRectt,false).GetComponent<RectTransform>();
        _instances.Add(titleTipRectt.gameObject);
        Text titleTxt=titleTipRectt.Find("Text").GetComponent<Text>();
        titleTxt.text=categoryName;

        XmlNodeList nodes=xmlElement.ChildNodes;
        for(int i=0;i<nodes.Count;i++){
            XmlElement album=nodes[i] as XmlElement;
            if(album!=null){
                string albumName=album.GetAttribute("name");
                string tip=album.GetAttribute("tip");
                string iconURL=album.GetAttribute("iconURL");
                //创建专辑
                RectTransform rectt=Instantiate(albumPrefab,rootRectt,false).GetComponent<RectTransform>();
                _instances.Add(rectt.gameObject);
                MusicAlbumCtl musicAlbumCtl=rectt.GetComponent<MusicAlbumCtl>();
                musicAlbumCtl.initialize(albumName,tip,iconURL,album,this);
            }
        }
    }

    public override void setActive(bool value) {
        if(_instances==null)return;
        base.setActive(value);
        if(value && (_flags&e_isInitialize)==0){
            _flags|=e_isInitialize;
            init();
        }
    }
	
    /**点击打开内容窗口*/
	public void openContentWindow(string titleName,XmlElement contentXml){
        windowCtl.deactiveCanvasCategory();
        CanvasMusicContentWindowCtl musicContentWindowCtl=Instantiate(canvasMusicContentWindowPrefab).GetComponent<CanvasMusicContentWindowCtl>();
        musicContentWindowCtl.initialize(titleName,contentXml);
    }

    protected override XmlElement getCategoryXmlWithName(string categoryTitle, Game.XmlName xmlName){
        XmlDocument xml=Game.instance.getXmlDoc(xmlName);
        XmlNodeList categoryNodes=xml.SelectSingleNode("rootNode").ChildNodes;
        for(int i=0;i<categoryNodes.Count;i++){
            XmlElement category=categoryNodes[i] as XmlElement;
            if(category!=null){
                string categoryKingBookId=category.GetAttribute("kingBookID");
                if(categoryKingBookId=="topBigImage"){
                    XmlNodeList bigImageNodes=category.ChildNodes;
                    for(int j=0;j<bigImageNodes.Count;j++){
                        XmlElement bigImage=bigImageNodes[j] as XmlElement;
                        if(bigImage!=null){
                            string bigImageName=bigImage.GetAttribute("name");
                            if(bigImageName==categoryTitle)return bigImage;
                        }
                    }
                }else if(categoryKingBookId=="scene"){
                    XmlNodeList sceneNodes=category.ChildNodes;
                    for(int j=0;j<sceneNodes.Count;j++){
                        XmlElement scene=sceneNodes[j] as XmlElement;
                        if(scene!=null){
                            string sceneName=scene.GetAttribute("name");
                            if(sceneName==categoryTitle)return scene;
                        }
                    }
                }else if(categoryKingBookId=="album"){
                    XmlNodeList albumNodes=category.ChildNodes;
                    for(int j=0;j<albumNodes.Count;j++){
                        XmlElement album=albumNodes[j] as XmlElement;
                        if(album!=null){
                            string albumName=album.GetAttribute("name");
                            if(albumName==categoryTitle)return album;
                        }
                    }
                }

            }
        }
        return null;
    }


    public GameObject sceneContentPrefab;//场景区
    public GameObject albumPrefab;
    public GameObject albumTitleTipPrefab;
    public GameObject historyContentIconPrefab;
    public WindowCtl windowCtl;
    public GameObject canvasMusicContentWindowPrefab;

    private RectTransform _historyContentRectt;
    private RectTransform _topContentRectt;
   
}
