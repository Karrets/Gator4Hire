using Godot;

public class Player : KinematicBody2D
{
	[Export(PropertyHint.Range, "0,200")]
	public int MoveSpeed = 100;
	[Export(PropertyHint.Range, "0,1000")]
	public int JumpSpeed = 550;

	[Inherit]
	public int Gravity;

	private Vector2 _velocity = Vector2.Zero;

	public override void _PhysicsProcess(float delta)
	{
		_velocity.x /= 1.5f; //Reset Horizontal

		if (Input.IsActionPressed("moveRight"))
		{
			if (Input.IsActionPressed("sprint"))
			{
				_velocity.x += MoveSpeed;
			}
			_velocity.x += MoveSpeed;
		}
		
		if (Input.IsActionPressed("moveLeft"))
		{
			if (Input.IsActionPressed("sprint"))
			{
				_velocity.x -= MoveSpeed;
			}
			_velocity.x -= MoveSpeed;
		}
		
		if (Input.IsActionPressed("jump"))
		{
			if (IsOnFloor())
			{
				_velocity.y -= JumpSpeed;
			}
			else {
				if (IsOnWall()) {
					_velocity.y -= JumpSpeed / 2f;
					_velocity.x += JumpSpeed * 2f;
				}
			}

		}

		if (!IsOnFloor()) { //This is used so we don't apply gravity when we are on slopes.
			_velocity.y += Gravity * delta;
		}

		if (IsOnWall()) {
			_velocity.y += (Gravity / 2f) * delta;
		}

		_velocity = MoveAndSlide(_velocity, Vector2.Up);
	}
	
}
