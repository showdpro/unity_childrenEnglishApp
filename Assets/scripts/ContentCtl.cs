using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Xml;

public class ContentCtl : MonoBehaviour {

    virtual protected void Awake(){
        _instances=new List<GameObject>();
    }

	virtual protected void Start () {
        
	}

    virtual public void onValueChangedScrollbarRoot(){
        if(_lastScrollbarRootValue==scrollbarRootVertical.value)return;
        int dir=(scrollbarRootVertical.value>_lastScrollbarRootValue)?1:-1;//-1:向上滚，1:向下滚
        
        if(dir<0){
            if(scrollbarRootVertical.value<=swapTopParentValue){
                //切换顶部内容到置顶不滚动的容器
                swapTopToFixedNode();
            }
        }else{
            if(scrollbarRootVertical.value>=swapTopParentValue){
                //切换顶部内容到可以滚动的容器
                swapTopToScrollNode();
            }
        }
        _lastScrollbarRootValue=scrollbarRootVertical.value;
    }

    virtual protected void swapTopToFixedNode(){}
    virtual protected void swapTopToScrollNode(){}

    /**实例化一个标题卡预制件*/
    protected RectTransform instanceCategoryTitlePrefab(RectTransform parent){
        RectTransform rectt=Instantiate(categoryTitlePrefab,parent,false).GetComponent<RectTransform>();
        _instances.Add(rectt.gameObject);
        return rectt;
    }

    /**实例化一个记录大小的节点预制件*/
    protected RectTransform instanceRecardSizeNode(){
        RectTransform rectt=Instantiate(recardSizeNodePrefab,rootRectt,false).GetComponent<RectTransform>();
        _instances.Add(rectt.gameObject);
        return rectt;
    }

    /**实例化一个顶部预制件*/
    protected RectTransform instanceTopPrefab(RectTransform recardSizeRectt){
        RectTransform rectt=Instantiate(topPrefab,recardSizeRectt,false).GetComponent<RectTransform>();
        _instances.Add(rectt.gameObject);
        LayoutElement topElement=rectt.GetComponent<LayoutElement>();
        //记录大小
        LayoutElement element=recardSizeRectt.GetComponent<LayoutElement>();
        element.minWidth=topElement.minWidth;
        element.minHeight=topElement.minHeight;
        return rectt;
    }

    /**实例化一个历史(横滚动)预制件*/
    protected RectTransform instanceHistoryPrefab(){
        RectTransform rectt=Instantiate(historyPrefab,rootRectt,false).GetComponent<RectTransform>();
        _instances.Add(rectt.gameObject);
        return rectt;
    }

    /**实例化一个分类预制件*/
    protected RectTransform instanceCategoryPrefab(){
        RectTransform rectt=Instantiate(categoryPrefab,rootRectt,false).GetComponent<RectTransform>();
        _instances.Add(rectt.gameObject);
        return rectt;
    }

    virtual protected XmlElement getCategoryXmlWithName(string categoryTitle,Game.XmlName xmlName){
        //XmlDocument xml=Game.instance.getXmlDoc(xmlName);
        return null;
    }

    virtual public void setActive(bool value){
        if(_instances==null)return;
        if(value)scrollbarRootVertical.value=1;
        for(int i=0;i<_instances.Count;i++){
            _instances[i].SetActive(value);
        }
    }

    

    public Scrollbar scrollbarRootVertical;
    public float swapTopParentValue=0.92f;//决定滚动什么位置切换顶部内容
    public RectTransform rootRectt;
    public RectTransform topFixedRectt;
    public GameObject historyPrefab;
    public GameObject categoryPrefab;
    public GameObject topPrefab;
    public GameObject recardSizeNodePrefab;
    public GameObject categoryTitlePrefab;

    protected const uint e_isInitialize=0x0001;
    protected uint _flags;
    protected List<GameObject> _instances;

    private float _lastScrollbarRootValue=1;
    


}
