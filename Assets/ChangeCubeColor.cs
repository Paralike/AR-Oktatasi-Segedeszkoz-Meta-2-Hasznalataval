using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using OpenCvSharp;
using MetaCoreInterop = Meta.Interop.MetaCoreInterop;

public class ChangeCubeColor : MonoBehaviour {
    Intrinsic camera_parameters;
    // Use this for initialization
    void Start () {
        //Debug.Log("initcor: x: " + transform.position.x + " y: " + transform.position.y + " z: " + transform.position.z);
        camera_parameters = GameObject.FindGameObjectWithTag("Camera").GetComponent<Intrinsic>();
    }
	
	// Update is called once per frame
	void Update () {
		
	}

    public void updatePosition(Mat TRS, Point2f p)
    {
        //Debug.Log("Transform elott x: " + transform.position.x + " y: " + transform.position.y + " z: " + transform.position.z);
        //transform.lo
        MetaCoreInterop.MetaPolyCameraParams intrinsic = camera_parameters.getIntrinsic();
        float[] camera_matrix = new float[9] { intrinsic.fx, 0, intrinsic.cx, 0, intrinsic.fy, intrinsic.cy, 0, 0, 1 };
        Mat camera_matrix_mat = new Mat(3, 3, MatType.CV_32FC1, camera_matrix);
        Debug.Log("Pass TRS: ");
        for (int i = 0; i < 4; i++)
        {
            Debug.Log("[" + i + "]" + TRS.At<float>(i, 0) + ", " + TRS.At<float>(i, 1) + ", " + TRS.At<float>(i, 2) + ", " + TRS.At<float>(i, 3));
        }
        Debug.Log("trs divide" + TRS.At<float>(0, 0) / TRS.At<float>(2, 3));
        //Debug.Log("Pass TRS: ");
        //Mat H = new Mat(3, 3, MatType.CV_32FC1, new float[9] {TRS.m00/TRS.m32, TRS.m10/TRS.m32, TRS.m30/TRS.m32, TRS.m01/TRS.m32, TRS.m11/TRS.m32, TRS.m31/TRS.m32, TRS.m02/TRS.m32, TRS.m12/TRS.m32/TRS.m32, TRS.m32/TRS.m32 });
        Mat H = new Mat(3, 3, MatType.CV_32FC1, new float[9] {
            TRS.At<float>(0, 0) / TRS.At<float>(2, 3), TRS.At<float>(1, 0) / TRS.At<float>(2, 3), TRS.At<float>(3, 0) / TRS.At<float>(2, 3),
            TRS.At<float>(0, 1) / TRS.At<float>(2, 3), TRS.At<float>(1,1) / TRS.At<float>(2, 3), TRS.At<float>(3,1) / TRS.At<float>(2, 3),
            TRS.At<float>(0,2) / TRS.At<float>(2, 3), TRS.At<float>(1,2) / TRS.At<float>(2, 3), TRS.At<float>(3,2) / TRS.At<float>(2, 3)});
        Debug.Log("Pass H before: ");
        for (int i = 0; i < 3; i++)
        {
            Debug.Log("[" + i + "]" + H.At<float>(i, 0) + ", " + H.At<float>(i, 1) + ", " + H.At<float>(i, 2));
        }
        H = camera_matrix_mat*H;
        Debug.Log("Pass H: ");
        for (int i = 0; i < 3; i++) 
        {
            Debug.Log("[" + i + "]" + H.At<float>(i, 0) + ", " + H.At<float>(i, 1) + ", " + H.At<float>(i,2));
        }
        Mat P = new Mat(3, 1, MatType.CV_32FC1, new float[3] { p.X, p.Y, 0 });
        Mat projection =  H * P;
        Debug.Log("Projection: ");
        //for (int i = 0; i < 3; i++)
        //{
        //    Debug.Log("[" + i + "]" + projection.At<float>(i, 0));
        //}
        Vector3 proj = new Vector3(projection.At<float>(0,0), projection.At<float>(0, 1), projection.At<float>(0, 2));
        Debug.Log("proj: " + proj.ToString());
        transform.position = transform.TransformPoint(proj);
        //transform.position = TRS.GetColumn(3);
        //transform.position = new Vector3(compx, compy, compz);
        //Debug.Log(TRS.GetColumn(3));
    }
}
