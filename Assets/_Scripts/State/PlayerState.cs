using System;

namespace Omok.State
{
    [Serializable]
    public class PlayerState
    {
        public (int x, int y) Location;
        public int HP;
        public bool Dead => (HP <= 0);
    }
}
