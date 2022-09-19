using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossMoveIndicator : MonoBehaviour
{
    Vector3 offset = new Vector3(0, 0.8f, -0.25f);
    
    public IEnumerator SetSprite(Material sprite, float indicatorTime)
    {
        gameObject.SetActive(true);
        GetComponent<MeshRenderer>().material = sprite;
        yield return new WaitForSeconds(indicatorTime);
        gameObject.SetActive(false);
    }

    void Update()
    {
        transform.rotation = Quaternion.Euler(new Vector3(15, 180, 0));
        transform.position = transform.parent.position + (offset * 2);
    }
}
