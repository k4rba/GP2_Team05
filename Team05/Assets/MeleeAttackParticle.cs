using System;
using Unity.VisualScripting;
using UnityEngine;
using DG.Tweening;

public class MeleeAttackParticle : MonoBehaviour {
    public GameObject _target;
    
    private void Start() {
        transform.DOMove(_target.transform.position, 0.1f);
    }
}