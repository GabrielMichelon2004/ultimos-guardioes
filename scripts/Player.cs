using Godot;
using System;

public partial class Player : CharacterBody2D
{
    [Export] public float Speed = 200f;
    [Export] public float Gravity = 900f;
    [Export] public float JumpForce = 400f;

    private AnimatedSprite2D animation;
    private string lastDirection = "right";
    private bool isJumping = false;

    public override void _Ready()
    {
        animation = GetNode<AnimatedSprite2D>("Animated");
        animation.AnimationFinished += OnAnimationFinished;
    }
    
    public override void _PhysicsProcess(double delta)
    {
        Vector2 velocity = Velocity;
        float inputDirection = 0f;
        
        // Direção do movimento
        if (Input.IsActionPressed("right"))
        {
            inputDirection += 1f;
            lastDirection = "right";
        }
        else if (Input.IsActionPressed("left"))
        {
            inputDirection -= 1f;
            lastDirection = "left";
        }
        
        velocity.X = inputDirection * Speed;

        // Pulo
        if (IsOnFloor() && Input.IsActionJustPressed("jump"))
        {
            velocity.Y = -JumpForce;
            isJumping = true;
            animation.Play("jump"); // Toca apenas uma vez
        }

        // Gravidade
        velocity.Y += Gravity * (float)delta;

        Velocity = velocity;
        MoveAndSlide();

        // Flip de direção
        animation.FlipH = (lastDirection == "left");

        // Controle de animação (apenas se não estiver pulando)
        if (!isJumping)
        {
            if (IsOnFloor())
            {
                if (inputDirection != 0)
                    animation.Play("run");
                else
                    animation.Play("idle");
            }
        }
    }

    private void OnAnimationFinished()
    {
        // Quando a animação jump terminar
        if (animation.Animation == "jump")
        {
            isJumping = false;
        }
    }
}
