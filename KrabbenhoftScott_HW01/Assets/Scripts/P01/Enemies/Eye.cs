using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Eye : MonoBehaviour
{
    [Header("Eye Settings")]
    [SerializeField] Color _normalColor;
    [SerializeField] Color _normalEmissionColor;
    [SerializeField] Color _alertColor;
    [SerializeField] Color _alertEmissionColor;
    
    GameObject pupil;
    Vector3 lookTarget;
    float _blinkPeriod = 1f;
    float _minimumBlinkPeriod = 2f;
    float counter = 0f;
    float lastBlink = 0f;

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
            lookTarget = new Vector3(Random.Range(-3, 4) / 10f, Random.Range(-3, 4) / 10f, pupil.transform.localPosition.z);
            if (lastBlink >= _minimumBlinkPeriod && Random.Range(0, 2) == 0)
            {
                StartCoroutine(Blink());
            }
        }

        pupil.transform.localPosition = Vector3.Lerp(pupil.transform.localPosition, lookTarget, 0.1f);
    }

    public void SetColorNormal()
    {
        pupil.transform.GetChild(0).GetComponent<MeshRenderer>().material.SetColor("_Color", _normalColor);
        pupil.transform.GetChild(0).GetComponent<MeshRenderer>().material.SetColor("_EmissionColor", _normalEmissionColor);

        pupil.transform.GetChild(1).GetComponent<MeshRenderer>().material.SetColor("_Color", _normalColor);
        pupil.transform.GetChild(1).GetComponent<MeshRenderer>().material.SetColor("_EmissionColor", _normalEmissionColor);
    }

    public void SetColorAlert()
    {
        pupil.transform.GetChild(0).GetComponent<MeshRenderer>().material.SetColor("_Color", _alertColor);
        pupil.transform.GetChild(0).GetComponent<MeshRenderer>().material.SetColor("_EmissionColor", _alertEmissionColor);

        pupil.transform.GetChild(1).GetComponent<MeshRenderer>().material.SetColor("_Color", _alertColor);
        pupil.transform.GetChild(1).GetComponent<MeshRenderer>().material.SetColor("_EmissionColor", _alertEmissionColor);
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
