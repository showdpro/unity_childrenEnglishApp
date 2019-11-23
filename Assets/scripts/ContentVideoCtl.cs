using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Xml;
using UnityEngine.UI;

public class ContentVideoCtl : ContentCtl {

	override protected void Start () {
        base.Start();
        FuncUtil.destroyChilds(rootRectt,true);
        //顶部内容
        createTopContent();
        //创建网页链接
        createLinkPageBar();
        //历史内容
        createHistoryContent();
        //分类内容
        createCategoryContent();

        _flags|=e_isInitialize;
	}

    private void createTopContent(){
        RectTransform rectt=instanceRecardSizeNode();
        _topScrollParent=rectt;
        _topRectt=instanceTopPrefab(rectt);
    }

    protected override void swapTopToFixedNode() {
         _topRectt.SetParent(topFixedRectt);
        Vector3 pos=_topRectt.localPosition;
        pos.y=topFixedY;
        _topRectt.localPosition=pos;
    }
    protected override void swapTopToScrollNode() {
        _topRectt.SetParent(_topScrollParent);
    }

    private RectTransform createCategoryTitle(string text){
        RectTransform rectt=instanceRecardSizeNode();
        rectt=instanceCategoryTitlePrefab(rectt);
        Text txt=rectt.Find("Text").GetComponent<Text>();
        txt.text=text;
        return rectt;
    }

    public void createHistoryContent(){
        string xmlName=Game.VIDEO_HISTORY;
        if(Game.instance.localMan.isExist(xmlName)){
            
            string xmlText=Game.instance.localMan.readLocalText(xmlName);
            XmlDocument xml=new XmlDocument();
            xml.LoadXml(xmlText);
            XmlNode root=xml.SelectSingleNode("rootNode");

            if(_historyContentRectt==null){
                //创建标题卡
                string tipTitle="History";
                if(Game.instance.language==SystemLanguage.ChineseSimplified)tipTitle="历史";
                else if(Game.instance.language==SystemLanguage.ChineseTraditional)tipTitle="歷史";
                else if(Game.instance.language==SystemLanguage.Chinese)tipTitle="历史";
                Transform titleParentRectt=createCategoryTitle(tipTitle).parent;
                if(_linkPageBarRectt!=null){
                    titleParentRectt.SetSiblingIndex(_linkPageBarRectt.GetSiblingIndex()+1);//历史标题 放在linkPageBar下面
                }else{
                    titleParentRectt.SetSiblingIndex(_topScrollParent.GetSiblingIndex()+1);//历史标题 放在顶部内容滚动容器的下面
                }
                RectTransform historyRectt=instanceHistoryPrefab();
                historyRectt.SetSiblingIndex(titleParentRectt.GetSiblingIndex()+1);//
                _historyContentRectt=historyRectt.Find("Viewport/Content").GetComponent<RectTransform>();
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

                    XmlElement categoryXml=getCategoryXmlWithName(categoryName,Game.XmlName.VIDEO);
                    ContentIconCtl iconCtl=Instantiate(historyContentIconPrefab,_historyContentRectt,false).GetComponent<ContentIconCtl>();
                    iconCtl.initialize(categoryName,0,itemName,iconURL,contentURL,ContentIconCtl.LinkType.VIDEO_DO_PLAY_HISTORY,categoryXml);
                }
            }
        }
    }

    private void createLinkPageBar(){
        XmlDocument xml=Game.instance.getXmlDoc(Game.XmlName.VIDEO);
        XmlElement linkPage=xml.SelectSingleNode("rootNode").SelectSingleNode("linkPage") as XmlElement;
        Debug.Log(linkPage==null);
        if(linkPage!=null){
            string iconURL=linkPage.GetAttribute("iconURL");
            string text=linkPage.GetAttribute("text");
            bool isLandscape=linkPage.GetAttribute("isLandscape")=="true";
            string linkURL=linkPage.GetAttribute("linkURL");
            LinkPageBar linkPageBar=Instantiate(linkPageBarPrefab,rootRectt,false).GetComponent<LinkPageBar>();
            _instances.Add(linkPageBar.gameObject);
            linkPageBar.initialize(iconURL,text,isLandscape,linkURL);
            _linkPageBarRectt=linkPageBar.GetComponent<RectTransform>();
        }
    }

    private void createCategoryContent(){
        XmlDocument xml=Game.instance.getXmlDoc(Game.XmlName.VIDEO);
        XmlNodeList nodeList=xml.SelectSingleNode("rootNode").SelectNodes("category");

        for(int i=0;i<nodeList.Count;i++){
            XmlElement category=nodeList[i] as XmlElement;
            if(category!=null){
                string categoryName=category.GetAttribute("name");//分类名
                createCategoryTitle(categoryName);
                
                RectTransform rectt=instanceCategoryPrefab();
                for(int j=0;j<category.ChildNodes.Count;j++){
                    XmlElement series=category.ChildNodes[j] as XmlElement;
                    if(series!=null){
                        string seriesName=series.GetAttribute("name");
                        string seriesIconURL=series.GetAttribute("iconURL");

                        ContentIconCtl seriesIconCtl=Instantiate(categoryContentIconPrefab,rectt,false).GetComponent<ContentIconCtl>();
                        seriesIconCtl.initialize(categoryName,0,seriesName,seriesIconURL,string.Empty,ContentIconCtl.LinkType.VIDEO,series);
                    }
                }
            }
        }
    }

    protected override XmlElement getCategoryXmlWithName(string categoryTitle, Game.XmlName xmlName){
        XmlDocument xml=Game.instance.getXmlDoc(xmlName);
        XmlNodeList nodeList=xml.SelectSingleNode("rootNode").ChildNodes;
        for(int i=0;i<nodeList.Count;i++){
            XmlElement category=nodeList[i] as XmlElement;
            if(category!=null){
                for(int j=0;j<category.ChildNodes.Count;j++){
                    XmlElement series=category.ChildNodes[j] as XmlElement;
                    if(series!=null){
                        string seriesName=series.GetAttribute("name");
                        if(seriesName==categoryTitle)return series;
                    }
                }
            }
        }
        return null;
    }

    public GameObject historyContentIconPrefab;
    public GameObject categoryContentIconPrefab;
    public GameObject linkPageBarPrefab;
    public float topFixedY;
    private RectTransform _topRectt;
    private RectTransform _topScrollParent;//顶部内容在滚动容器中的父级节点
    private RectTransform _historyContentRectt=null;
    private RectTransform _linkPageBarRectt;
}
