using UnityEngine;

namespace Andreas.Scripts.PlayerData
{
    [CreateAssetMenu(fileName = "new Player Sfx Data",
        menuName = "GP2/PlayerSfxData", order = 0)]
    public class PlayerSfxData : ScriptableObject
    {
        /// <summary>
        /// aka WOOSH
        /// </summary>
        public AudioClip BasicAttack;
        
        public AudioClip SecondaryAttack;   //  shield slam / tether explosion
        public AudioClip SpecialAttack;     //  shield dome / tether stun

        public AudioClip AttackHit;
        
    }
}