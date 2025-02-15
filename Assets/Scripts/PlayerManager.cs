using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class PlayerManager : MonoBehaviour
{
    [SerializeField] private float speed;
    public float Speed { get { return speed; } private set { speed = value; } }

    private void Start()
    {
        ChangeSpeed(1);
    }

    public void ChangeSpeed(float value)
    {
        Speed += value;
    }
}
