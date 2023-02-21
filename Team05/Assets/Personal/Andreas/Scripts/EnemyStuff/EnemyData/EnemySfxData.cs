using System;
using AudioSystem;
using UnityEngine;
using Util;

namespace Andreas.Scripts.EnemyData
{
    [CreateAssetMenu(fileName = "new Enemy Sfx Data",
        menuName = "GP2/EnemySfxData", order = 0)]
    public class EnemySfxData : ScriptableObject
    {
        public AudioData Attack;
        public AudioData SpecialAttack;
        
        [Space(10)]
        
        public AudioData OnDeath;
        public AudioData OnAggro;
        public AudioData OnHint;
        public AudioData OnKill;
    }
    
    [Serializable]
    public class AudioData
    {
        public static implicit operator AudioClip[](AudioData data) => data.Audios;

        [Range(1, 100)] public int Chance = 100;
        public AudioClip[] Audios;
        
        public AudioClip GetRandomAudio() => Audios.RandomItem();

        public void Play(Vector3 pos)
        {
            if(Audios.Length == null || Audios.Length <= 0)
                return;

            if(!Rng.Roll(Chance))
                return;
            
            var audio = Audios.RandomItem();
            AudioManager.PlaySfx(audio.name, pos);
        }

        public void Stop()
        {
            //  todo
            //AudioManager.StopSfx();
        }
        
    }
}