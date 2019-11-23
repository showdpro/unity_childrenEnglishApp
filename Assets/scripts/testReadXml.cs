using UnityEngine;
using System.Collections;
using System.Xml;

public class testReadXml : MonoBehaviour {
	public string xmlURL;//http://192.168.1.106/app/cfg.xml

	void Start () {
		StartCoroutine (getXML());
	}

	IEnumerator getXML(){
		WWW www = new WWW (xmlURL);
		string progress;
		while(!www.isDone){
			progress=(((int)(www.progress * 100)) % 100) + "%";
			Debug.Log (progress);
			yield return 1;
		}
		if(www.error!=null){
			Debug.Log ("loading error:"+www.url);
		}else{
			progress="100%";
			Debug.Log (progress);
			//enter complete code
			Debug.Log(www.text);
			parseXML(www.text);
		}
    }

	private void parseXML(string xmlText){
		XmlDocument xmlDoc=new XmlDocument();
		xmlDoc.LoadXml(xmlText);
		XmlNodeList nodeList=xmlDoc.SelectSingleNode("rootNode").ChildNodes;
		for(int i=0;i<nodeList.Count;i++){
			XmlElement category=nodeList[i] as XmlElement;
			Debug.Log (category.GetAttribute("name"));//output: 网站
			Debug.Log (category.InnerXml);//output: <item name="mainPage">www.4463.com</item>
            for(int j=0;j<category.ChildNodes.Count;j++){
				XmlElement item=category.ChildNodes[j] as XmlElement;
				Debug.Log (item.GetAttribute("name"));//output: mainPage
				Debug.Log (item.InnerXml);//output: www.4463.com
				Debug.Log (item.InnerText);//output: www.4463.com
			}
		}

	}

}
