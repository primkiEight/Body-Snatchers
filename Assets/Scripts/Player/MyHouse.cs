using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MyHouse : MonoBehaviour {

    public AudioClip HouseMusic;

    private void OnTriggerEnter(Collider other)
    {
        if(other.tag == "Player")
        {
            other.tag = "PlayerMasked";
            if(HouseMusic)
                GameManager.GM.ChangeMusic(HouseMusic);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.tag == "PlayerMasked")
        {
            other.tag = "Player";
            if (HouseMusic)
                GameManager.GM.ResetMusic();
        }
    }

}
