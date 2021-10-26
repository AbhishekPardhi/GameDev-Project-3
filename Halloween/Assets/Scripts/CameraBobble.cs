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
    private float m_Parameter;

    void Start() {
        defaultPosition = new Vector3(0, eyeHeight, 0.3f);
        m_Parameter = camAmp/4;
    }
    void Update()
    {

    }

    public void bobble() {
        if(cr_Running)
            StopAllCoroutines();
        cr_Running = false;

        m_Parameter += Time.deltaTime*smoothness;
        if(m_Parameter > camAmp)
            m_Parameter = m_Parameter - camAmp;

        applyPosition(m_Parameter);
    }

    public void returnToEyeHeight() {
        if(Mathf.Abs(transform.localPosition.x) > 0.001f && !cr_Running) {
            StartCoroutine(fixHeight());
        }
    }

    IEnumerator fixHeight() {
        while(Mathf.Abs(transform.localPosition.x) > 0.001f) {
            cr_Running = true;
            m_Parameter = Mathf.Lerp(m_Parameter, camAmp/4, 10*Time.deltaTime);
            applyPosition(m_Parameter);
            yield return null;
        }

        cr_Running = false;
    }

    void applyPosition(float t) {
        float x = (t > camAmp/2 ? camAmp - t : t) - camAmp/4;
        float y = 3.5f*x*x;
        transform.localPosition = new Vector3(x, y, 0) + defaultPosition;
    } 
}
