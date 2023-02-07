using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestroyAfterXTime : MonoBehaviour {
    public float time;

    private void Start() {
        StartCoroutine(DestroyAfterTime());
    }

    IEnumerator DestroyAfterTime() {
        yield return new WaitForSeconds(time);
        Destroy(this.gameObject);
    }
}
