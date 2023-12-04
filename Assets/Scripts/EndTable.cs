using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EndTable : MonoBehaviour
{
    public delegate void DelEndTableHandler();
    public static event DelEndTableHandler NextRound;

    private void OnTriggerEnter(Collider other)
    {
        this.Score(other);
    }
    private void Score(Collider other)
    {
        if (other.gameObject.GetComponent<Bocha>() != null)
        {
            NextRound?.Invoke();
        }
    }
}
