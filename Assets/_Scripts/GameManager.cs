using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using Nekoyume;
using System;
using LibplanetUnity;

public class GameManager : Nekoyume.MonoSingleton<GameManager>
{
    public Text textField;
    public string currentSession;
    public SessionUI sessionUI;

    public override void Awake()
    {
        base.Awake();
    }

    // Start is called before the first frame update
    void Start()
    {
        textField.text = "에이전트 준비중...";
        Agent.Initialize();
        Agent.instance.PreloadEnded += LoadEnded;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void LoadEnded(object target, EventArgs args)
    {
        SceneManager.LoadScene("EnterSession");
    }
}
