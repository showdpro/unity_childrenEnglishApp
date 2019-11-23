using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using UnityEditor;
using UnityEditor.Callbacks;
#if UNITY_IOS
using UnityEditor.iOS.Xcode;
#endif


public class PBXProjectSetting{

    //该属性是在build完成后，被调用的callback
    [PostProcessBuild]
    public static void OnPostprocessBuild(BuildTarget buildTarget, string path){
        // BuildTarget需为iOS
        if (buildTarget != BuildTarget.iOS) return;
        Debug.Log("===PostProcessBuildAttribute();===");
#if UNITY_IOS
        string projPath = PBXProject.GetPBXProjectPath(path);
        PBXProject pbxProject = new PBXProject();

        pbxProject.ReadFromString(File.ReadAllText(projPath));
        string target = pbxProject.TargetGuidByName("Unity-iPhone");

        // 关闭Bitcode
        pbxProject.SetBuildProperty(target, "ENABLE_BITCODE", "NO");

        // 添加framework, admob需要的所有framework
        pbxProject.AddFrameworkToProject(target, "AdSupport.framework", false);
        pbxProject.AddFrameworkToProject(target, "EventKit.framework", false);
        pbxProject.AddFrameworkToProject(target, "EventKitUI.framework", false);
        pbxProject.AddFrameworkToProject(target, "CoreTelephony.framework", false);
        pbxProject.AddFrameworkToProject(target, "StoreKit.framework", false);
        pbxProject.AddFrameworkToProject(target, "MessageUI.framework", false);

        // 添加广点通需要的所有framework
        pbxProject.AddFrameworkToProject(target, "AdSupport.framework", false);
        pbxProject.AddFrameworkToProject(target, "CoreLocation.framework", false);
        pbxProject.AddFrameworkToProject(target, "QuartzCore.framework", false);
        pbxProject.AddFrameworkToProject(target, "SystemConfiguration.framework", false);
        pbxProject.AddFrameworkToProject(target, "CoreTelephony.framework", false);
        pbxProject.AddFrameworkToProject(target, "Security.framework", false);
        pbxProject.AddFrameworkToProject(target, "StoreKit.framework", false);
        pbxProject.AddFrameworkToProject(target, "SafariServices.framework", false);//测试时发现缺少的framework,没有这个编译不过
        

        // 广点通需要的libz.dylib 或 libz.tbd
        AddLibToProject(pbxProject, target, "libz.dylib");

        // 修改Info.plist文件
        var plistPath = Path.Combine(path, "Info.plist");
        var plist = new PlistDocument();
        plist.ReadFromFile(plistPath);
        //广点通要求信任 HTTP 请求
        PlistElementDict dict=plist.root.CreateDict("NSAppTransportSecurity");
        dict.SetBoolean("NSAllowsArbitraryLoads",true);

        //应用修改
        plist.WriteToFile(plistPath);

        //保存到本地
        File.WriteAllText(projPath,pbxProject.WriteToString());
#endif
    }

#if UNITY_IOS
    //添加lib方法
    static void AddLibToProject(PBXProject inst, string targetGuid, string lib){
        string fileGuid = inst.AddFile("usr/lib/" + lib, "Frameworks/" + lib, PBXSourceTree.Sdk);
        inst.AddFileToBuild(targetGuid, fileGuid);
    }
#endif
}
