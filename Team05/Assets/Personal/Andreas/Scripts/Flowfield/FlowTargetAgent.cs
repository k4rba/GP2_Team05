﻿using FlowFieldSystem;
using UnityEngine;
using Util;

namespace Andreas.Scripts.Flowfield
{
    public class FlowTargetAgent : MonoBehaviour
    {
        [SerializeField] private FlowFieldManager _fieldManager;
        public FlowFieldManager GetFieldManagerManager => _fieldManager;

        private Vector2Int prevPos;

        private Timer _updateInterval = 0.1f;

        private void Update()
        {
            if(_updateInterval.UpdateTick())
            {
                var position = transform.position;
                var field = _fieldManager.GetField();
                CoordinateHelper.PositionToWorldCoords(position.x, position.y, field.TileSize, out int x, out int y);
                var point = new Vector2Int(x, y);
                if(point != prevPos)
                {
                    prevPos = point;
                    field.UpdateField(new Vector2(position.x, position.z));
                }
            }
        }
    }
}