using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FuncUtil{
    /**吊销所有子节点*/
    public static Transform[] deactiveChilds(Transform root,bool includeInactive){
        Transform[] transforms=root.GetComponentsInChildren<Transform>(includeInactive);
        int i=transforms.Length;
        while(--i>=0){
            if(transforms[i].gameObject==root.gameObject)continue;
            transforms[i].gameObject.SetActive(false);
        }
        return transforms;
    }
    
    public static void deactiveTransforms(Transform[] transforms){
        int i=transforms.Length;
        while(--i>=0) transforms[i].gameObject.SetActive(false);
    }

    /**激活有子节点*/
    public static Transform[] activeChilds(Transform root,bool includeInactive){
        Transform[] transforms=root.GetComponentsInChildren<Transform>(includeInactive);
        int i=transforms.Length;
        while(--i>=0){
            if(transforms[i].gameObject==root.gameObject)continue;
            transforms[i].gameObject.SetActive(true);
        }
        return transforms;
    }

    public static void activeTransforms(Transform[] transforms){
        int i=transforms.Length;
        while(--i>=0) transforms[i].gameObject.SetActive(true);
    }

    /**销毁所有子节点*/
    public static void destroyChilds(Transform root,bool includeInactive){
        Transform[] transforms=root.GetComponentsInChildren<Transform>(includeInactive);
        int i=transforms.Length;
        while(--i>=0){
            if(transforms[i].gameObject==root.gameObject)continue;
            Object.Destroy(transforms[i].gameObject);
        }
    }

    public static void destroyTransforms(Transform[] transforms){
        int i=transforms.Length;
        while(--i>=0) Object.Destroy(transforms[i].gameObject);
    }
	
}
