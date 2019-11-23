using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System;
using System.Xml;

public class LocalManager : MonoBehaviour {

    public void saveText(string text,string name,bool isReplaceExists=true){
        StreamWriter stream;

        FileInfo fileInfo=new FileInfo(Application.persistentDataPath+"//"+name);

        if(isReplaceExists){
            deleteFile(name);
            stream=fileInfo.CreateText();
        }else{
            if(!fileInfo.Exists) stream=fileInfo.CreateText();
            else                 stream=fileInfo.AppendText();
        }
        stream.Write(text);
        stream.Close();
        stream.Dispose();
    }

    public void saveXml(XmlDocument xml,string name){
        xml.Save(Application.persistentDataPath+"//"+name);
    }

    public string readLocalText(string name){
        StreamReader reader=null;
        try{
            reader=File.OpenText(Application.persistentDataPath+"//"+name);
        }catch(Exception err){
            Debug.Log("read path is error:"+Application.persistentDataPath+"//"+name);
            return null;
        }
        string text=reader.ReadToEnd();
        reader.Close();
        reader.Dispose();
        return text;
    }

    public bool isExist(string name){
        FileInfo fileInfo=new FileInfo(Application.persistentDataPath+"//"+name);
        return fileInfo.Exists;
    }

    public void deleteFile(string name){
        File.Delete(Application.persistentDataPath+"//"+name);
    }

}
