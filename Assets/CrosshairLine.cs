using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CrosshairLine : MonoBehaviour
{
    [HideInInspector] public float maxSize, minSize; 
    [HideInInspector] public Vector3 direccion; 
    RectTransform rt;
    Vector3 startingPos;
    // Start is called before the first frame update
    void Start()
    {
        rt = GetComponent<RectTransform>();
        direccion = rt.localPosition.normalized;
        startingPos = rt.position;
        Vector2 size = rt.sizeDelta;
        if(size.x > size.y){
            maxSize = size.x;
            minSize = size.y;
        }
        else
        {
            maxSize = size.y;
            minSize = size.x;
        }
    }

    public void UpdatePosition(float a){
        rt.position = startingPos + direccion * (a * 150 + 6);
    }
}
