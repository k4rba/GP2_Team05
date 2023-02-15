using System.Collections.Generic;
using Joakim.Scripts.Mechanics;
using UnityEngine;

public class FuseBoxMultipleCoilsOnly : MonoBehaviour {
    public List<PowerCoil> powerCoilsRequiredToOpen = new List<PowerCoil>();
    public List<GameObject> doorsToOpenUponFinished;


    public void CheckIfFinished() {
        Debug.Log(powerCoilsRequiredToOpen[0].done);
        Debug.Log(powerCoilsRequiredToOpen[1].done);
        if (powerCoilsRequiredToOpen[0].done && powerCoilsRequiredToOpen[1].done) {
            foreach (var gobject in doorsToOpenUponFinished) {
                gobject.gameObject.SetActive(false);
            }
        }
    }
}
