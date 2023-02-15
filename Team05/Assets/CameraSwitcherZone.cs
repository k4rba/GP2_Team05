using System;
using System.Collections;
using System.Collections.Generic;
using Andreas.Scripts;
using Cinemachine;
using TMPro;
using UnityEngine;

public class CameraSwitcherZone : MonoBehaviour
{
    public CinemachineVirtualCamera Camera;
    private void OnTriggerEnter(Collider other)
    {
        var player = other.GetComponent<Player>();
        if (player)
        {
            GameManager.Instance.DollyManager.SetActiveCamera(Camera);
        }
    }
}
