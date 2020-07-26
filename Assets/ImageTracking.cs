using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using OpenCvSharp;
using OpenCvSharp.Tracking;
using MetaCoreInterop = Meta.Interop.MetaCoreInterop;


public class ImageTracking : MonoBehaviour {
    Intrinsic camera_parameters;
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
    private int frames = 0;
    //public Matrix4x4 pose3d;
    public Mat pose_new;
    // Use this for initialization
    void Start () {
        camera_parameters = GameObject.FindGameObjectWithTag("Camera").GetComponent<Intrinsic>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public Mat calculate3DPointFrom2D( float X, float Y)
    {
        MetaCoreInterop.MetaPolyCameraParams intrinsic = camera_parameters.getIntrinsic();
        float[] camera_matrix = new float[9] {intrinsic.fx,0,intrinsic.cx,0,intrinsic.fy,intrinsic.cy,0,0,1};
        float[] coordinate_points= new float[3] {X,Y,1};
        


        Mat CM = new Mat(3, 3, MatType.CV_32FC1, camera_matrix);
        Mat P = new Mat(3, 1, MatType.CV_32FC1, coordinate_points);
        //Debug.Log("CM 0,0: " + CM.At<double>(0, 0));
        //Debug.Log("P 0,0: " + P.At<double>(0, 0));
        //Debug.Log("CM: " + CM.Data + " P: " + P.Data);

        //Debug.Log("bef CM 0,0: " + CM.At<float>(0, 0) + " CM 0,1: " + CM.At<float>(0, 1) + " CM 0,2: " + CM.At<float>(0, 2));
        //Debug.Log("bef CM 1,0: " + CM.At<float>(1, 0) + " CM 1,1: " + CM.At<float>(1, 1) + " CM 1,2: " + CM.At<float>(1, 2));
        //Debug.Log("bef CM 2,0: " + CM.At<float>(2, 0) + " CM 1,1: " + CM.At<float>(2, 1) + " CM 1,2: " + CM.At<float>(2, 2));

        if (CM.Cols == P.Rows)
        {
            Mat result = CM.Inv() * P;
            //Debug.Log("CM 0,0: " + CM.Inv().At<float>(0, 0) + " CM 0,1: " + CM.Inv().At<float>(0, 1) + " CM 0,2: " + CM.Inv().At<float>(0, 2));
            //Debug.Log("CM 1,0: " + CM.Inv().At<float>(1, 0) + " CM 1,1: " + CM.Inv().At<float>(1, 1) + " CM 1,2: " + CM.Inv().At<float>(1, 2));
            //Debug.Log("CM 2,0: " + CM.Inv().At<float>(2, 0) + " CM 1,1: " + CM.Inv().At<float>(2, 1) + " CM 1,2: " + CM.Inv().At<float>(2, 2));

            //Debug.Log("P 0,0: " + P.At<float>(0, 0)+ " P 0,1: " + P.At<float>(0, 1)+ " P 0,2: " + P.At<float>(0, 2));
            //Debug.Log("P 1,0: " + P.At<float>(1, 0)+ " P 1,1: " + P.At<float>(0, 1)+ " P 1,2: " + P.At<float>(1, 2));
            //Debug.Log("P 2,0: " + P.At<float>(2, 0)+ " P 2,1: " + P.At<float>(2, 1)+ " P 2,2: " + P.At<float>(2, 2));

            //Debug.Log("res 0,0: " + result.At<float>(0, 0) + " res 0,1: " + result.At<float>(0, 1) + " res 0,2: " + result.At<float>(0, 2));
            //Debug.Log("res 1,0: " + result.At<float>(1, 0) + " res 1,1: " + result.At<float>(1, 1) + " res 1,2: " + result.At<float>(1, 2));
            //Debug.Log("res 2,0: " + result.At<float>(2, 0) + " res 2,1: " + result.At<float>(2, 1) + " res 2,2: " + result.At<float>(2, 2));
            return result;
        }
        else
        {
            throw new System.ArgumentException("A dimenziók nem egyeznek");
        }
        
    }

    public Mat calculate3DFrom2D(Point2f [] imagePoints)
    {
        //Debug.Log("Calculate function");
        Point3f balFelso = new Point3f(0, 0, 0);
        Point3f jobbFelso = new Point3f(0, 55, 0);
        Point3f balAlso = new Point3f(55, 0, 0);
        Point3f alignmentPoint = new Point3f(40,40,0);
        Point3f[] worldPoints = new Point3f[4]{balFelso,jobbFelso,balAlso,alignmentPoint};
        Mat wP = new Mat(4,3,MatType.CV_32FC1, worldPoints);
        //Debug.Log("Pass 1: ");
        //for(int i = 0;i< 3; i++) //Oszloponként megy és nem soronként de egyéként jó
        //{
        //    Debug.Log("["+i+"]"+wP.At<float>(0,i)+", " + wP.At<float>(1, i) + ", " + wP.At<float>(2, i) + ", " + wP.At<float>(3, i) + ", ");
        //}
        MetaCoreInterop.MetaPolyCameraParams intrinsic = camera_parameters.getIntrinsic();
        float[] camera_matrix = new float[9] { intrinsic.fx, 0, intrinsic.cx, 0, intrinsic.fy, intrinsic.cy, 0, 0, 1 };
        Mat camera_matrix_mat = new Mat(3, 3, MatType.CV_32FC1, camera_matrix);
        Debug.Log("Camera matrix: ");
        for (int i = 0; i < 3; i++) 
        {
            Debug.Log("[" + i + "]" + camera_matrix_mat.At<float>(i, 0) + ", " + camera_matrix_mat.At<float>(i, 1) + ", " + camera_matrix_mat.At<float>(i, 2));
        }
        Mat distCoeffs = new Mat(4,1,MatType.CV_32FC1, new float[4] { 0, 0, 0, 0 });
        //Debug.Log("Pass 2");
        Mat rotationVector = new Mat();
        Mat translationVector = new Mat();
        Mat iP = new Mat(4, 2, MatType.CV_32FC1, imagePoints);
        //for (int i = 0; i < 4; i++) // itt már soronként iratom ki
        //{
        //    Debug.Log("[" + i + "]" + iP.At<float>(i, 0) + ", " + iP.At<float>(i,1) );
        //}
        //Debug.Log("Pass 3");
        Cv2.SolvePnP(wP, iP, camera_matrix_mat,distCoeffs, rotationVector, translationVector);
        //Debug.Log("after solvePnP");
        Mat rotationMatrix = new Mat(3,3,MatType.CV_32FC1);
        Cv2.Rodrigues(rotationVector,rotationMatrix);
        //Debug.Log("after rodrigues: ");

        //Mat screenCoordinates = new Mat(3, 1, MatType.CV_32FC1, new float[3] { 0, 0, 0 });
        //Mat invR_x_invM_x_uv1 = rotationMatrix.Inv() * camera_matrix_mat.Inv() * screenCoordinates;
        //Mat invR_x_tvec = rotationMatrix.Inv() * translationVector;
        //Mat wcPoint = (0 + invR_x_tvec.At<float>(2, 0)) / invR_x_invM_x_uv1.At<float>(2, 0) * invR_x_invM_x_uv1 - invR_x_tvec;
        //Point3f worldCoordinates = new Point3f(wcPoint.At<float>(0, 0), wcPoint.At<float>(1, 0), wcPoint.At<float>(2, 0));

        //Debug.Log("World Coordinates: " + screenCoordinates.At<float>(0, 0) + "," + screenCoordinates.At<float>(1, 0)
        //    + worldCoordinates.X + "," + worldCoordinates.Y);

        //TRS-re át kell írni
        //pose3d.SetRow(0, new Vector4((float)rotationMatrix.At<float>(0, 0), (float)rotationMatrix.At<float>(0, 1), (float)rotationMatrix.At<float>(0, 2), (float)translationVector.At<float>(0, 0)));
        //pose3d.SetRow(1, new Vector4((float)rotationMatrix.At<float>(1, 0), (float)rotationMatrix.At<float>(1, 1), (float)rotationMatrix.At<float>(1, 2), (float)translationVector.At<float>(1, 0)));
        //pose3d.SetRow(2, new Vector4((float)rotationMatrix.At<float>(2, 0), (float)rotationMatrix.At<float>(2, 1), (float)rotationMatrix.At<float>(2, 2), (float)translationVector.At<float>(2, 0)));
        //pose3d.SetRow(3, new Vector4(0, 0, 0, 1));
        pose_new = new Mat(4, 4, MatType.CV_32FC1, new float[16] { (float)rotationMatrix.At<float>(0, 0), (float)rotationMatrix.At<float>(0, 1), (float)rotationMatrix.At<float>(0, 2), (float)translationVector.At<float>(0, 0),
            (float)rotationMatrix.At<float>(1, 0), (float)rotationMatrix.At<float>(1, 1), (float)rotationMatrix.At<float>(1, 2), (float)translationVector.At<float>(1, 0),
            (float)rotationMatrix.At<float>(2, 0), (float)rotationMatrix.At<float>(2, 1), (float)rotationMatrix.At<float>(2, 2), (float)translationVector.At<float>(2, 0),
            0,0,0,1
        });

        return pose_new;
        //return pose3d;

    }
}
