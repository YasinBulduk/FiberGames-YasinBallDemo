using UnityEngine;

public class FinishHole : AbstractInteractableTrigger
{
    public override void Interact(GameObject other)
    {
        BallController ball = other.GetComponent<BallController>();

        if (!ball) return;

        Trigger();
    }

    public override void Trigger()
    {
        triggerList.ForEach(t => t.Triggered());
    }
}
