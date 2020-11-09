using OpenCvSharp;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ZXing;
using ZXing.QrCode;

public class QR : MonoBehaviour {


private WebCamTexture camTexture;
    GameObject Cube;
    ChangeCubeColor cube_script;
    ImageTracking camera_parameters;
    public float[] outPoints = new float[] { 0,0,0,0,0,0,0,0};
    //private Rect screenRect;
    private int frames = 0;
    private bool isInicialized = false;
    // Use this for initialization
    void Start () {
        //screenRect = new Rect(0, 0, Screen.width, Screen.height);
        camTexture = new WebCamTexture();
        camTexture.requestedHeight = Screen.height;
        camTexture.requestedWidth = Screen.width;
        if (camTexture != null)
        {
            camTexture.Play();
        }
        camera_parameters = GameObject.FindGameObjectWithTag("Camera").GetComponent<ImageTracking>();
        Cube = GameObject.FindGameObjectWithTag("Cube1");
        cube_script = Cube.GetComponent<ChangeCubeColor>();
    }
            /// <param name="camera_params">ref to struct to be filled with params</param>
	
	// Update is called once per frame
	void Update () {
        // drawing the camera on screen
        //GUI.DrawTexture(screenRect, camTexture, ScaleMode.ScaleToFit);
        // do the reading — you might want to attempt to read less often than you draw on the screen for performance sake
        try
        {
            frames++;
            if (frames % 10 == 0)
            {
                
                IBarcodeReader barcodeReader = new BarcodeReader();
                // decode the current frame
                
                var result = barcodeReader.Decode(camTexture.GetPixels32(), camTexture.width, camTexture.height);

                if (result != null)
                {
                    //Debug.Log("DECODED TEXT FROM QR: " + result.Text);
                    ResultPoint[] resultPoints = result.ResultPoints;
                    //Debug.Log(resultPoints.Length);
                    //Debug.Log("resultPoints:");
                    int cv = 0;
                    for (int i = 0; i < resultPoints.Length; i++)
                    {
                        ResultPoint resultPoint = resultPoints[i];
                        outPoints[cv] = resultPoint.X;
                        cv++;
                        outPoints[cv] = resultPoint.Y;
                        cv++;
                        Debug.Log("  [" + i + "]:" + " x = " + resultPoint.X + ", y = " + resultPoint.Y);
                    }
                    Point2f p1 = new Point2f(resultPoints[0].X, resultPoints[0].Y);
                    Point2f p2 = new Point2f(resultPoints[1].X, resultPoints[1].Y);
                    Point2f p3 = new Point2f(resultPoints[2].X, resultPoints[2].Y);
                    Point2f p4 = new Point2f(resultPoints[3].X, resultPoints[3].Y);
                    //Mat CP = camera_parameters.calculate3DPointFrom2D(resultPoints[0].X,resultPoints[0].Y);
                    Point2f[] points = new Point2f[4] { p1, p2, p3, p4 };
                    writeToFile(points);
                    Mat TSR = camera_parameters.calculate3DFrom2D(points);
                    //Debug.Log(TSR);
                    cube_script.updatePosition(TSR, p1);
                }

            }
        }
        catch (Exception ex) { Debug.LogWarning(ex.Message); }

    }

    void writeToFile(Point2f[] points)
    {
        if(!isInicialized)
            using (System.IO.StreamWriter file =
                new System.IO.StreamWriter(@"C:\Users\User\Documents\GitHub\AR-Oktatasi-Segedeszkoz-Meta-2-Hasznalataval\out.txt"))
            {
                foreach (float line in outPoints)
                {
                    file.WriteLine(line);
                }
                isInicialized = true;
            }
    }

}
