using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

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
    }

    private void Notify(string content)
    {
        NotificationPanel.SetActive(true);
        NotificationPanel.transform.Find("Text").GetComponent<Text>().text = content;
    }
}
