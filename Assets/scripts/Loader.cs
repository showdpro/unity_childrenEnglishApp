using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class Loader : MonoBehaviour {
    
	void Start () {

	}

    public void loadScene(string sceneName,bool isShowBg=false,SceneLoadedCallback callback=null,LoadSceneMode mode=LoadSceneMode.Single){
        setProgressBarVisible(true,isShowBg);
        StartCoroutine(doLoadScene(sceneName,callback,mode));
    }
    IEnumerator doLoadScene(string sceneName,SceneLoadedCallback callback,LoadSceneMode mode){
        _async=SceneManager.LoadSceneAsync(sceneName,mode);
        while(!_async.isDone){
            updateProgress(_async.progress,"scene "+sceneName);
            yield return 1;
        }
        //加载完成
        updateProgress(1,"scene "+sceneName);
        setProgressBarVisible(false);
        if(callback!=null)callback();
    }

    public void loadXmls(string[]urls,string[]xmlNames, XmlLoadedCallback callback=null){
        _xmlURLs=urls;
        _xmlNames=xmlNames;
        _xmlsLoadedCallback=callback;
        if(_xmlTexts==null)_xmlTexts=new string[_xmlURLs.Length];

        _xmlId=0;
        loadXml(_xmlURLs[_xmlId],_xmlNames[_xmlId],xmlLoaded);
    }
    private void xmlLoaded(string[] xmlDocs){
        _xmlTexts[_xmlId]=xmlDocs[0];

        if(_xmlId>=_xmlURLs.Length-1){
            if(_xmlsLoadedCallback!=null)_xmlsLoadedCallback(_xmlTexts);
            _xmlURLs=null;
            _xmlNames=null;
            _xmlsLoadedCallback=null;
        }else{
            _xmlId++;
            loadXml(_xmlURLs[_xmlId],_xmlNames[_xmlId],xmlLoaded);
        }
    }

    private void loadXml(string url,string xmlName,XmlLoadedCallback callback=null){
        setProgressBarVisible(true,false);
        StartCoroutine(doLoadXml(url,xmlName,callback));
    }
    IEnumerator doLoadXml(string url,string xmlName,XmlLoadedCallback callback){
        WWW www = new WWW (url);
        while(!www.isDone){
            updateProgress(www.progress,"xml "+xmlName);
            yield return 1;
        }
        if(www.error!=null){
            updateProgress(0,"xml "+xmlName+" error");
            Debug.Log ("loading error:"+www.url+","+www.error);
        }else{
            updateProgress(1,"xml "+xmlName);
            //加载完成
            setProgressBarVisible(false);
            if(callback!=null) callback(new string[]{www.text});
        }
        yield return 1;
    }
	
    /**更新进度条*/
    private void updateProgress(float progress,string loadItemName){
        slider.value=progress;
        progressText.text=string.Format("loading {0} {1}% ...",loadItemName,(int)(progress*100));
    }

    /**设置进度条是否显示*/
    private void setProgressBarVisible(bool visible,bool isShowBg=false){
        slider.gameObject.SetActive(visible);
        if(visible){
            if(isShowBg)sceneBg.gameObject.SetActive(visible);
        }else{
            sceneBg.gameObject.SetActive(visible);
        }
        progressText.gameObject.SetActive(visible);
    }

    public Slider slider;
    public Image sceneBg;
    public Text progressText;

    private AsyncOperation _async;

    private string[] _xmlURLs;
    private string[] _xmlNames;
    private XmlLoadedCallback _xmlsLoadedCallback;
    private int _xmlId;

    private string[] _xmlTexts;
}
