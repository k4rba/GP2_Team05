using System.Drawing;
using Andreas.Scripts.RopeSystem.SegmentStates;
using UnityEngine;
using Util;
using Color = UnityEngine.Color;

namespace Andreas.Scripts.RopeSystem.RopeStates
{
    public class RopeStateFlow : RopeStateBase
    {
        private int _startIndex;
        private int _endIndex;

        private int _index;
        private Timer _timer;

        private GameObject _objLight;

        public override void Start()
        {
            base.Start();
            // _objLight = new GameObject("RopeLight");
            var lightPrefab = FastResources.Load<GameObject>("Prefabs/Rope/RopeLight"); 
            _objLight = Object.Instantiate(lightPrefab, Rope.transform);
            
            // var light = _objLight.AddComponent<Light>();
            // light.color = Color.white;
            // light.type = LightType.Point;
            // light.intensity = 5f;
            // light.range = 20f;
            // light.shadows = LightShadows.Soft;
            _timer = Rope.Data.TransferSpeed / Rope.Segments.Count;
        }

        public override void Update(float dt)
        {
            base.Update(dt);

            foreach(var t in _timer.UpdateTicks())
            {
                SetRopeCool();
                _index++;
                if(_index >= Rope.Segments.Count)
                {
                    Exit();
                    break;
                }
            }
        }

        public void SetRopeCool()
        {
            var segment = Rope.Segments[_index].GetComponent<RopeSegment>();
            segment.GetComponent<Rigidbody>()
                .AddForce(Random.insideUnitSphere * Rng.NextF(3f, 10f), ForceMode.VelocityChange);
            segment.AddState(new RopeSegmentStateColor());
            segment.AddState(new RopeSegmentStateSize());

            _objLight.transform.position = segment.transform.position;
        }

        public override void Exit()
        {
            base.Exit();
            Object.Destroy(_objLight);
        }
    }
}