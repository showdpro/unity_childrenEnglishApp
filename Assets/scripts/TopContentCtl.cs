using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TopContentCtl : MonoBehaviour {
    public GameObject topPrefab;
    public GameObject categoryTitlePrefab;
    public RectTransform rectTransform;
    public float topHeight=250;

	void Start () {
        /*clearContent();
        createTop();
        addCategoryTitle();*/
	}

    private void clearContent(){
        Transform[] transforms=GetComponentsInChildren<Transform>(true);
        int i=transforms.Length;
        while(--i>=0){
            if(transforms[i].gameObject==gameObject)continue;
            Destroy(transforms[i].gameObject);
        }
    }

    private void addCategoryTitle(){
        RectTransform rectt=Instantiate(categoryTitlePrefab,rectTransform,false).GetComponent<RectTransform>();
        Vector3 pos=rectt.localPosition;
        //Debug.Log(string.Format("pos.y:{0}, height:{1}",pos.y,rectt.rect.height));
        pos.y=-topHeight;
        rectt.localPosition=pos;

    }

    private void createTop(){
        RectTransform rectt=Instantiate(topPrefab,rectTransform,false).GetComponent<RectTransform>();

    }
	
	void Update () {
        
	}


}
