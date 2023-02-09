using System.Collections.Generic;
using UnityEngine;

namespace Util
{
    public static class FastResources
    {
        private static Dictionary<string, Object> _resources = new();

        public static T Load<T>(string path) where T : Object
        {
            if(_resources.TryGetValue(path, out Object retValue))
            {
                return retValue as T;
            }

            retValue = Resources.Load<T>(path);

            if(retValue == null)
            {
                Debug.Log($"Could not load '{path}' - not found");
                return null;
            }
            
            _resources.Add(path, retValue);

            return retValue as T;
        }
        
    }
}