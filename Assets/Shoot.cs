using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shoot : MonoBehaviour
{
    [SerializeField] GameObject prefab;
    [SerializeField] Transform posicionInicial;
    [SerializeField] bool automatic, canShoot = true;
    [SerializeField] float speed, bulletDuration, bulletFrequency;
    [Range(0,1f)]
    [SerializeField] float bulletSpread = 1;
    [SerializeField] LayerMask hitable;
    public void Disparar(){
        //RaycastHit output;
        //Physics.Raycast(Camera.main.ScreenPointToRay(posicionInicial.position), out output,100,hitable,QueryTriggerInteraction.Ignore);

        if(canShoot){
            GameObject bullet = Instantiate(prefab,posicionInicial.position,Quaternion.identity);
            bullet.GetComponent<Rigidbody>().velocity = ShootingSpread() * speed;
            Destroy(bullet,bulletDuration);
            StartCoroutine(Delay());
        }
    }

    Vector3 ShootingSpread(){
        Vector3 spread = transform.right * RandomRangeSQRT2O2() + transform.up * RandomRangeSQRT2O2();
        if(spread.magnitude > 1) spread.Normalize();
        return transform.forward + spread * (1 - bulletSpread);
    }

    float RandomRangeSQRT2O2(){
        return Random.Range(-1f,1);
    }

    private void Update() {
        if(automatic && Input.GetMouseButton(0) || !automatic && Input.GetMouseButtonDown(0)){
            Disparar();
        }
    }

    IEnumerator Delay(){
        canShoot = false;
        if(bulletFrequency > 0) yield return new WaitForSeconds(1/bulletFrequency);
        canShoot = true;
    }
}
