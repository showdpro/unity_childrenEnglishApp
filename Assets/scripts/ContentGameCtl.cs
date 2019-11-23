using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Xml;
using UnityEngine.UI;

public class ContentGameCtl : ContentCtl {

    override protected void Start () {
        base.Start();
		
	}

    void Update () {

    }

    private void init(){
        createTopContent();
        createHistoryContent();
        createCategoryContent();
    }

    private void createTopContent(){
        RectTransform rectt=instanceRecardSizeNode();
        _topScrollParent=rectt;
        _topRectt=instanceTopPrefab(rectt);
    }

    public void createHistoryContent(){
        string xmlName=Game.GAME_HISTORY;
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

                titleParentRectt.SetSiblingIndex(_topScrollParent.GetSiblingIndex()+1);//历史标题 放在顶部内容滚动容器的下面
                RectTransform historyRectt=instanceHistoryPrefab();
                historyRectt.SetSiblingIndex(titleParentRectt.GetSiblingIndex()+1);//
                _historyContentRectt=historyRectt.Find("Viewport/Content").GetComponent<RectTransform>();
            }else{
                FuncUtil.destroyChilds(_historyContentRectt,true);
            }

            int id=0;
            XmlNodeList childNodes=root.SelectNodes("item");
            for(int i=0;i<childNodes.Count;i++){
                XmlElement item=childNodes[i] as XmlElement;
                if(item!=null){
                    string categoryName=item.GetAttribute("categoryTitle");
                    string itemName=item.GetAttribute("name");
                    string iconURL=item.GetAttribute("iconURL");
                    string contentURL=item.GetAttribute("contentURL");
                    bool isLandscape=item.GetAttribute("isLandscape")=="true";

                    ContentIconCtl iconCtl=Instantiate(historyContentIconPrefab,_historyContentRectt,false).GetComponent<ContentIconCtl>();
                    iconCtl.initialize(categoryName,id,itemName,iconURL,contentURL,ContentIconCtl.LinkType.GAME,isLandscape);
                    id++;
                }
            }
        }
    }

    private void createCategoryContent(){
        XmlDocument xml=Game.instance.getXmlDoc(Game.XmlName.GAME);
        XmlNodeList nodeList=xml.SelectSingleNode("rootNode").ChildNodes;
        for(int i=0;i<nodeList.Count;i++){
            XmlElement category=nodeList[i] as XmlElement;
            if(category!=null){
                string categoryName=category.GetAttribute("name");
                createCategoryTitle(categoryName);

                int id=0;
                RectTransform rectt=instanceCategoryPrefab();
                for(int j=0;j<category.ChildNodes.Count;j++){
                    XmlElement item=category.ChildNodes[j] as XmlElement;
                    if(item!=null){
                        string itemName=item.GetAttribute("name");
                        string iconURL=item.GetAttribute("iconURL");
                        string contentURL=item.GetAttribute("contentURL");
                        bool isLandscape=item.GetAttribute("isLandscape")=="true";

                        ContentIconCtl iconCtl=Instantiate(categoryContentIconPrefab,rectt,false).GetComponent<ContentIconCtl>();
                        iconCtl.initialize(categoryName,id,itemName,iconURL,contentURL,ContentIconCtl.LinkType.GAME,isLandscape);
                        id++;
                    }
                }
            }
        }
    }

    private RectTransform createCategoryTitle(string text){
        RectTransform rectt=instanceRecardSizeNode();
        rectt=instanceCategoryTitlePrefab(rectt);
        Text txt=rectt.Find("Text").GetComponent<Text>();
        txt.text=text;

        return rectt;
    }

    public override void setActive(bool value) {
        if(_instances==null)return;
        base.setActive(value);
        if(value && (_flags&e_isInitialize)==0){
            _flags|=e_isInitialize;
            init();
        }
    }
	
    public GameObject categoryContentIconPrefab;
    public GameObject historyContentIconPrefab;
    private RectTransform _topRectt;
    private RectTransform _topScrollParent;//顶部内容在滚动容器中的父级节点
    private RectTransform _historyContentRectt;

}
