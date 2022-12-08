using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class CoinPickup : MonoBehaviour
{
    [SerializeField] AudioClip coinpicksfx;
    [SerializeField] int pointsforcoin = 10;

    bool wasCollected = false;
    

     void OnTriggerEnter2D(Collider2D other) {
        if (other.tag == "Player" && !wasCollected)
        {
            wasCollected = true;
            //this.gameObject.transform.DOMove(coinLoc,1f);
            //FindObjectOfType<GameSession>().AddScore(pointsforcoin);
            AudioSource.PlayClipAtPoint(coinpicksfx, Camera.main.transform.position);
            //Destroy(this.gameObject);
        }
    }
    
}
