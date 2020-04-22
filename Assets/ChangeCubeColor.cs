using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChangeCubeColor : MonoBehaviour {

	// Use this for initialization
	void Start () {
        Debug.Log("initcor: x: " + transform.position.x + " y: " + transform.position.y + " z: " + transform.position.z);

    }
	
	// Update is called once per frame
	void Update () {
		
	}

    public void updatePosition(float compx, float compy)
    {
        Debug.Log("Transform elott x: " + transform.position.x + " y: " + transform.position.y + " z: " + transform.position.z);
        transform.position = new Vector3(compx, compy, 0);
        Debug.Log("Transform utan: x: " + transform.position.x + " y: " + transform.position.y + " z: " + transform.position.z);
    }
}
