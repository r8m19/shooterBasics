using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Crosshair : MonoBehaviour
{
    [SerializeField] Shoot shoot;
    [SerializeField] CrosshairLine[] cross;
    void Update() {
        foreach (CrosshairLine item in cross)
        {
            item.UpdatePosition(shoot.SPREAD);
        }
    }
}
