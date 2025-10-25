using System.Collections.Generic;
using UnityEngine;

namespace Game._Scripts.Utils
{
    public class Helpers
    {
        private static readonly Dictionary<float, WaitForSeconds> WaitDictionary = new();

        public static WaitForSeconds GetWait(float seconds)
        {
            if (WaitDictionary.TryGetValue(seconds, out var wait)) return wait;
            
            WaitDictionary[seconds] = new WaitForSeconds(seconds);
            return WaitDictionary[seconds];
        }
    }
}