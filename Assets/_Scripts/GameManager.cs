using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using Nekoyume;
using Nekoyume.BlockChain;
using System;

public class GameManager : MonoSingleton<GameManager>
{
    public Text textField;
    public string currentSession;

    protected override void Awake()
    {
        base.Awake();
    }

    // Start is called before the first frame update
    void Start()
    {
        textField.text = "에이전트 준비중...";
        AgentController.Initialize(AgentInitialized, textField, LoadEnded);
    }

    // Update is called once per frame
    void Update()
    {
        
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
