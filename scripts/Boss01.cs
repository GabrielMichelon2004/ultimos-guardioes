using Godot;
using System;

public partial class Boss01 : CharacterBody2D
{
    [Export] public float Speed = 200f;
    [Export] public float Gravity = 900f;
    [Export] public float DirectionChangeInterval = 2f;

    private Sprite2D sprite;
    private float direction = -1f;
    private float timeSinceDirectionChange = 0f;

    public override void _Ready()
    {
        sprite = GetNode<Sprite2D>("sprite");
        sprite.Scale = new Vector2(-1, 1);
        sprite.FlipH = direction > 0;
    }

    public override void _PhysicsProcess(double delta)
    {
        Vector2 velocity = Velocity;
        velocity.Y += Gravity * (float)delta;
        velocity.X = direction * Speed;

        timeSinceDirectionChange += (float)delta;
        if (timeSinceDirectionChange >= DirectionChangeInterval)
        {
            direction *= -1;
            sprite.FlipH = direction > 0;
            timeSinceDirectionChange = 0f;
        }

        Velocity = velocity;
        MoveAndSlide();
    }
}
