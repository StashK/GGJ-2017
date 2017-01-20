using UnityEngine;
using System.Collections;

namespace JPL
{
    public class Errors : Mono
    {
        //private bool _initialized = false;
        //public string output;
        //public string stack;
        //public LogType type;

        //void Awake ()
        //{
        //    System.IO.File.WriteAllText(Application.dataPath + "/errors.txt", string.Empty);
        //    DontDestroyOnLoad(this.gameObject);
        //}

        //void OnEnable()
        //{
        //    Application.logMessageReceived += HandleLog;
        //}

        //void OnDisable()
        //{
        //    Application.logMessageReceived -= HandleLog;
        //}

        //void HandleLog(string logString, string stackTrace, LogType _type)
        //{
        //    output = logString;
        //    stack = stackTrace;
        //    type = _type;

        //    if (_type != LogType.Log)
        //    {
        //        if (!_initialized)
        //        {
        //            Save("*////////////////////// NEW GAME ////////////////////////*");
        //            Save("BUILD: " + Core.buildnumber + "\nTime: " + System.DateTime.Now + "\n");

        //            _initialized = true;
        //        }

        //        Save("["+ _type + "] " + logString + " \n" + stackTrace + " ");
        //    }
        //}

        //void Save(string data)
        //{
        //    System.IO.File.AppendAllText(Application.dataPath+"/errors.txt", "\n"+data);
        //}
    }
}
