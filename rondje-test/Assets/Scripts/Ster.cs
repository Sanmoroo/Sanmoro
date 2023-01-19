
using System;

public class Ster : InteractableSprite
{
    public override float TimeToFade => 1f;
    public override string AttachedAnimation => "Ster_Incomming";
    public override string AttachedSound => "Ster";
    public override string AttachedTrigger => "Ster_Trigger";
    public override bool UntriggeredInteraction { get; set; }
}
