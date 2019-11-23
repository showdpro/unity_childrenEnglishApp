using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using UnityEngine.Video;
using System.Xml;

public class ContentIconCtl : MonoBehaviour {

    public enum LinkType{
        VIDEO,
        VIDEO_DO_PLAY_HISTORY,//视频页的历史栏图标
        VIDEO_PLAYER_DO_PLAYE,//视频播放器内列表图标
        MUSIC,
        MUSIC_DO_PLAY_HISTORY,//音乐页的历史栏图标
        MUSIC_PLAYER_DO_PLAYE,//音乐播放器内列表图标
        GAME
    }

	void Start () {
        _windowCtl=GameObject.Find("WindowManager").GetComponent<WindowCtl>();
        ImageLoader imageLoader=gameObject.AddComponent<ImageLoader>();
        imageLoader.initialize(imageIcon,_title,_iconURL);
	}

    void OnDestroy(){
        CancelInvoke("doShowInterstitial");
    }

    public void initialize(string categoryTitle,int id,string title,string iconURL,string contentURL,LinkType linkType){
        _categoryTitle=categoryTitle;
        _id=id;
        _title=title;
        _iconURL=iconURL;

        textTitle.text=title;
        _contentURL=contentURL;
        _linkType=linkType;
    }

    /**初始视频link图标*/
    public void initialize(string categoryTitle,int id,string title,string iconURL,string contentURL,LinkType linkType,XmlElement videoPlayerListXml){
        initialize(categoryTitle,id,title,iconURL,contentURL,linkType);
        _categoryXml=videoPlayerListXml;
    }

    /**初始音乐link图标*/
    public void  initialize(string categoryTitle,int id,string title,string iconURL,string contentURL,LinkType linkType,RectTransform musicContentWindowRoot){
        initialize(categoryTitle,id,title,iconURL,contentURL,linkType);
        _musicContentWindowRoot=musicContentWindowRoot;
    }
    /**初始游戏link图标*/
    public void  initialize(string categoryTitle,int id,string title,string iconURL,string contentURL,LinkType linkType,bool isLandscape){
        initialize(categoryTitle,id,title,iconURL,contentURL,linkType);
        _isLandscape=isLandscape;
    }
	
    public void onClick(){
        switch(_linkType){
            case LinkType.VIDEO:
                gotoVideoPlayer();
                waitScreenRotationShowInterstitial();
                break;
            case LinkType.VIDEO_DO_PLAY_HISTORY:
                videoDoPlayHistory();
                waitScreenRotationShowInterstitial();
                break;
            case LinkType.VIDEO_PLAYER_DO_PLAYE:
                videoPlayerDoPlay();
                doShowInterstitial();
                break;
            case LinkType.MUSIC:
                gotoMusicPlayer();
                doShowInterstitial();
                break;
            case LinkType.MUSIC_DO_PLAY_HISTORY:
                musicDoPlayHistory();
                doShowInterstitial();
                break;
            case LinkType.MUSIC_PLAYER_DO_PLAYE:
                musicPlayerDoPlay();
                doShowInterstitial();
                break;
            case LinkType.GAME:
                gotoGamePlayer();
                waitScreenRotationShowInterstitial();
                break;
        }
    }

    private void waitScreenRotationShowInterstitial(){
        Invoke("doShowInterstitial",1.5f);
    }
    private void doShowInterstitial(){
        AdManager.instance.showIntersitial();
    }

    /**新建视频播放器并播放第一集*/
    private void gotoVideoPlayer(){
        _windowCtl.deactiveCanvasCategory();//隐藏分类主界面

        GameObject canvasVideoObj=Instantiate(canvas_video_player_prefab) as GameObject;
        CanvasVideoPlayerWindowCtl videoPlayerWindowCtl=canvasVideoObj.GetComponent<CanvasVideoPlayerWindowCtl>();

        videoPlayerWindowCtl.initialize(canvasVideoObj,_categoryXml);
        ContentIconCtl contentIconCtl=videoPlayerWindowCtl.playID(0);//点剧集图标时播放第一集
        canvasVideoObj.SetActive(true);

        //添加到历史记录
        Game.instance.recordHistoryItem(contentIconCtl.categoryTitle,contentIconCtl.titleName,contentIconCtl.iconURL,contentIconCtl.contentURL,LinkType.VIDEO);
    }
    /**点击视频历史图标时 */
    private void videoDoPlayHistory(){
        _windowCtl.deactiveCanvasCategory();//隐藏分类主界面

        GameObject canvasVideoObj=Instantiate(canvas_video_player_prefab) as GameObject;
        CanvasVideoPlayerWindowCtl videoPlayerWindowCtl=canvasVideoObj.GetComponent<CanvasVideoPlayerWindowCtl>();

        videoPlayerWindowCtl.initialize(canvasVideoObj,_categoryXml);
        ContentIconCtl contentIconCtl=videoPlayerWindowCtl.playTiTle(_title);
        canvasVideoObj.SetActive(true);

        //添加到历史记录
        Game.instance.recordHistoryItem(contentIconCtl.categoryTitle,contentIconCtl.titleName,contentIconCtl.iconURL,contentIconCtl.contentURL,LinkType.VIDEO);
    }
    /**视频播放器内播放*/
    private void videoPlayerDoPlay(){
        GameObject videoPlayerWindowObj=GameObject.Find("canvas_video_player_window");
        CanvasVideoPlayerWindowCtl videoPlayerWindowCtl=videoPlayerWindowObj.GetComponent<CanvasVideoPlayerWindowCtl>();
        ContentIconCtl contentIconCtl=videoPlayerWindowCtl.playID(_id);

        //添加到历史记录
        Game.instance.recordHistoryItem(contentIconCtl.categoryTitle,contentIconCtl.titleName,contentIconCtl.iconURL,contentIconCtl.contentURL,LinkType.VIDEO);
    }

