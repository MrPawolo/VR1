using System.Collections;
using UnityEngine;
using UnityEngine.Events;

public class OnCollisionHit : MonoBehaviour
{
    public GameObject[] objectsToSpawn;
    public AudioClip hitClip;
    public UnityEvent onCollisionHit;
    public float hitAmount { get; set; }
    bool cooled = true;
    public void OnCollision(float force, Collision collision)
    {
        if (!cooled) return;
        hitAmount = force;
        if (hitClip)
        {
            AudioSource.PlayClipAtPoint(hitClip, transform.position, hitAmount * StaticConfig.onCollisionHitVolumeMultiplier);
        }
        if(objectsToSpawn.Length > 0 && force > StaticConfig.impactForce)
        {
             
            foreach(GameObject obj in objectsToSpawn)
            {

                GameObject go = Instantiate(obj, collision.GetContact(0).point, Quaternion.identity);
                Destroy(go, 1f);
            }
        }
        onCollisionHit?.Invoke();
        StartCoroutine(CoolDown());
    }
    IEnumerator CoolDown()
    {
        cooled = false;
        yield return new WaitForSeconds(StaticConfig.hitCoolDown);
        cooled = true;
    }
}
