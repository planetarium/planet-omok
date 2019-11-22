using UnityEngine.UI;
using UnityEngine.SceneManagement;
using Omok;
using System;
using Omok.UI;
using LibplanetUnity;

public class GameManager : Omok.MonoSingleton<GameManager>
{
    public Text textField;
    public string currentSession;
    public SessionUI sessionUI;

    public override void Awake()
    {
        base.Awake();
    }

    void Start()
    {
        textField.text = "에이전트 준비중...";
        Agent.Initialize();
        Agent.instance.PreloadEnded += LoadEnded;
    }

    private void AgentInitialized(bool succeed)
    {
        if (succeed)
        {
            textField.text = "에이전트 준비 성공";
        }
        else
        {
            textField.text = "에이전트 준비 실패";
        }
    }

    private void LoadEnded(object target, EventArgs args)
    {
        SceneManager.LoadScene("EnterSession");
    }
}