    /**新建音乐播放器并播放*/
    private void gotoMusicPlayer(){
        _windowCtl.deactiveCanvasCategory();//隐藏分类主界面
        if(_musicContentWindowRoot!=null){
            _categoryXml=_musicContentWindowRoot.GetComponent<CanvasMusicContentWindowCtl>().contentXml;
            _musicContentWindowRoot.gameObject.SetActive(false);//隐藏音乐内容窗口
        }
        CanvasMusicPlayerWindowCtl musicPlayerWindowCtl=Instantiate(canvas_music_player_prefab).GetComponent<CanvasMusicPlayerWindowCtl>();
        musicPlayerWindowCtl.initialize(_musicContentWindowRoot,_windowCtl.canvasCategoryRectt,_windowCtl,_categoryXml);
        ContentIconCtl contentIconCtl=musicPlayerWindowCtl.playID(_id);

        //添加到历史记录
        Game.instance.recordHistoryItem(contentIconCtl.categoryTitle,contentIconCtl.titleName,contentIconCtl.iconURL,contentIconCtl.contentURL,LinkType.MUSIC);
    }
    /**点击音乐历史图标时 */
    private void musicDoPlayHistory(){
        _windowCtl.deactiveCanvasCategory();//隐藏分类主界面
        if(_musicContentWindowRoot!=null){
            _categoryXml=_musicContentWindowRoot.GetComponent<CanvasMusicContentWindowCtl>().contentXml;
        }
        CanvasMusicPlayerWindowCtl musicPlayerWindowCtl=Instantiate(canvas_music_player_prefab).GetComponent<CanvasMusicPlayerWindowCtl>();
        musicPlayerWindowCtl.initialize(_musicContentWindowRoot,_windowCtl.canvasCategoryRectt,_windowCtl,_categoryXml);
        ContentIconCtl contentIconCtl=musicPlayerWindowCtl.playID(_id);

        //添加到历史记录
        Game.instance.recordHistoryItem(contentIconCtl.categoryTitle,contentIconCtl.titleName,contentIconCtl.iconURL,contentIconCtl.contentURL,LinkType.MUSIC);
    }
    /**音乐播放器内播放*/
    private void musicPlayerDoPlay(){
        GameObject musicPlayerWindowObj=GameObject.Find("canvas_music_player_window");
        CanvasMusicPlayerWindowCtl musicPlayerWindowCtl=musicPlayerWindowObj.GetComponent<CanvasMusicPlayerWindowCtl>();
        ContentIconCtl contentIconCtl=musicPlayerWindowCtl.playID(_id);

        //添加到历史记录
        Game.instance.recordHistoryItem(contentIconCtl.categoryTitle,contentIconCtl.titleName,contentIconCtl.iconURL,contentIconCtl.contentURL,LinkType.MUSIC);
    }

    /**新建游戏播放器*/
    private void gotoGamePlayer(){
        _windowCtl.deactiveCanvasCategory();//隐藏分类主界面
        CanvasGamePlayerWindowCtl gamePlayerWindowCtl=Instantiate(canvas_game_player_prefab).GetComponent<CanvasGamePlayerWindowCtl>();
        gamePlayerWindowCtl.initialize(_isLandscape,_contentURL,_windowCtl);

        //添加到历史记录
        Game.instance.recordHistoryItem(_categoryTitle,_title,_iconURL,_contentURL,LinkType.GAME,_isLandscape);
    }

	/*private void setVisible(bool value){
		if(value==_visible)return;
		_visible=value;
		imageIcon.gameObject.SetActive(_visible);
		textTitle.gameObject.SetActive(_visible);
		Image bg=GetComponent<Image>();
		if(bg!=null)bg.enabled=_visible;
	}*/

    public string contentURL{ get{return _contentURL;} }
    public string titleName{get{return _title;}}
    public string categoryTitle{get{return _categoryTitle;}}
    public string iconURL{get{return _iconURL;}}
    public LinkType linkType{get{return _linkType;}}

    public Text textTitle;
    public Image imageIcon;
    public GameObject canvas_video_player_prefab;
    public GameObject canvas_music_player_prefab;
    public GameObject canvas_game_player_prefab;

    private string _title;
    private string _iconURL;
    private string _contentURL;
    private LinkType _linkType;
	//private bool _visible=true;
    private WindowCtl _windowCtl;
    private RectTransform _musicContentWindowRoot;
    private int _id;//这个图标位于当前图标列表的id
    private bool _isLandscape;
    private XmlElement _categoryXml;//这个图标所处当前分类内容xml
    private string _categoryTitle;//这个图标所处当前分类标题

}
