
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Runtime.InteropServices;

public class dll : MonoBehaviour
{
    [DllImport("SOLVEPNPDLL.dll", EntryPoint = "fnSolvePnPDll")]
    public static extern int fnSolvePnPDll();


    //public class BindingDllClass
    //{
    //    [DllImport("SOLVEPNPDLL.dll")]
    //    public static extern double Add(double a, double b);
    //}

    // Start is called before the first frame update
    void Start()
    {
        Debug.Log("DLL CODE: " + fnSolvePnPDll());
        //Debug.Log("DLL CODE: "+ BindingDllClass.Add(2,3));
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
