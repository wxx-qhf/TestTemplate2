using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
/// <summary>
/// 单例脚本用来全局切换场景
/// </summary>
public class SceneChange : MonoBehaviour
{
    public static SceneChange Instance;
    public int SceneTotalCount;
    public KeyCode[] InputKey;
    //public void Awake()
    //{

    //}
    //// Start is called before the first frame update

    void Start()
    {
        if (Instance != null)
        {
            GameObject.Destroy(this);
            return;
        }
        else
        {
            Instance = this;
            DontDestroyOnLoad(this);
            SceneTotalCount = GetAllScenePath();
            InputKey = GetKeyCodes(SceneTotalCount);
            AddCamraControl();
        }
    }
    public int GetAllScenePath()
    {
        return SceneManager.sceneCountInBuildSettings;
    }

    public KeyCode[] GetKeyCodes(int count)
    {
        List<KeyCode> keys = new List<KeyCode>();
        for (int i = 0; i < count; i++)
        {
            string name = $"Alpha{i}";
            KeyCode key = (KeyCode)System.Enum.Parse(typeof(KeyCode), name);
            keys.Add(key);
        }
        return keys.ToArray();
    }

    // Update is called once per frame
    void Update()
    {
        OnWaitKeyCodePress();
    }
    bool isShowText = false;
    float currentTime = 3;
    string DebugContent;
    private void OnGUI()
    {

    }
    bool isLoading = false;
    private void OnWaitKeyCodePress()
    {
        if (isLoading) return;
        for (int i = 0; i < InputKey.Length; i++)
        {
            if (Input.GetKeyDown(InputKey[i]))
            {
                ChangeScene(i);
                break;
            }
        }
    }
    private void ChangeScene(int buildIndex)
    {
        var loadAsy = SceneManager.LoadSceneAsync(buildIndex);
        loadAsy.completed += OnChangeSceneComplete;
    }
    private void OnChangeSceneComplete(AsyncOperation operation)
    {
        if (operation.isDone)
        {
            operation.completed -= OnChangeSceneComplete;
            operation.allowSceneActivation = true;
            isLoading = false;

            AddCamraControl();
        }
        else
        {
            Debug.Log("loading-------");
        }
    }

    public void AddCamraControl()
    {
        var came = GameObject.FindObjectOfType<UnityTemplateProjects.SimpleCameraController>();
        if (came == null)
        {
            Camera camera = Camera.main == null ? FindObjectOfType<Camera>() : Camera.main;
            camera.gameObject.AddComponent<UnityTemplateProjects.SimpleCameraController>();
        }
    }
}
