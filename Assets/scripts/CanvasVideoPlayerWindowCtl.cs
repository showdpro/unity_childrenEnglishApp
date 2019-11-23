using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using UnityEngine.Video;
using System.Xml;


public class CanvasVideoPlayerWindowCtl : MonoBehaviour {

    public delegate void PlayEndDelegate();
    public event PlayEndDelegate playEndEvent;

    void Awake(){
        videoPlayer.errorReceived+=errorReceived;
        videoPlayer.frameDropped+=frameDropped;
        videoPlayer.frameReady+=frameReady;
        videoPlayer.loopPointReached+=loopPointReached;
        videoPlayer.prepareCompleted+=prepareCompleted;
        videoPlayer.seekCompleted+=seekCompleted;
        videoPlayer.started+=started;

        videoPlayer.sendFrameReadyEvents=true;
    }

	void Start () {
        gameObject.name="canvas_video_player_window";

        Camera uiCamera=GameObject.Find("UICamera").GetComponent<Camera>();
        canvas.worldCamera=uiCamera;

        _anchorMin0 = maskTf.anchorMin;
        _anchorMax0 = maskTf.anchorMax;

        //设置横屏
        ScreenCtl.setLandscape();

	}
	
	void Update () {
        int totalTime=getTotalTime();
        timeTxt.text=getTimeStr((int)videoPlayer.time)+" / "+getTimeStr(totalTime);

        if(!_isEnd){
            if(videoPlayer.frame>0&&videoPlayer.time>=totalTime){
                _isEnd=true;
                playEndHandler();
            }
        }
	}

    public void initialize(GameObject canvasVideoObj,XmlElement listXml){
        _canvasVideoObj=canvasVideoObj;
        string categoryTitle=createIconList(listXml);
        playingCategoryTxt.text="正在播放 "+categoryTitle+" 系列";
    }

    private int getTotalTime(){
        int totalTime=(int)((float)videoPlayer.frameCount/videoPlayer.frameRate);
        if(totalTime<0)totalTime=0;
        return totalTime;
    }

