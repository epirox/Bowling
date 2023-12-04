using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bolo : MonoBehaviour
{
    public delegate void DelScoreHandler(int score);
    public static event DelScoreHandler OnAddPoints;
    [SerializeField] private int point;
    [SerializeField] private GameObject puff;

    private void OnCollisionEnter(Collision collision)
    {
        this.Score(collision);
    }
    private void Score(Collision collision)
    {
        if (collision.gameObject.GetComponent<Bocha>() != null)
        {
            Debug.Log($"Boomm! {point}");            
            OnAddPoints?.Invoke(point);
            Instantiate(puff,transform.position,Quaternion.identity);
            Destroy(this.gameObject);
        }
    }
}
