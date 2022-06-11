using Godot;
using System;

public class Hurtbox : Area2D
{
    public bool IsInvicible = false;
    private Timer _timer {get;set;}
    private CollisionShape2D _collisionShape {get;set;}
    // private PackedScene _hitEffect = (PackedScene)ResourceLoader.Load("");

    [Signal]
    delegate void InvicibilityStarted();
    [Signal]
    delegate void InvicibilityEnded();
    public override void _Ready()
    {
        _timer = GetNode<Timer>("Timer");
        _collisionShape = GetNode<CollisionShape2D>("CollisionShape2D");
    }

    public void SetInviciblity(bool value)
    {
        IsInvicible = value;
        if(IsInvicible)
            EmitSignal("InvicibilityStarted");
        else
            EmitSignal("InvicibilityEnded");
    }
    public void StartInvicibility(int duration)
    {
        IsInvicible = true; 
    }

    public void OnTimerTimeout()
    {
        IsInvicible = false;
    }

    public void OnInvicibilityStarted()
    {
        _collisionShape.SetDeferred("disabled",true);
    }

    public void OnInvicibilityEnded()
    {
        _collisionShape.Disabled = false;
    }

    public void ShowHitEffect()
    {
        // var effect = (HitEffect)_hitEffect.Instance();
    }
}
