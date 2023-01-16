
using System;

public class Mier : InteractableSprite
{
    public override float TimeToFade => 0.1f;
    public override string AttachedAnimation => "Mier_Incomming";
    public override string AttachedSound => "Mier";
    public override string AttachedTrigger => "Mier_Trigger";
    public override bool UntriggeredInteraction { get; set; }
}
