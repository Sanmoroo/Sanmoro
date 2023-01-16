using System;

public class Fly : InteractableSprite
{
    public override float TimeToFade => 0.1f;
    public override string AttachedAnimation => "Fly_Incomming";
    public override string AttachedSound => "Fly";
    public override string AttachedTrigger => "Fly_Trigger";
    public override bool UntriggeredInteraction { get; set; }
}
