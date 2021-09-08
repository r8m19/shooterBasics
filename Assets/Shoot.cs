using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shoot : MonoBehaviour
{
    [SerializeField] GameObject prefab;
    [SerializeField] Animator anim;
    [SerializeField] Transform posicionInicial;
    [SerializeField] bool automatic;
    
    [Range(1,16f)]
    [SerializeField] int bulletsPerShot = 1;
    [Range(1-0.000001f,0.000001f)]
    [SerializeField] float initialSpread = .25f;
    [Range(-1,1f)]
    [SerializeField] float spreadPerShot = 0;

    [SerializeField] float speed, bulletDuration, bulletFrequency, maxSpread = 1, minSpread = 0.05f, spreadRecoveryDelay = 1, spreadRecoveryTime = 1;

    [SerializeField] AudioSource audioSource;
    float shotsFiredRecently = 0, shootDelay = 0, canShoot, delayTimer;


    Coroutine spreadRecoveryProcessCoroutine;

    [SerializeField] LayerMask hitable;

    public float SPREAD {
        get{
            return Mathf.Clamp(initialSpread + spreadPerShot * shotsFiredRecently,minSpread,maxSpread);
        }
    }

    void Start() {
        canShoot = Time.time;
        delayTimer = Time.time;
    }

    public void Disparar(){
        //RaycastHit output;
        //Physics.Raycast(Camera.main.ScreenPointToRay(posicionInicial.position), out output,100,hitable,QueryTriggerInteraction.Ignore);
        canShoot = Time.time + 1/bulletFrequency;
        for(int i = 0; i < bulletsPerShot; i++){
            GameObject bullet = Instantiate(prefab,posicionInicial.position,Quaternion.identity);
            bullet.GetComponent<Rigidbody>().velocity = ShootingSpread() * speed;
            Destroy(bullet,bulletDuration);
        }
        
        delayTimer = Time.time + spreadRecoveryDelay;
        if(spreadRecoveryProcessCoroutine != null){
            StopCoroutine(spreadRecoveryProcessCoroutine);
            spreadRecoveryProcessCoroutine = null;
        }

        if(SPREAD > minSpread && SPREAD < maxSpread)
            shotsFiredRecently++;
        anim.SetTrigger("Shoot");
        audioSource.Play();
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
        if((automatic && Input.GetMouseButton(0) || !automatic && Input.GetMouseButtonDown(0)) && Time.time >= canShoot)
            Disparar();
        
        if(Time.time >= delayTimer)
            spreadRecoveryProcessCoroutine = StartCoroutine(SpreadRecoveryProcess());
    }

    /*IEnumerator SpreadRecoveryDelay(){
        yield return new WaitForSeconds(spreadRecoveryDelay);
        spreadRecoveryProcessCoroutine = StartCoroutine(SpreadRecoveryProcess());
        spreadRecoveryDelayCoroutine = null;
    }*/

    IEnumerator SpreadRecoveryProcess(){
        float shotsAtStart = shotsFiredRecently;
        while(shotsFiredRecently > 0){
            shotsFiredRecently -= shotsAtStart/100;
            yield return new WaitForSeconds(spreadRecoveryTime/100);
        }
        shotsFiredRecently = 0;
        spreadRecoveryProcessCoroutine = null;
    }
}
