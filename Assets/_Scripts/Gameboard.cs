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
        public Sprite tempSprite;
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

            GameManager.instance.gameboard = this;
        }

        public int GetIndex(int x, int y)
        {
            return BoardWidth * y + x;
        }

        public void PlaceNode(bool temp, int player, int index)
        {
            var node = _nodePool[index];
            if (!node.Enabled) return;
            node.SetSprite(temp ? tempSprite : ((player & 1) == 0 ? blackSprite : whiteSprite));
            node.SetEnabled(false);
            
        }
    }
}