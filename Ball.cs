using Godot;

public partial class Ball : CharacterBody2D
{
	[Export] public float MinStartSpeedX = 140f;
	[Export] public float MaxStartSpeedX = 200f;
	[Export] public float MaxStartSpeedY = 50f;
	[Export] public float SpeedUpFactor = 1.03f;
	[Export] public float Radius = 2f;
	[Export] public Color Color = Colors.White;

	private Vector2 _startPosition;

	public override void _Ready()
	{
		_startPosition = Position;
		Serve(GD.Randf() < 0.5f ? -1 : 1);
	}

	public override void _Draw()
	{
		DrawCircle(Vector2.Zero, Radius, Color, antialiased: true);
	}

	public override void _PhysicsProcess(double delta)
	{
		Vector2 motion = Velocity * (float)delta;
		KinematicCollision2D collision = MoveAndCollide(motion);
		if (collision != null)
		{
			HandleCollision(collision);
		}
	}

	private void HandleCollision(KinematicCollision2D collision)
	{
		GodotObject other = collision.GetCollider();
		if (other is Paddle)
		{
			Velocity = new Vector2(
				-Velocity.X * SpeedUpFactor, (float)GD.RandRange(-MaxStartSpeedY, MaxStartSpeedY));
		}
		else
		{
			Velocity = Velocity.Bounce(collision.GetNormal());
		}
	}

	public void Serve(int direction)
	{
		Position = _startPosition;
		float vx = (float)GD.RandRange(MinStartSpeedX, MaxStartSpeedX) * direction;
		float vy = (float)GD.RandRange(-MaxStartSpeedY, MaxStartSpeedY);
		Velocity = new Vector2(vx, vy);
	}

	public void Reset()
	{
		Position = _startPosition;
		Velocity = Vector2.Zero;
	}
}
