using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Runtime.InteropServices;
using MetaCoreInterop = Meta.Interop.MetaCoreInterop;
using MetaCompositor = Meta.Plugin.MetaCompositor;

public class Intrinsic : MonoBehaviour {

    private int frames = 0;

    [StructLayout(LayoutKind.Sequential)]
    public struct MetaPolyCameraParams
    {
        public float fx;
        public float fy;
        public float cx;
        public float cy;
        public float k1;
        public float k2;
        public float k3;
    }
    MetaCoreInterop.MetaPolyCameraParams intrinsic;
    [SerializeField]
    private Camera cameraContent;


    // Use this for initialization
    void Start () {
        intrinsic = new MetaCoreInterop.MetaPolyCameraParams();
       
    }
	
	// Update is called once per 30 frame
	void Update () {
        if (frames % 30 == 0)
        {
            MetaCoreInterop.meta_get_rgb_intrinsics(ref intrinsic);
            //Debug.Log("fx: "+ intrinsic.fx + " fy: "+intrinsic.fy+" cx: "+intrinsic.cx+"cy: "+intrinsic.cy+" k1: " +intrinsic.k1 + " k2: "+intrinsic.k2+" k3: " + intrinsic.k3);
            //Debug.Log();

        }
        frames++;

    }
    public MetaCoreInterop.MetaPolyCameraParams getIntrinsic()
    {
        return intrinsic;
    }

}
