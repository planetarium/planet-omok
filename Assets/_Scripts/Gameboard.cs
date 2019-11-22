using Omok.BlockChain;
using Omok.State;
using UnityEngine;

namespace Omok.Game
{
    public class Gameboard : MonoBehaviour
    {
        public const int BoardWidth = 14;
        public const int BoardHeight = 13;

        public OmokNode nodePrefab;
        public Sprite blackSprite;
        public Sprite whiteSprite;
        public Transform layout;

        private readonly OmokNode[] _nodePool = new OmokNode[BoardWidth * BoardHeight];
        
        void Awake()
        {
            for (int i = 0; i < _nodePool.Length; ++i)
            {
                var node = _nodePool[i] = Instantiate(nodePrefab, layout);
                node.SetEnabled(true);
                node.Init(this, i);
            }
            States.Instance.GameState = new GameState(GameState.Address);
        }

        public int GetIndex(int x, int y)
        {
            return BoardWidth * y + x;
        }

        int i = 0;
        public void PlaceNode(int index)
        {
            var node = _nodePool[index];
            if (node.index == -1) return;

            node.SetSprite((i++ & 1) == 0 ? blackSprite : whiteSprite);
            node.SetEnabled(true);
        }
    }
}