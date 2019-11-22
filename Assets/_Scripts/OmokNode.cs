using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Omok.Game
{
    public class OmokNode : MonoBehaviour
    {
        public Image image;
        public Button button;
        public int index = -2;

        public void Init(Gameboard board, int index)
        {
            this.index = index;
            button.onClick.AddListener(() => board.PlaceNode(index));
        }

        public void SetSprite(Sprite sprite)
        {
            image.overrideSprite = sprite;
        }

        public void SetEnabled(bool value)
        {
            if (value)
            {
                index = -1;
            }
            else
            {
                index = -2;
                image.overrideSprite = null;
            }
        }
    }
}