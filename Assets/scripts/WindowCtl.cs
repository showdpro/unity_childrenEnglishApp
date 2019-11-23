using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class WindowCtl : MonoBehaviour {


    public void deactiveCanvasCategory(){
        if(!_IsActiveCanvasCategory)return;
        _IsActiveCanvasCategory=false;
        GraphicRaycaster raycaster=canvasCategoryRectt.GetComponent<GraphicRaycaster>();
        raycaster.enabled=false;
        _transforms=FuncUtil.deactiveChilds(canvasCategoryRectt,false);
    }

    public void activeCanvasCategory(){
        if(_IsActiveCanvasCategory)return;
        if(_transforms==null)return;
        /*if(!IsInvoking("doActiveCanvasCategory")){
            Invoke("doActiveCanvasCategory",0.2f);
        }*/
        doActiveCanvasCategory();
    }
    private void doActiveCanvasCategory(){
        _IsActiveCanvasCategory=true;
        GraphicRaycaster raycaster=canvasCategoryRectt.GetComponent<GraphicRaycaster>();
        raycaster.enabled=true;
        FuncUtil.activeTransforms(_transforms);
        _transforms=null;
    }

    public void delayActiveGameObject(GameObject gameObj){
        if(!IsInvoking("doDelayActive")){
            _delayActiveTarget=gameObj;
            Invoke("doDelayActive",0.01f);
        }
    }
    private void doDelayActive(){
        _delayActiveTarget.SetActive(true);
        _delayActiveTarget=null;
    }

    void OnDestroy(){
        CancelInvoke("doActiveCanvasCategory");
        CancelInvoke("doDelayActive");
    }

    public RectTransform canvasCategoryRectt;
    private Transform[] _transforms;
    private GameObject _delayActiveTarget;
    private bool _IsActiveCanvasCategory=true;
}
