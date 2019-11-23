using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Xml;

public class CanvasMusicPlayerWindowCtl : MonoBehaviour {

	void Start () {
        gameObject.name="canvas_music_player_window";
		Canvas canvas=GetComponent<Canvas>();
        canvas.worldCamera=GameObject.Find("UICamera").GetComponent<Camera>();

	}
	
	void Update () {
        if(playSwapButton.isOn!=musicPlayer.isPlaying){
            playSwapButton.swapTo(musicPlayer.isPlaying);
        }

        progressTimeTxt.text=getTimeStr(musicPlayer.curTime);
        totalTimeTxt.text=getTimeStr(musicPlayer.curClipLength);

        updateRotateImage();
	}

    private string getTimeStr(float second){
        return (int)(second/60)+":"+(second%60);
    }

    public void initialize(RectTransform musicContentWindowRoot,RectTransform categoryRoot,WindowCtl windowCtl,XmlElement listXml){
        _musicContentWindowRoot=musicContentWindowRoot;
        _categoryRoot=categoryRoot;
        _windowCtl=windowCtl;

        createIconList(listXml);
    }

    /**创建音乐播放器内的图标列表*/
    private void createIconList(XmlElement xml){
        int id=0;
        string categoryTitle=xml.GetAttribute("name");//分类/系列名称
        XmlNodeList nodes=xml.ChildNodes;
        for(int i=0;i<nodes.Count;i++){
            XmlElement element=nodes[i] as XmlElement;
            if(element!=null){
                string name=element.GetAttribute("name");
                string iconURL=element.GetAttribute("iconURL");
                string contentURL=element.GetAttribute("contentURL");
                ContentIconCtl contentIconCtl=Instantiate(listIconPrefab,listIconParent,false).GetComponent<ContentIconCtl>();
                contentIconCtl.initialize(categoryTitle,id,name,iconURL,contentURL,ContentIconCtl.LinkType.MUSIC_PLAYER_DO_PLAYE);
                _listIconContentCtls.Add(contentIconCtl);
                id++;
            }
        }
    }

    /**更新旋转图*/
    private void updateRotateImage(){
        Sprite curSp=_listIconContentCtls[_curPlayID].imageIcon.sprite;
        if(rotateImage.sprite!=curSp){
            rotateImage.sprite=_listIconContentCtls[_curPlayID].imageIcon.sprite;
        }
    }

    public void onClickBack(){
        if(_musicContentWindowRoot!=null){
            _windowCtl.delayActiveGameObject(_musicContentWindowRoot.gameObject);
        }else{
            _windowCtl.activeCanvasCategory();

            ContentMusicCtl contentMusicCtl=GameObject.Find("ContentManager").GetComponent<ContentMusicCtl>();
            contentMusicCtl.createHistoryContent();
        }
        Destroy(gameObject);
    }

    private void ativeMusicContentWindowRoot(){
        _musicContentWindowRoot.gameObject.SetActive(true);
    }

    /**外部播放音乐链接的接口*/
    public ContentIconCtl playID(int id){
        _curPlayID=id;

        string url=_listIconContentCtls[_curPlayID].contentURL;
        musicPlayer.playURL(url);
        return _listIconContentCtls[_curPlayID];
    }

    public void onClickPlayOrPause(){
        musicPlayer.autoPlayOrPause();
    }

    public void onClickPre(){
        if(_curPlayID>0)_curPlayID--;
        else _curPlayID=_listIconContentCtls.Count-1;
        playID(_curPlayID);
    }

    public void onClickNext(){
        if(_curPlayID<_listIconContentCtls.Count-1)_curPlayID++;
        else _curPlayID=0;
        playID(_curPlayID);
    }

    public void onClickRepeat(SwapButtonImage swapBtn){
        musicPlayer.loop=!swapBtn.isOn;
    }

    public MusicPlayer musicPlayer;
    public SwapButtonImage playSwapButton;
    public Text progressTimeTxt;
    public Text totalTimeTxt;
    public RectTransform listIconParent;//图标列表的父级
    public GameObject listIconPrefab;//列表的图标预制件
    public Image rotateImage;//旋转图 

    private RectTransform _musicContentWindowRoot;
    private RectTransform _categoryRoot;
    private WindowCtl _windowCtl;
    private List<ContentIconCtl> _listIconContentCtls=new List<ContentIconCtl>();
    private int _curPlayID;
}
