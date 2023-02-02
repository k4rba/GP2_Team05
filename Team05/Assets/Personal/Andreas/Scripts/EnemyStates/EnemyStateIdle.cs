using Andreas.Scripts.EnemyStates.EnemyModes;

namespace Andreas.Scripts.EnemyStates
{
    public class EnemyStateIdle : EnemyState
    {
        public override void Start()
        {
            base.Start();
            SetCommand(new EnemyCommandScanForPlayer());
        }
    }
}