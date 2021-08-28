using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shoot : MonoBehaviour
{
    [SerializeField] GameObject prefab;
    [SerializeField] Transform posicionInicial;
    [SerializeField] bool automatic, canShoot = true;
    [SerializeField] float speed, bulletDuration, bulletFrequency;
    [Range(1,0f)]
    [SerializeField] float initialSpread = .25f;
    [Range(-1,1f)]
    [SerializeField] float spreadPerShot = 0;
    [SerializeField] float maxSpread = 1;
    [SerializeField] float minSpread = 0.05f;
    [SerializeField] float spreadRecoveryDelay = 1;
    [SerializeField] float spreadRecoveryTime = 1;
    float shotsFiredRecently = 0, shootDelay = 0;

    Coroutine spreadRecoveryDelayCoroutine, spreadRecoveryProcessCoroutine;

    [SerializeField] LayerMask hitable;

    public float SPREAD {
        get{
            return Mathf.Clamp(initialSpread + spreadPerShot * shotsFiredRecently,minSpread,maxSpread);
        }
    }

    public void Disparar(){
        //RaycastHit output;
        //Physics.Raycast(Camera.main.ScreenPointToRay(posicionInicial.position), out output,100,hitable,QueryTriggerInteraction.Ignore);

        if(canShoot){
            GameObject bullet = Instantiate(prefab,posicionInicial.position,Quaternion.identity);
            bullet.GetComponent<Rigidbody>().velocity = ShootingSpread() * speed;
            Destroy(bullet,bulletDuration);
            StartCoroutine(Delay());
            if(spreadRecoveryProcessCoroutine != null){
                StopCoroutine(spreadRecoveryProcessCoroutine);
                spreadRecoveryProcessCoroutine = null;
            }
            if(spreadRecoveryDelayCoroutine != null){
                StopCoroutine(spreadRecoveryDelayCoroutine);
                spreadRecoveryDelayCoroutine = null;
            }
            shotsFiredRecently++;
        }
    }

    Vector3 ShootingSpread(){
        Vector3 spreadDisplacement = transform.right * RandomRangeSQRT2O2() + transform.up * RandomRangeSQRT2O2();
        if(spreadDisplacement.magnitude > 1) spreadDisplacement.Normalize();
        return transform.forward + spreadDisplacement * SPREAD;
    }

    float RandomRangeSQRT2O2(){
        return Random.Range(-1f,1);
    }

    private void Update() {
        //print(SPREAD);
        if(automatic && Input.GetMouseButton(0) || !automatic && Input.GetMouseButtonDown(0)){
            Disparar();
        }
        else
        {
            if(spreadRecoveryDelayCoroutine == null) spreadRecoveryDelayCoroutine = StartCoroutine(SpreadRecoveryDelay());
        }
    }

    IEnumerator SpreadRecoveryDelay(){
        yield return new WaitForSeconds(spreadRecoveryDelay);
        spreadRecoveryProcessCoroutine = StartCoroutine(SpreadRecoveryProcess());
        spreadRecoveryDelayCoroutine = null;
    }

    IEnumerator SpreadRecoveryProcess(){
        float shotsAtStart = shotsFiredRecently;
        while(shotsFiredRecently > 0){
            shotsFiredRecently -= shotsAtStart/50;
            print(shotsFiredRecently);
            yield return new WaitForSeconds(spreadRecoveryTime/50);
        }
        shotsFiredRecently = 0;
        spreadRecoveryProcessCoroutine = null;
    }

    IEnumerator Delay(){
        canShoot = false;
        if(bulletFrequency > 0) yield return new WaitForSeconds(1/bulletFrequency);
        canShoot = true;
    }
}
