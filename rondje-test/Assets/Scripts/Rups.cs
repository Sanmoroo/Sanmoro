
using System;

public class Rups : InteractableSprite
{
    public override float TimeToFade => 1f;
    public override string AttachedAnimation => "Rups_Incomming";
    public override string AttachedSound => "Rups";
    public override string AttachedTrigger => "Rups_Trigger";
    public override bool UntriggeredInteraction { get; set; }
}