    /**创建播放器内图标列表*/
    private string createIconList(XmlElement xml){
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
                contentIconCtl.initialize(categoryTitle,id,name,iconURL,contentURL,ContentIconCtl.LinkType.VIDEO_PLAYER_DO_PLAYE);
                _listIconContentCtls.Add(contentIconCtl);
                id++;
            }
        }
        return categoryTitle;
    }

    public ContentIconCtl playID(int id){
        _curPlayID=id;

        videoPlayer.url=_listIconContentCtls[_curPlayID].contentURL;
        _isEnd=false;

        movieTitleTxt.text=_listIconContentCtls[_curPlayID].titleName;
        return _listIconContentCtls[_curPlayID];
    }
    public ContentIconCtl playTiTle(string titleName){
        int id=getIdFromURL(titleName);
        return playID(id);
    }
    private int getIdFromURL(string titleName){
        int id=0;
        for(int i=0;i<_listIconContentCtls.Count;i++){
            if(_listIconContentCtls[i].titleName==titleName){
                id=i;
                break;
            }
        }
        return id;
    }


    public void autoScreenOrRest(){
        if (_isFullScreen) resetScreen();
        else fullScreen();
    }

    public void autoPauseOrPlay(){
        if(_isSeeking)return;
        if (videoPlayer.isPlaying){
            pause();
        }else{
            play();
        }
    }
    private void pause(bool isDoSwap=true){
        videoPlayer.Pause();
        if(isDoSwap)playOrPauseButton.swapTo(false);
    }
    private void play(bool isDoSwap=true){
        videoPlayer.Play();
        if(isDoSwap)playOrPauseButton.swapTo(true);
        _isEnd=false;
    }
    private void stop(bool isDoSwap=true){
        videoPlayer.Stop();
        if(isDoSwap)playOrPauseButton.swapTo(false);
        _isSeeking=false;
    }

    public void autoMuteOrPlaySound(){
        _isMute=!_isMute;
        videoAudioSource.mute=_isMute;
    }

    public void onClickBack(){
        if(_isFullScreen){
            resetScreen();
        }else{
            ScreenCtl.setPortrait();

            //退出video播放器
            Destroy(_canvasVideoObj);

            WindowCtl windowCtl=GameObject.Find("WindowManager").GetComponent<WindowCtl>();
            windowCtl.activeCanvasCategory();//激活分类主界面

            ContentVideoCtl contentVideoCtl=GameObject.Find("ContentManager").GetComponent<ContentVideoCtl>();
            contentVideoCtl.createHistoryContent();
        }
    }

    public void onClickRepeat(SwapButtonImage swapBtn){
        _isLoop=swapBtn.isOn;
    }

    public void setTime(float progress){
        if(_isEnd)return;

        double targetTime=((double)videoPlayer.frameCount/(double)videoPlayer.frameRate)*progress;
        pause(false);
        videoPlayer.time=targetTime;
        _isSeeking=true;
    }

    private void fullScreen(){
        if (_isFullScreen) return;
        _isFullScreen = true;
        mask.enabled = false;
        float delay=0.5f;
        maskTf.DOAnchorMin(new Vector2(0,0),delay);
        maskTf.DOAnchorMax(new Vector2(1,1),delay);
    }

    private void resetScreen(){
        if (!_isFullScreen)return;
        _isFullScreen = false;
        float delay=0.5f;
        maskTf.DOAnchorMin(_anchorMin0,delay);
        maskTf.DOAnchorMax(_anchorMax0,delay).OnComplete(resetComplete);
    }
    private void resetComplete(){
        mask.enabled = true;
    }

    private string getTimeStr(float second){
        int hour=(int)(second/3600);
        int minute=(int)((second%3600)/60);
        int sec=(int)(second%3600)%60;

        string hourStr=hour.ToString();if(hourStr.Length<2)hourStr="0"+hourStr;
        string minuteStr=minute.ToString();if(minuteStr.Length<2)minuteStr="0"+minuteStr;
        string secStr=sec.ToString();if(secStr.Length<2)secStr="0"+secStr;
            
        return hourStr+":"+minuteStr+":"+secStr;
    }

    private void errorReceived(VideoPlayer source,string message){
        //Debug.Log("videoPlayer errorReceived:"+message);
    }
    private void frameDropped(VideoPlayer source){
        //Debug.Log("videoPlayer frameDropped");
    }
    private void frameReady(VideoPlayer source,long frameidx){
        //Debug.Log("videoPlayer frameReady:");
    }
    private void loopPointReached(VideoPlayer source){
        //Debug.Log("videoPlayer loopPointReached");
    }
    private void prepareCompleted(VideoPlayer source){
        //Debug.Log("videoPlayer prepareCompleted");
    }
    private void seekCompleted(VideoPlayer source){
        //Debug.Log("videoPlayer seekCompleted");
        if(_isSeeking){
            play();
            _isSeeking=false;
        }
    }
    private void started(VideoPlayer source){
        //Debug.Log("started");
    }
    private void playEndHandler(){
        stop();
        videoPlayer.targetTexture.Release();
        playEndEvent();

        if(_isLoop)play();
    }

    void OnDestroy(){
        videoPlayer.targetTexture.Release();
        _listIconContentCtls=null;
    }

    public bool isPlaying{get{return videoPlayer.isPlaying;}}

    public GameObject videoParent;
    public GameObject maskObj;
    public RectTransform maskTf;
    public Mask mask;
    public VideoPlayer videoPlayer;
    public AudioSource videoAudioSource;
    public Canvas canvas;
    public GameObject listIconPrefab;
    public RectTransform listIconParent;
    public Text timeTxt;
    public Text playingCategoryTxt;
    public Text movieTitleTxt;
    public SwapButtonImage playOrPauseButton;


    private GameObject _canvasVideoObj;
    private Vector2 _anchorMin0;
    private Vector2 _anchorMax0;
    private bool _isFullScreen;
    private bool _isMute;
    private List<ContentIconCtl> _listIconContentCtls=new List<ContentIconCtl>();
    private int _curPlayID;
    private bool _isSeeking;
    private bool _isEnd;
    private bool _isLoop;

}
