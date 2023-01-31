namespace Personal.Andreas.Scripts.Flowfield
{
    public struct VNode
    {
        public ushort Distance;
        public bool LineOfSight;
        public ushort PathId;

        public void SetDistance(ushort dis, ushort id)
        {
            Distance = dis;
            PathId = id;
        }

        public void Reset()
        {
            Distance = 0;
            LineOfSight = false;
        }

        public static readonly VNode Empty = new VNode();
    }
}