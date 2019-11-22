using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Omok.Game
{
    public class Gameboard : MonoBehaviour
    {
        public const int BoardWidth = 13;
        public const int BoardHeight = 13;

        public readonly OmokNode[,] board = new OmokNode[BoardWidth, BoardHeight];
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
                node.SetEnabled(false);
                node.Init(this, i);
            }
            
        }

        // Update is called once per frame
        void Update()
        {

        }

        public int GetIndex(int x, int y)
        {
            return BoardWidth * y + x;
        }

        int i = 0;
        public void PlaceNode(int index)
        {
            var node = _nodePool[index];
            node.SetSprite((i++ & 1) == 0 ? blackSprite : whiteSprite);
            node.SetEnabled(true);
        }
    }
}