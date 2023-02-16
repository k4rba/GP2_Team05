using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Andreas.Scripts.StateMachine.States
{
    struct MaterialColors
    {
        public Material Material;
        public Color Start;
        public Color End;

        public MaterialColors(Color start, Color end, Material material)
        {
            Start = start;
            End = end;
            Material = material;
        }
    }
    public class StateColorFlash : State
    {
        private float _timer = 0.1f;
        private Color _color;
        private GameObject _model;
        private List<MaterialColors> _materials;

        public StateColorFlash(GameObject model, Color color)
        {
            _model = model;
            _color = color;
            _materials = new();
        }

        public override void Start()
        {
            base.Start();

            var modelMaterials = _model.GetComponentsInChildren<MeshRenderer>().Select(x => x.material);
            
            foreach(var m in modelMaterials)
            {
                _materials.Add(new MaterialColors(_color, m.color, m));
            }
            
            SetColorStart();
        }

        public void SetColorStart()
        {
            foreach(var mat in _materials)
            {
                mat.Material.color = mat.Start;
            }
        }
        
        public void SetColorEnd()
        {
            foreach(var mat in _materials)
            {
                mat.Material.color = mat.End;
            }
        }
        

        public override void Update(float dt)
        {
            base.Update(dt);

            _timer -= dt;

            if(_timer <= 0)
                Exit();
        }

        public override void Exit()
        {
            base.Exit();
            SetColorEnd();
        }
    }
}