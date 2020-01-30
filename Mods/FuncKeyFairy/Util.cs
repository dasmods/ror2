using System;
using System.Collections.Generic;
using UnityEngine;


namespace Dasmods
{
    class Util
    {
        public static T pickRand<T>(List<T> list)
        {
            int randNdx = UnityEngine.Random.Range(0, list.Count);
            return list[randNdx];
        }
    }
}
