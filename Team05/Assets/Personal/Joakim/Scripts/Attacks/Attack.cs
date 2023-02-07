using UnityEngine;

namespace AttackNamespace {
    public class Attack {
        public interface IAttack {
            public float BasicDamage { get; set; }
            public float SpecialDamage { get; set; }
            public float ProjectileSpeed { get; set; }
            public float AttackSize { get; set; }
            public void DoDamage(float value);
        }

        public interface IPlayerAttacker {
            public float AttackSpeed { get; set; }
            public float AbilityACooldown { get; set; }
            public float AbilityXCooldown { get; set; }
            public float AbilityBCooldown { get; set; }
        }

        public interface IEnemyAttacker {
            //todo: stuff that affect enemy dmg etc
        }
    }
}