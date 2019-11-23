using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using System.Xml;
using System;

public class Game : MonoBehaviour {

    public static Game instance{
        get{
            GameObject go=GameObject.Find("Game");
            Game game=go.GetComponent<Game>();
            return game;
        }
    }

    public static GameObject instanceObj{
        get{
            GameObject go=GameObject.Find("Game");
            return go;
        }
    }

    void Awake(){
        
    }

	void Start () {
        log(Application.persistentDataPath);
        log(Application.systemLanguage);

        #if UNITY_EDITOR
        for(int i=0;i<SceneManager.sceneCount;i++){
            if(i>0){
                Scene scene=SceneManager.GetSceneByBuildIndex(i);
                SceneManager.UnloadScene(scene);
            }
        }
        #endif

        ScreenCtl.setPortrait();
        gameObject.name="Game";

        _loader=GetComponent<Loader>();
        _localMan=GetComponent<LocalManager>();

        //1.加载xml数据
        string[] urls=new string[]{videoXmlURL,musicXmlURL,gameXmlURL};
        _xmlNames=new string[]{"videoConfig.xml","musicConfig.xml","gameConfig.xml"};
        _loader.loadXmls(urls,_xmlNames,xmlsLoaded);
	}

    private void xmlsLoaded(string[] xmlTexts){
        int count=xmlTexts.Length;
        //2.保存xml数据到本地
        for(int i=0;i<count;i++){
            _localMan.saveText(xmlTexts[i],_xmlNames[i]);
        }

        //3.生成xmlDocument
        _xmlDocs=new XmlDocument[count];
        for(int i=0;i<count;i++){
            string xmlText=_localMan.readLocalText(_xmlNames[i]);

            XmlDocument xmlDoc=new XmlDocument();
            xmlDoc.LoadXml(xmlText);
            _xmlDocs[i]=xmlDoc;
        }
        //4.加载场景

        _loader.loadScene("category",true,loadCategorySceneComplete,LoadSceneMode.Additive);
    }
    private void loadCategorySceneComplete(){
        Scene scene=SceneManager.GetSceneByName("category");
        SceneManager.SetActiveScene(scene);

        //进入应用如果是中文加载场景完成马上显示广点通广告
        if(isCnLanguage&&adManager.isCallGdt){
            GdtAdManager.instance.autoLoadShowInterstitial();
        }
    }


    public void log(object message){
        Debug.Log(message);
        logText.text+=message.ToString()+"\n";
    }

    /**记录历史项*/
    public void recordHistoryItem(string categoryTitle,string title,string iconURL,string contentURL,ContentIconCtl.LinkType linkType,bool isLandscape=false){
        switch(linkType){
            case ContentIconCtl.LinkType.VIDEO:
                addHistoryItemToLcalXml(VIDEO_HISTORY,categoryTitle,title,iconURL,contentURL,15);
                break;
            case ContentIconCtl.LinkType.MUSIC:
                addHistoryItemToLcalXml(MUSIC_HISTORY,categoryTitle,title,iconURL,contentURL,15);
                break;
            case ContentIconCtl.LinkType.GAME:
                addHistoryItemToLcalXml(GAME_HISTORY,categoryTitle,title,iconURL,contentURL,15,isLandscape);
                break;
        }
    }

    /**添加历史项到xml*/
    private void addHistoryItemToLcalXml(string xmlName,string categoryTitle,string title,string iconURL,string contentURL,int recardMax,bool isLandscape=false){
        XmlDocument xml=null;
        if(_localMan.isExist(xmlName)){
            string xmlText=_localMan.readLocalText(xmlName);
            xml=new XmlDocument();
            xml.LoadXml(xmlText);
            XmlManager.removeExistNameNode(title,xml);//移除已经保存有的当前项
            XmlManager.removeOutRangeNode(recardMax,xml);//移除超出保存个数的项
            XmlManager.addNodeToXml(xml,categoryTitle,title,iconURL,contentURL,true,isLandscape);//添加到头部
        }else{
            xml=XmlManager.createXml();
            XmlManager.addNodeToXml(xml,categoryTitle,title,iconURL,contentURL,false,isLandscape);
        }
        if(xml!=null){
            _localMan.saveXml(xml,xmlName);
        }
    }
    
	public static readonly float W=640;
	public static readonly float H=960;
    public static readonly string VIDEO_HISTORY="videoHistory.xml";
    public static readonly string MUSIC_HISTORY="musicHistory.xml";
    public static readonly string GAME_HISTORY="gameHistory.xml";

    public string videoXmlURL;
    public string gameXmlURL;
    public string musicXmlURL;
    public Text logText;
    public AdManager adManager;

    private string[] _xmlNames;
    private XmlDocument[] _xmlDocs;
    private Loader _loader;

    public Loader loader{
        get{return _loader;}
    }

    private LocalManager _localMan;
    public LocalManager localMan{
        get{return _localMan;}
    }

    public enum XmlName{
        VIDEO=0,
        MUSIC=1,
        GAME=2
    }
    public XmlDocument getXmlDoc(XmlName name){
        return _xmlDocs[(int)name];
    }
    
    public SystemLanguage language{
        get{
            return Application.systemLanguage;
        }
    }

    public bool isCnLanguage{
        get{
            return Application.systemLanguage==SystemLanguage.Chinese 
                || Application.systemLanguage==SystemLanguage.ChineseSimplified 
                || Application.systemLanguage==SystemLanguage.ChineseTraditional;
        }
    }
        
}
