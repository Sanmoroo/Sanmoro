using System;

public class Flow : InteractableSprite
{
    public override float TimeToFade => 1f;
    public override string AttachedAnimation => "Flow_Incomming";
    public override string AttachedSound => "Flow";
    public override string AttachedTrigger => "Flow_Trigger";
    public override bool UntriggeredInteraction { get; set; }
}
