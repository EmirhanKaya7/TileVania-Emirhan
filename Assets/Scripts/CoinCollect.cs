using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using DG.Tweening;
using TMPro;
using Unity.UI;

public class CoinCollect : MonoBehaviour
{
    [SerializeField] TMP_Text coinUIText;
    private int _c=0;

    public float speed;
    public int Coins{
        get{return _c;}
        set{
            _c=value;
            coinUIText.text=Coins.ToString();
        }
    }
    [SerializeField] Transform target;
    [SerializeField] Ease easeType;

    // public Transform initial;
    [SerializeField] GameObject CoinPreFab;
    [SerializeField] int MaxCoins;
    Queue<GameObject> coinsQ = new Queue<GameObject>();

    [SerializeField] [Range(0.5f,0.9f)] float minAnim;
    [SerializeField] [Range(0.9f,2f)] float maxAnim;
    Vector3 targetPos;
    void Awake() {
       targetPos = target.position; 

PrepareCoins();
    }

    void PrepareCoins(    ){
            GameObject coin;
        
        for (int i = 0; i < MaxCoins; i++)
        {
        
                coin = Instantiate (CoinPreFab);
                coin.transform.parent = transform;
                coin.SetActive(false);
                coinsQ.Enqueue(coin);   
        }
        

    }
    void Animate(Vector3 collectedLoc,int amount){
        
        for (int i = 0; i < amount; i++)
        {
            if (coinsQ.Count > 0 )
            {
                GameObject coin = coinsQ.Dequeue();
                coin.SetActive(true);
                coin.transform.position = collectedLoc;
                float duration = UnityEngine.Random.Range(minAnim,maxAnim);
                coin.transform.DOMove(coinUIText.transform.position,duration).SetEase(easeType).OnComplete(()=>{
                    coin.SetActive(false);
                    coinsQ.Enqueue(coin);
                    Coins++;
                });
            }


        }
    }
    public void AddCoins(Vector3 collectedLoc,int amount){
        Animate(collectedLoc,amount);
        
    }
    
}