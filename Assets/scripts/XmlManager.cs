using System.Xml;
using System.Collections.Generic;

public class XmlManager{
    public static XmlDocument createXml(){
        XmlDocument xml=new XmlDocument();
        xml.AppendChild(xml.CreateXmlDeclaration("1.0","UTF-8",null));//声明
        xml.AppendChild(xml.CreateElement("rootNode"));//根元素
        return xml;
    }

    public static void addNodeToXml(XmlDocument xml,string categoryTitle,string title,string iconURL,string contentURL,bool isPrepend=false,bool isLandscape=false){
        XmlNode root=xml.SelectSingleNode("rootNode");//获取根节点
        XmlElement element=xml.CreateElement("item");
        element.SetAttribute("categoryTitle",categoryTitle);
        element.SetAttribute("name",title);
        element.SetAttribute("iconURL",iconURL);
        element.SetAttribute("contentURL",contentURL);
        if(isLandscape)element.SetAttribute("isLandscape","true");
        if(isPrepend)root.PrependChild(element);
        else root.AppendChild(element);

    }

    public static bool removeExistNameNode(string itemName,XmlDocument xml){
        XmlNode root=xml.SelectSingleNode("rootNode");
        XmlNodeList childNodes=root.ChildNodes;

        XmlNode matchNode=null;

        for(int i=0;i<childNodes.Count;i++){
            XmlElement element=childNodes[i] as XmlElement;
            if(element!=null){
                string name=element.GetAttribute("name");
                if(itemName==name){
                    matchNode=element;
                    break;
                }
            }
        }
        if(matchNode!=null){
            root.RemoveChild(matchNode);
            return true;
        }
        return false;
    }

    public static void removeOutRangeNode(int max,XmlDocument xml){
        XmlNode root=xml.SelectSingleNode("rootNode");
        XmlNodeList childNodes=root.ChildNodes;

        List<XmlNode> outNodes=new List<XmlNode>();
        int validCount=0,i=0;
        for(i=0;i<childNodes.Count;i++){
            XmlElement element=childNodes[i] as XmlElement;
            if(element!=null){
                validCount++;
                if(validCount>max)outNodes.Add(element);
            }
        }

        i=outNodes.Count;
        while(--i>=0){
            root.RemoveChild(outNodes[i]);
        }

    }
  
}

