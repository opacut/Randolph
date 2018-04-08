using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Switch : MonoBehaviour {
    public bool On;
    public List<SpikeTrap> Spikes;

    private void OnTriggerEnter2D(Collider2D other) {
        On = !On;
        Spikes.ForEach(y => y.Toggle());
    }
    // Use this for initialization
    void Start() {

    }

    // Update is called once per frame
    void Update() {

    }
}