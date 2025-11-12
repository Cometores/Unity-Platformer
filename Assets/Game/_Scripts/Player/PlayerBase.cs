using UnityEngine;

namespace Game._Scripts.Player
{
    public abstract class PlayerBase: MonoBehaviour
    {
        public abstract void DisableInput();
        public abstract void EnableInput();
    }
}