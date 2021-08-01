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
            //AudioSource.PlayClipAtPoint(hitClip, transform.position, hitAmount * StaticConfig.onCollisionHitVolumeMultiplier);
            PlayClipAt(hitClip, transform.position, hitAmount * StaticConfig.onCollisionHitVolumeMultiplier);
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
    void PlayClipAt(AudioClip clip, Vector3 pos, float vol)
    {
        GameObject go = new GameObject("TempAudio");
        go.transform.position = pos;
        AudioSource source = go.AddComponent<AudioSource>();
        source.clip = clip;
        source.volume = vol;
        source.spatialBlend = 1;
        source.pitch = Random.Range(0.97f, 1.03f);
        source.Play();
        Destroy(go, clip.length);
    }
}
