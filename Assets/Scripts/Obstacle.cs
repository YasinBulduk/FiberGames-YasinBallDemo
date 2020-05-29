using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Obstacle : MonoBehaviour, ICanInteractable
{
    public void Interact(GameObject other)
    {
        BallController ball = other.GetComponent<BallController>();

        if (!ball) return;

        ball.Die();
    }
}