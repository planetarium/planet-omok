using Nekoyume.BlockChain;
using UnityEngine;
using UnityEngine.UI;
using Libplanet.Action;
using UniRx;
using Nekoyume.State;
using Nekoyume.Action;

public class SessionUI : MonoBehaviour
{
    public Text SessionTextField;
    public Button EnterButton;
    public GameObject NotificationPanel;

    // Start is called before the first frame update
    void Start()
    {
        NotificationPanel.SetActive(false);
        EnterButton.onClick.AddListener(ClickHandler);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void ClickHandler()
    {
        Notify(SessionTextField.text);
        ActionManager.instance.JoinSesion(SessionTextField.text)
            .Subscribe(eval =>
            {
                Debug.LogWarning(((SessionState)eval.OutputStates.GetState(SessionState.Address)).sessions.Count);
            });
    }

    private void Notify(string content)
    {
        if(!NotificationPanel.activeSelf)
            NotificationPanel.SetActive(true);
        NotificationPanel.transform.Find("Text").GetComponent<Text>().text = content;
    }

    private void JoinHandler(object target, IActionContext args)
    {
        NotificationPanel.transform.Find("Text").GetComponent<Text>().text = "액션 실행 됨! 와!";
    }
}
