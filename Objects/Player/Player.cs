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
		_velocity.x /= 1.5f; //Reset Horizontal

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
			else {
				if (IsOnWall()) {
					_velocity.y -= _jumpSpeed / 2f;
					_velocity.x += _jumpSpeed * 2f;
				}
			}

		}

		if (!IsOnFloor()) { //This is used so we don't apply gravity when we are on slopes.
			_velocity.y += _gravity * delta;
		}

		if (IsOnWall()) {
			Console.WriteLine("WALL RIDAAA");
			_velocity.y += (_gravity / 2f) * delta;
		}

		_velocity = MoveAndSlide(_velocity, Vector2.Up);
	}
	
}
