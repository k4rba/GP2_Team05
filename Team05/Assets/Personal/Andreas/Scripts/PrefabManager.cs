using UnityEngine;

namespace Andreas.Scripts
{
    /// <summary>
    /// temporary prefab container/manager
    /// </summary>
    public class PrefabManager : MonoBehaviour
    {
        public static PrefabManager Get { get; private set; }

        public GameObject Glob;
        
        public void Awake()
        {
            if(Get == null)
            {
                Get = this;
            }
            else
            {
                Destroy(this);
                return;
            }
        }
        
    }
}