using UnityEngine;
using UnityEngine.Events;

public class OnCollisionHit : MonoBehaviour
{
    public AudioClip hitClip;
    public UnityEvent onCollisionHit;
    public float hitAmount;

    public void OnCollision(float force)
    {
        hitAmount = force;
        if (hitClip)
        {
            AudioSource.PlayClipAtPoint(hitClip, transform.position, hitAmount * StaticConfig.onCollisionHitVolumeMultiplier);
        }
        onCollisionHit?.Invoke();
    }
}
