using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PainterParticleController : MonoBehaviour
{
    public ParticleSystem drop;
    public ParticleSystem poolDrop;

    public void SetColor(Color color)
    {
        ParticleSystem.MainModule dropMain = drop.main;
        ParticleSystem.MainModule poolMain = poolDrop.main;
        dropMain.startColor = color;
        poolMain.startColor = color;
    }
}
