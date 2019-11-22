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
        public int index;

        // Start is called before the first frame update
        void Start()
        {

        }

        // Update is called once per frame
        void Update()
        {

        }

        public void Init(Gameboard board, int index)
        {
            this.index = index;
            button.onClick.AddListener(() => board.PlaceNode(index));
        }

        public void SetSprite(Sprite sprite)
        {
            image.sprite = sprite;
        }

        public void SetEnabled(bool value)
        {
            image.enabled = value;
        }
    }
}