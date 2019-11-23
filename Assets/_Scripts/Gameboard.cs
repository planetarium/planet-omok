using Omok.BlockChain;
using UnityEngine;
using UnityEngine.UI;

namespace Omok.Game
{
    public class Gameboard : MonoBehaviour
    {
        public const int BoardWidth = 14;
        public const int BoardHeight = 13;

        public OmokNode nodePrefab;
        public Text infoText;
        public Sprite tempSprite;
        public Sprite blackSprite;
        public Sprite whiteSprite;
        public Button resignButton;
        public Transform layout;
        public bool finished;

        private readonly OmokNode[] _nodePool = new OmokNode[BoardWidth * BoardHeight];
        
        void Awake()
        {
            for (int i = 0; i < _nodePool.Length; ++i)
            {
                var node = _nodePool[i] = Instantiate(nodePrefab, layout);
                node.SetEnabled(true);
                node.Init(this, i);
            }

            finished = false;
            resignButton.onClick.AddListener(Resign);
            GameManager.instance.gameboard = this;
            UpdateInfo();
        }

        private void OnDisable()
        {
            GameManager.instance.gameboard = null;
        }

        public void UpdateInfo()
        {
            infoText.text = GameManager.instance.isMyTurn ? "My Turn" : "Waiting For Other Player...";
        }

        public void SetResult(bool win)
        {
            resignButton.onClick.RemoveAllListeners();
            finished = true;
            infoText.text = win ? "You Win" : "You Lose";
        }

        public int GetIndex(int x, int y)
        {
            return BoardWidth * y + x;
        }

        public void PlaceNode(bool temp, int player, int index)
        {
            if (finished || (temp && !GameManager.instance.isMyTurn)) return;
            Debug.Log($"PlaceNode {temp}, {player}, {index}");
            var node = _nodePool[index];
            if (temp && !node.Enabled) return;
            node.SetSprite(temp ? tempSprite : (player == 0 ? blackSprite : whiteSprite));
            node.SetEnabled(false);
            if (temp)
            {
                GameManager.instance.SetMyTurn(false);
                ActionManager.instance.Place(index);
            }
        }

        public void Resign()
        {
            ActionManager.instance.Resign();
            SetResult(false);
        }
    }
}