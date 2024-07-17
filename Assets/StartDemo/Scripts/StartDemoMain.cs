using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StartDemoMain : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
       var cfgData =  ConfigManager.Get<ExecelDemoConfig>();
       Debug.Log(cfgData.GetStudent(19911305));
       // ConfigManager.UnLoad<ExecelDemoConfig>();
       // Debug.Log(cfgData.GetStudent(19911305));
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnApplicationQuit()
    {
        ConfigManager.CleanAll();
    }
}
