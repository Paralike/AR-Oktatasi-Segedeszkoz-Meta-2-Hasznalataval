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
    
    //private Rect screenRect;
    private int frames = 0;
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
        Cube = GameObject.FindGameObjectWithTag("Cube1");
        cube_script = Cube.GetComponent<ChangeCubeColor>();
    }
	
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
                    //Debug.Log("resultPoints:");
                    for (int i = 0; i < resultPoints.Length; i++)
                    {
                        ResultPoint resultPoint = resultPoints[i];
                        //Debug.Log("  [" + i + "]:"+ " x = " + resultPoint.X + ", y = " + resultPoint.Y);
                    }
                    cube_script.updatePosition(resultPoints[0].X, resultPoints[0].Y);
                }
            }
        }
        catch (Exception ex) { Debug.LogWarning(ex.Message); }

    }
}
