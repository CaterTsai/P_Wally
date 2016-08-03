using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Threading;

public class fileWatcher : MonoBehaviour {

    enum eFileType
    {
        eFile_Wally =   0
       , eFile_Mark
    }

    public wallyMgr _wallyMgr = null;
    private FileSystemWatcher _watch;
    
    private Queue<string> _orderList = new Queue<string>();

    static private Object _lock = new Object();

    private bool _isProcessing = false;
    private int _loadCount = 0;
    private Texture _tempMark = null;
    private Texture _tempWally = null;

    //Config
    private string _exFolderPath = "";

    #region Basic Method
    //------------------------------------------------
    // Use this for initialization
    void Start()
    {
        if(loadConfig())
        {
            _watch = new FileSystemWatcher();
            _watch.NotifyFilter = NotifyFilters.LastAccess | NotifyFilters.LastWrite;
            _watch.Filter = "*.order";
            _watch.Path = _exFolderPath;
            _watch.Created += onCreated;

            _watch.EnableRaisingEvents = true;
            _watch.IncludeSubdirectories = true;
        }
    }

    //------------------------------------------------
    void Update()
    {
        if(!_isProcessing && _orderList.Count > 0)
        {
            string orderName_ = "";
            lock (_lock)
            {
                orderName_ = _orderList.Dequeue();
            }
            if(orderName_ != null)
            {
                _loadCount = 2;
                _isProcessing = true;
                StartCoroutine(loadResource(orderName_, "file:///" + _exFolderPath + orderName_ + "_1.png", eFileType.eFile_Wally));
                StartCoroutine(loadResource(orderName_, "file:///" + _exFolderPath + orderName_ + "_2.png", eFileType.eFile_Mark));
                
            }
        }
    }


    //------------------------------------------------
    void OnApplicationQuit()
    {
        _watch.Created -= onCreated;
        _watch = null;
    }

    #endregion

    #region Image Resource
    //------------------------------------------------
    private void onCreated(object source, FileSystemEventArgs e)
    {
        int start_ = e.FullPath.LastIndexOf("\\") + 1;
        int end_ = e.FullPath.LastIndexOf(".order");

        string name_ = e.FullPath.Substring(start_, end_ - start_);

        //Debug.Log("New order : " + name_);

        lock (_lock)
        {
            _orderList.Enqueue(name_);
        }
    }

    //------------------------------------------------
    private IEnumerator loadResource(string name, string path, eFileType type)
    {
        WWW www = new WWW(path);
        yield return www;

        switch (type)
        {
            case eFileType.eFile_Mark:
                {
                    _tempMark = www.texture;
                    break;
                }
            case eFileType.eFile_Wally:
                {
                    _tempWally = www.texture;
                    break;
                }
            default:
                {
                    break;
                }
        }

        www.Dispose();
        www = null;

        _loadCount--;

        //load
        if (_loadCount == 0)
        {
            _isProcessing = false;
            _wallyMgr.newWally(ref _tempMark, ref _tempWally);
        }
    }
    #endregion

    #region Config file
    private bool loadConfig()
    {
        string configPath_ = Application.dataPath + "/StreamingAssets/";
        string configJson_ = System.IO.File.ReadAllText(configPath_ + "_config.json");

        JSONObject configObj_ = new JSONObject(configJson_);

        if(configObj_.GetField(ref _exFolderPath, "path"))
        {
            Debug.Log("[fileWatcher]loadConfig success. Path : " + _exFolderPath);
            return true;
        }
        else
        {
            Debug.Log("[fileWatcher]loadConfig failed");
            return false;
        }
        
    }
    #endregion
}