using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneNameCont
{
    public static string TitleScene = "TitleScene";
    public static string LoadingScene = "LoadingScene";
    public static string PlayerchoiceScene = "PlayerchoiceScene";
    public static string Stage1Scene = "Stage1Scene";
    public static string Stage2Scene = "Stage2Scene";
    public static string Stage3Scene = "Stage3Scene";
    public static string GameOverScene = "GameOverScene";
}

public class SceneController : MonoBehaviour
{
    static SceneController instance;
    public static SceneController Instance
    {
        get
        {
            return instance;
        }
    }


    private void Awake()
    {
        if (instance == null)
            instance = this;
        DontDestroyOnLoad(this);
    }

    public string NextSceneName { get; set; }
    public bool IsFin { get; set; }
    public float Progress { get; set; }

    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    public void GotoMainScene()
    {
        SceneManager.LoadScene(SceneNameCont.TitleScene);
    }

    public void ChangeLoadingScene(string _nextSceneName)
    {
        NextSceneName = _nextSceneName;
        SceneManager.LoadScene(SceneNameCont.LoadingScene);
    }

    public void CallNextScene()
    {
        StartCoroutine("AsyncSceneLoad");
    }

    IEnumerator AsyncSceneLoad()
    {
        AsyncOperation asyncOperation = SceneManager.LoadSceneAsync(NextSceneName, LoadSceneMode.Single);

        while(asyncOperation.isDone)
        {
            //IsFin = asyncOperation.isDone;
            //Progress = asyncOperation.progress;

            yield return null;
        }

        Debug.Log("Scene Load Coplete!! : " + NextSceneName);
    }
}
