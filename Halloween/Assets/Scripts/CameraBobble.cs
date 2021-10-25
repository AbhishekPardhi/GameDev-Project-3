using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraBobble : MonoBehaviour
{
    [SerializeField] private float camAmp;
    [SerializeField] private float eyeHeight;
    [SerializeField] private float smoothness;
    [SerializeField] private bool cr_Running = false;
    [SerializeField] private Vector3 defaultPosition;

    void Start() {
        defaultPosition = new Vector3(0, eyeHeight, 0);
    }
    void Update()
    {
        
    }

    public void bobble() {
        if(cr_Running)
            StopAllCoroutines();
        cr_Running = false;
        float y = Mathf.PingPong(Time.time, camAmp);
        transform.localPosition = new Vector3(0, y + (eyeHeight - camAmp/2), 0);
    }

    public void returnToEyeHeight() {
        if(Mathf.Abs(transform.localPosition.y - eyeHeight) > 0.001f && !cr_Running) {
            StartCoroutine(fixHeight());
        }
    }

    IEnumerator fixHeight() {
        while(Mathf.Abs(transform.localPosition.y - eyeHeight) > 0.001f) {
            cr_Running = true;
            transform.localPosition = Vector3.Lerp(transform.localPosition, defaultPosition, smoothness*Time.deltaTime);
            yield return null;
        }

        cr_Running = false;
    }
}
