using System;
using System.Collections.Generic;
using UnityEngine;

public abstract class AbstractInteractableTrigger : MonoBehaviour, ICanInteractable, ICanTrigger
{
    public List<AbstractTriggerable> triggerList;

    public abstract void Interact(GameObject other);

    public abstract void Trigger();
}
