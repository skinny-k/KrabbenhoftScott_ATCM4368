using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Eye : MonoBehaviour
{
    GameObject pupil;
    float _blinkPeriod = 1f;
    float _minimumBlinkPeriod = 2f;
    float counter = 0f;
    float lastBlink = 0f;
    Vector3 lookTarget;

    void Awake()
    {
        pupil = transform.GetChild(0).gameObject;
        lookTarget = transform.localPosition;
    }

    void FixedUpdate()
    {
        counter += Time.deltaTime;
        lastBlink += Time.deltaTime;

        if (counter >= _blinkPeriod)
        {
            counter = 0f;
            lookTarget = new Vector3(Random.Range(-4, 5) / 10f, Random.Range(-4, 5) / 10f, pupil.transform.localPosition.z);
            if (lastBlink >= _minimumBlinkPeriod && Random.Range(0, 2) == 0)
            {
                StartCoroutine(Blink());
            }
        }

        pupil.transform.localPosition = Vector3.Lerp(pupil.transform.localPosition, lookTarget, 0.1f);
    }
    
    IEnumerator Blink()
    {
        pupil.SetActive(false);
        transform.localScale = new Vector3(0.5f, 0.1f, 0.5f);

        yield return new WaitForSeconds(0.25f);

        transform.localScale = new Vector3(0.5f, 0.5f, 0.5f);
        pupil.SetActive(true);
        lastBlink = 0f;
    }
}
