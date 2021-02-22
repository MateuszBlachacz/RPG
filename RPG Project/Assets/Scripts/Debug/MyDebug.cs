using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RPG
{
    public class MyDebug
    {
        public static void info(Object obj = null, params (string Key, object Value)[] pairs)
        {
            foreach(var pair in pairs)
            {   
                if (obj) Debug.Log($"Debug messge in {obj}: {pair.Key}:{pair.Value}");
                else Debug.Log($"{pair.Key}:{pair.Value}");
            }
        }

        public static void error(Object obj = null, params (string Key, object Value)[] pairs)
        {
            foreach (var pair in pairs)
            {
                if (obj) Debug.LogError($"Error in {obj}: {pair.Key}:{pair.Value}");
                else Debug.LogError($"{pair.Key}:{pair.Value}");
            }
        }
    }
}
