﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class RestartLevel : MonoBehaviour {

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Projectile")
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
}
