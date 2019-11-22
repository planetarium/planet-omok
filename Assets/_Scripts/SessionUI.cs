using Nekoyume.BlockChain;
using UnityEngine;
using UnityEngine.UI;
using Libplanet.Action;
using UniRx;
using Nekoyume.State;
using Nekoyume.Action;
using UnityEngine.SceneManagement;

namespace Omok.UI
{
    public class SessionUI : MonoBehaviour
    {
        public Text SessionTextField;
        public Button EnterButton;

        public GameObject NotificationPanel;
        //private string target;

        // Start is called before the first frame update
        void Start()
        {
            NotificationPanel.SetActive(false);
            EnterButton.onClick.AddListener(ClickHandler);
        }

        private void ClickHandler()
        {
            Notify("세션 참여 중입니다");
            GameManager.instance.sessionUI = this;
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

        private void SubscribeJoinSession(string id)
        {
            States.Instance.sessionState.ObserveOnMainThread().Subscribe(state => { UpdateUI(state, id); });
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
                if (state.sessions[target].Count == 2)
                {
                    SceneManager.LoadScene("SampleScene");
                }

                var content = $"세션 {target}";
                foreach (var addr in state.sessions[target])
                {
                    content += $"\n{addr.ToString()}";
                }

                Notify(content);
            }
            else
            {
                Notify("해당 세션을 생성하는 데 실패했습니다.");
            }
        }
    }
}