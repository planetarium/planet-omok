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
        public int Index { get; private set; } = -2;

        public void Init(Gameboard board, int index)
        {
            Index = index;
            Button.onClick.AddListener(() => board.PlaceNode(index));
        }

        public void SetSprite(Sprite sprite)
        {
            Image.overrideSprite = sprite;
        }

        public void SetEnabled(bool value)
        {
            if (value)
            {
                Index = -1;
            }
            else
            {
                Index = -2;
                Image.overrideSprite = null;
            }
        }
    }
}