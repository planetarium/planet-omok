using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System;
using Omok.UI;
using LibplanetUnity;
using Omok.Game;

public class GameManager : Omok.MonoSingleton<GameManager>
{
    public Text textField;
    public string currentSession;
    public SessionUI sessionUI;
    public bool isMyTurn;
    public Gameboard gameboard;

    public override void Awake()
    {
        base.Awake();
    }

    void Start()
    {
        textField.text = "Preparing agent...";
        Agent.Initialize();
        Agent.instance.PreloadEnded += LoadEnded;
    }

    private void AgentInitialized(bool succeed)
    {
        if (succeed)
        {
            textField.text = "Agent is ready.";
        }
        else
        {
            textField.text = "Failed to initialize agent.";
        }
    }

    private void LoadEnded(object target, EventArgs args)
    {
        SceneManager.LoadScene("EnterSession");
    }

    public void SetMyTurn(bool value)
    {
        isMyTurn = value;
    }
}
