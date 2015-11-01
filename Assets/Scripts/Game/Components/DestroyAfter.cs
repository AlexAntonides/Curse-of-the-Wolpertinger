using UnityEngine;
using System.Collections;

public class DestroyAfter : MonoBehaviour {

    public float seconds;
    private float timer;

	void Update () {
        timer += Time.deltaTime;

        if (timer > seconds)
        {
            Destroy(gameObject);
        }
	}
}
