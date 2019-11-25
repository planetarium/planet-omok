using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Omok.Game
{
    public class OmokNode : MonoBehaviour
    {
        public Image Image;
        public Button Button;
        public int Index { get; private set; }
        public bool Enabled { get; private set; }

        public void Init(Gameboard board, int index)
        {
            Index = index;
            Button.onClick.AddListener(() => board.PlaceNode(true, -1, index));
        }

        public void SetSprite(Sprite sprite)
        {
            Image.overrideSprite = sprite;
        }

        public void SetEnabled(bool value)
        {
            Enabled = value;
        }
    }
}