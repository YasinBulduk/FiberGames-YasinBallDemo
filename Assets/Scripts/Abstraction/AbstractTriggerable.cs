using System;
using System.Collections.Generic;
using UnityEngine;

public abstract class AbstractTriggerable : MonoBehaviour, ICanTriggerable
{
    public abstract void Triggered();
}
