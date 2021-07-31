using System.Collections;
using UnityEngine;
using UnityEngine.Events;

public class OnCollisionHit : MonoBehaviour
{
    public AudioClip hitClip;
    public UnityEvent onCollisionHit;
    public float hitAmount { get; set; }
    bool cooled = true;
    public void OnCollision(float force)
    {
        if (!cooled) return;
        hitAmount = force;
        if (hitClip)
        {
            AudioSource.PlayClipAtPoint(hitClip, transform.position, hitAmount * StaticConfig.onCollisionHitVolumeMultiplier);
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
