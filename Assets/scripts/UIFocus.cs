using System.Collections;
using UnityEngine;

public class UIFocus : MonoBehaviour,ICanvasRaycastFilter {

    public bool IsFocus=false;
    public bool IsRaycastLocationValid(Vector2 sp, Camera eventCamera){
        return IsFocus;
    }


}
