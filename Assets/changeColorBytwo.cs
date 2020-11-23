using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class changeColorBytwo : MonoBehaviour
{
    GameObject Cube1;
    GameObject Cube2;
    ChangeCubeColor cube1_script;
    ChangeCubeColor cube2_script;
    public Renderer myRenderer;
    // Start is called before the first frame update
    void Start()
    {
        Cube1 = GameObject.FindGameObjectWithTag("Cube1");
        cube1_script = Cube1.GetComponent<ChangeCubeColor>();
        Cube2 = GameObject.FindGameObjectWithTag("Cube2");
        cube2_script = Cube2.GetComponent<ChangeCubeColor>();
        myRenderer = gameObject.GetComponent<Renderer>();
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 pos1 = cube1_script.getPos();
        Vector3 pos2 = cube2_script.getPos();
        float distance =(float) Math.Sqrt( (pos1.x-pos2.x)* (pos1.x - pos2.x) + (pos1.y - pos2.y) * (pos1.y - pos2.y) + (pos1.z - pos2.z) * (pos1.z - pos2.z));
        if (distance < 0.1)
        {
            foreach (var m in myRenderer.materials)
            {
                if (m.name.Contains("Meta"))
                {
                    m.color = new Color(0.5f,0f,0.5f,1f);
                }
            }
        }
        else
        {
            foreach (var m in myRenderer.materials)
            {
                if (m.name.Contains("Meta"))
                {
                    m.color = new Color(0.2235294f, 0.4666667f, 0.6705883f, 1f);
                }
            }
        }

    }
}
