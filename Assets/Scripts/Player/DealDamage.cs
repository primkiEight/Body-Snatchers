using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DealDamage : MonoBehaviour {

    public Vector2 DamageToDealMinAndMax = Vector2.one;
    private float _damageToDeal = 1f;

    public enum HitForce
    {
        None,
        Pierce,
        Knockback,
        PierceAndKnockback,
        Explosion
    }

    public HitForce applyForce = HitForce.None;

    
    [Range(0,3)]
    public int Passible = 0; //0 ne prolazi kroz neprijatelje, >0 toliko puta prolazi kroz neprijatelje
    public Vector2 KnockBackForceMinAndMax = Vector2.one;
    private float _knockBackForce = 0f;
    public Vector2 ExplosionForceMinAndMax = Vector2.one;
    private float _explosionForce = 0f;
    public Vector2 ExplosionRadiusMinAndMax = Vector2.one;
    public float _explosionRadius = 0f;
    public Vector2 WaitToExplodeMinAndMax = Vector2.one;
    private float _waitToExplode = 0f;
    private int _currentPassible = 0;
    
    private void Awake()
    {
        _damageToDeal = Vector2RandomExtension.V2Random(DamageToDealMinAndMax);
        _knockBackForce = Vector2RandomExtension.V2Random(KnockBackForceMinAndMax);
        _explosionForce = Vector2RandomExtension.V2Random(ExplosionForceMinAndMax);
        _explosionRadius = Vector2RandomExtension.V2Random(ExplosionRadiusMinAndMax);
        _waitToExplode = Vector2RandomExtension.V2Random(WaitToExplodeMinAndMax);
    }

    private void OnTriggerEnter(Collider other)
    {
        
            if (other.GetComponentInParent<HealthManager>())
            {

                switch (applyForce)
                {
                    case HitForce.None:
                        _currentPassible = 0;
                        _waitToExplode = 0;
                        Debug.Log("pogodak");
                        other.GetComponentInParent<HealthManager>().ApplyDamage(_damageToDeal);
                        break;

                    case HitForce.Pierce:
                        _currentPassible = Passible;
                        _waitToExplode = 0;
                        Debug.Log("pierce");
                        //playPierceSound
                        Debug.Log("pogodak");
                        other.GetComponentInParent<HealthManager>().ApplyDamage(_damageToDeal);
                        break;

                    case HitForce.Knockback:
                        _currentPassible = 0;
                        _waitToExplode = 0;
                        if (other.GetComponentInParent<Rigidbody>())
                        {
                            Debug.Log("knockback");
                            other.GetComponentInParent<Rigidbody>().AddRelativeForce(Vector3.back * _knockBackForce, ForceMode.Impulse);
                            //playKnockBackSound
                            Debug.Log("pogodak");
                            other.GetComponentInParent<HealthManager>().ApplyDamage(_damageToDeal);
                        }
                        break;

                    case HitForce.PierceAndKnockback:
                        _currentPassible = Passible;
                        _waitToExplode = 0;
                        if (other.GetComponentInParent<Rigidbody>())
                        {
                            Debug.Log("knockback");
                            other.GetComponentInParent<Rigidbody>().AddRelativeForce(Vector3.back * _knockBackForce, ForceMode.Impulse);
                            //playKnockBackSound
                            Debug.Log("pogodak");
                            other.GetComponentInParent<HealthManager>().ApplyDamage(_damageToDeal);
                        }
                        break;

                    //PROUČITI
                    case HitForce.Explosion:
                        _currentPassible = 0;
                        if (other.GetComponentInParent<Rigidbody>())
                        {
                            Debug.Log("pogodak");
                            StartCoroutine(ExplodeCo(other.gameObject));
                        }
                        break;

                    default:
                        break;
                }

                if (_currentPassible > 0)
                {
                    --_currentPassible;
                    _damageToDeal /= 2;
                    _knockBackForce /= 2;
                }
                else if (applyForce != HitForce.Explosion)
                {
                    Destroy(gameObject);
                }
            }
                
    }

    private IEnumerator ExplodeCo(GameObject other)
    {
        yield return new WaitForSeconds(_waitToExplode);
        Debug.Log("explosion");
        other.GetComponentInParent<Rigidbody>().AddExplosionForce(_explosionForce, transform.position, _explosionRadius);
        //playExplosionSound
        other.GetComponentInParent<HealthManager>().ApplyDamage(_damageToDeal);
        Destroy(gameObject);
    }
}
