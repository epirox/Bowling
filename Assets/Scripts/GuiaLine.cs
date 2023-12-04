using System;
using UnityEngine;
using UnityEngine.UI;

public class GuiaLine : MonoBehaviour
{
    public GameObject bola;
    public GameObject imagenLineaGuia;
    private Vector3 movement;
    [SerializeField] private float speedRotation;
    [SerializeField] private Vector2 maxAngle;

    private void Update()
    {
        Rotate();
    }

    private void Rotate()
    {
        movement = Vector3.zero;
        movement.y = Input.GetAxis("Horizontal");
        Vector3 deltaFrameMove = movement.normalized * speedRotation * Time.deltaTime;
        float nextRotation = transform.rotation.y + deltaFrameMove.y;
        if (IsLimitRotation(nextRotation))
        {
            transform.Rotate(movement);
        }

    }
    
    private bool IsLimitRotation(float nextPosition)
    {
        float aux = nextPosition * 180f / (float)Math.PI;
        Debug.Log($"Giro en {aux}º; Min: {maxAngle.x}; Max: {maxAngle.y};");
        if (
            aux > maxAngle.x
         && aux < maxAngle.y
        )
        {
            return true;
        }
        else
        {
            return false;
        }
    }
    public float GetDirection()
    {
        return transform.rotation.y;
    }
}
