using System;
using Godot;

public class Player : KinematicBody2D {
	[Export(PropertyHint.Range, "0,300")]
	public int MoveSpeed = 100;
	[Export(PropertyHint.Range, "0,300")]
	public int JumpSpeed = 175;
	[Export(PropertyHint.Range, "1,3")]
	public float SprintModifier = 1.5f;
	[Export(PropertyHint.Range, "0,1")]
	public float DuckModifier = 0.5f;
	[Export(PropertyHint.Range, "0,5")]
	public float Drag = 1.5f;
	[Export(PropertyHint.Range, "0,0.25")]
	public float CoyoteTime = 0.05f;
	[Export(PropertyHint.Range, "0,25")]
	public float AerialPenalty = 7.5f;

	[Inherit]
	public int Gravity;
	[Inherit]
	public int TerminalVelocity;

	private float _lastGrounded = 0;

	private Vector2 _velocity = Vector2.Zero;

	public override void _PhysicsProcess(float delta) {
		_lastGrounded += delta;
		var sprinting = false;
		var ducking = false;
		if (IsOnFloor()) _lastGrounded = 0;

		if (_lastGrounded <= CoyoteTime) { //Only apply drag when "grounded"
			_velocity /= Drag;
		}

		//moveRight, moveLeft, jump, sprint and duck are the available inputs
		if (Input.IsActionPressed("sprint")) {
			sprinting = true;
		}
		if (Input.IsActionPressed("duck")) {
			ducking = true;
		}
		
		if (Input.IsActionPressed("moveRight")) {
			var moveApplied = 0f;
			if (sprinting) moveApplied += MoveSpeed * SprintModifier;
			else if (ducking) moveApplied += MoveSpeed * DuckModifier;
			else moveApplied += MoveSpeed;

			if (_lastGrounded > CoyoteTime) _velocity.x += moveApplied / AerialPenalty;
			else _velocity.x += moveApplied; //When in the air, your move speed is reduced...
		}
		if (Input.IsActionPressed("moveLeft")) {
			var moveApplied = 0f;
			if (sprinting) moveApplied -= MoveSpeed * SprintModifier;
			else if (ducking) moveApplied -= MoveSpeed * DuckModifier;
			else moveApplied -= MoveSpeed;

			if (_lastGrounded > CoyoteTime) _velocity.x += moveApplied / AerialPenalty;
			else _velocity.x += moveApplied;
		}
		if (Input.IsActionPressed("jump")) {
			if (_lastGrounded <= CoyoteTime) {
				_velocity.y -= JumpSpeed;
				_velocity.y -= Math.Abs(_velocity.x) / 10;
			}
			else if (IsOnWall()) {
				for (var i = 0; i < GetSlideCount(); i++) {
					var collision = GetSlideCollision(i);
					if (collision.Normal.x > 0)
						_velocity += new Vector2(JumpSpeed, -JumpSpeed / 1.25f);
					if (collision.Normal.x < 0)
						_velocity += new Vector2(-JumpSpeed, -JumpSpeed / 1.25f);
				}
			}
		}

		if (_lastGrounded > CoyoteTime) {
			_velocity.y += Gravity * delta;
			if (_velocity.y > 0) _velocity.y *= 1.1f;
		}

		if (_velocity.y > TerminalVelocity) _velocity.y = TerminalVelocity; //Cap speed...
		if (_velocity.x < -TerminalVelocity) _velocity.x = -TerminalVelocity;
		if (_velocity.y > TerminalVelocity) _velocity.y = TerminalVelocity;
		if (_velocity.x < -TerminalVelocity) _velocity.x = -TerminalVelocity;

		_velocity = MoveAndSlide(_velocity, Vector2.Up);
		//Apply our movement vector. Secondary vector indicates up / reverse gravity

		if (IsOnFloor()) _velocity += GetFloorVelocity();
		//Add the velocity of the floor we are currently standing on...
	}
}
