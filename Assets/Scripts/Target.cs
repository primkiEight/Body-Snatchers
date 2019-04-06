using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Target : MonoBehaviour {

    public int ScoreValue = 5;
    public float TimeValue = 5.0f;

    public ParticleSystem ExplosionPrefab;

    private void OnTriggerEnter(Collider other)
    {
        if (GameManager.GM.CurrentState == GameManager.GameState.GameOver)
            return;

        if (other.tag == "Projectile")
        {
            GameManager.GM.AddScore(ScoreValue);
            GameManager.GM.AddTime(TimeValue);

            if (ExplosionPrefab)
                Instantiate(ExplosionPrefab, transform.position, other.transform.rotation);

            Destroy(other.gameObject); //uništavamo projektil
            Destroy(gameObject); //uništavamo pogođeni target
        }
    }
}
