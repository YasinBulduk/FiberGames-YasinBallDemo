using UnityEngine;

public abstract class AbstractInteractableTriggerable : AbstractTriggerable, ICanInteractable
{
    public abstract void Interact(GameObject other);
}
