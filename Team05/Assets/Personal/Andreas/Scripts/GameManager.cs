using Personal.Andreas.Scripts;
using UnityEngine;

namespace Andreas.Scripts
{
    public class GameManager : MonoBehaviour
    {
        public static GameManager Instance { get; private set; }

        public EnemyManager EnemyManager;
        public CharacterManager CharacterManager;
        public PlayerJoinManager PlayerJoinManager;
        public CameraTopDownController CameraController;
        public WorldManager WorldManager;
        
        private void Awake()
        {
            if(Instance == null)
            {
                Instance = this;
                DontDestroyOnLoad(gameObject);
            }
            else
            {
                Destroy(gameObject);
                return;
            }
        }
    }
}