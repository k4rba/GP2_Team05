using UnityEngine;

namespace Andreas.Scripts
{
    public class PlayerSpawnPoints : MonoBehaviour
    {
        private Transform _player1;
        private Transform _player2;

        private void Awake()
        {
            SetPlayers();
        }

        private void SetPlayers()
        {
            var children = GetComponentsInChildren<Transform>();
            _player1 = children[1];
            _player2 = children[2];
        }

        private void OnDrawGizmos()
        {
            if(_player1 == null || _player2 == null)
            {
                SetPlayers();
                return;
            }

            Gizmos.color = Color.cyan;
            Gizmos.DrawWireSphere(_player1.position, 0.5f);
            Gizmos.DrawWireSphere(_player2.position, 0.5f);
            Gizmos.color = Color.white;
        }
    }
}