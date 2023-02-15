using Andreas.Scripts;
using Andreas.Scripts.RopeSystem;
using Andreas.Scripts.RopeSystem.SegmentStates;
using Andreas.Scripts.StateMachine;
using UnityEngine;

public class RopeSegment : MonoBehaviour
{
    private StatesManager _stateManager;

    public Vector3 BaseScale;
    public Color BaseColor;

    private RealRope _rope;
    
    private void Awake()
    {
        _stateManager = new();
        BaseScale = transform.localScale;
        BaseColor = GetComponent<MeshRenderer>().material.color;
    }

    public void AddState(RopeSegmentStateBase state)
    {
        if(_rope == null)
            _rope = GameManager.Instance.RopeManager.Rope;
        
        state.Segment = this;
        state.Rope = _rope;
        _stateManager.AddState(state);
    }

    private void Update()
    {
        _stateManager.Update(Time.deltaTime);
    }

    private void FixedUpdate()
    {
        _stateManager.Update(Time.fixedDeltaTime);
    }
}
