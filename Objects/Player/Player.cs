using Godot;
using System;

public class Player : KinematicBody2D
{
    [Export(PropertyHint.Range, "0,200")]
    private int _moveSpeed = 100;
    [Export(PropertyHint.Range, "0,1000")]
    private int _jumpSpeed = 550;
    [Export(PropertyHint.Range, "1000,3000")]
    private int _gravity = 2000;

    private Vector2 _velocity = Vector2.Zero;

    public override void _PhysicsProcess(float delta)
    {
        _velocity.x = 0; //Reset Horizontal

        if (Input.IsActionPressed("moveRight"))
        {
            if (Input.IsActionPressed("sprint"))
            {
                _velocity.x += _moveSpeed;
            }
            _velocity.x += _moveSpeed;
        }
        
        if (Input.IsActionPressed("moveLeft"))
        {
            if (Input.IsActionPressed("sprint"))
            {
                _velocity.x -= _moveSpeed;
            }
            _velocity.x -= _moveSpeed;
        }
        
        if (Input.IsActionPressed("jump"))
        {
            if (IsOnFloor())
            {
                _velocity.y -= _jumpSpeed;
            }
        }

        _velocity.y += _gravity * delta;

        _velocity = MoveAndSlide(_velocity, Vector2.Up);
    }
    
}
