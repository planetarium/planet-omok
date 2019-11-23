using Omok.BlockChain;
using UnityEngine;
using UnityEngine.UI;
using Omok.State;
using UnityEngine.SceneManagement;
using LibplanetUnity;

namespace Omok.UI
{
    public class SessionUI : MonoBehaviour
    {
        public Text SessionTextField;
        public Button EnterButton;

        public GameObject NotificationPanel;

        void Start()
        {
            NotificationPanel.SetActive(false);
            EnterButton.onClick.AddListener(ClickHandler);
        }

        private void ClickHandler()
        {
            if(SessionTextField.text == "")
            {
                Debug.LogError("Session ID should not be empty.");
                return;
            }

            GameManager.instance.sessionUI = this;
            Notify($"Joining Session: {SessionTextField.text}");
            ActionManager.instance.JoinSesion(SessionTextField.text);
        }

        private void OnDestroy()
        {
            GameManager.instance.sessionUI = null;
        }

        private void Notify(string content)
        {
            NotificationPanel.SetActive(true);
            NotificationPanel.transform.Find("Text").GetComponent<Text>().text = content;
        }

        public void UpdateUI(SessionState state, string target)
        {
            if (state is null)
            {
                Debug.LogWarning("State is null.");
                return;
            }

            if (state.sessions.ContainsKey(target))
            {
                if (state.sessions[target].Players.Count == 2)
                {
                    SceneManager.LoadScene("SampleScene");

                    var players = state.sessions[GameManager.instance.currentSession].Players;
                    if (!players.Contains(Agent.instance.Address))
                    {
                        Debug.LogError("Address does not exist in players.");
                    }

                    GameManager.instance.SetMyTurn(
                        state.sessions[GameManager.instance.currentSession].Turn ==
                        players.IndexOf(Agent.instance.Address));
                }

                var content = $"Session: {target}";
                foreach (var addr in state.sessions[target].Players)
                {
                    content += $"\n{addr.ToString()}";
                }

                Notify(content);
            }
            else
            {
                Notify($"Failed to create session: {target}");
            }
        }
    }
}