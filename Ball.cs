using Godot;

public partial class Ball : Node2D
{
	[Export] public float MinStartSpeedX = 140f;
	[Export] public float MaxStartSpeedX = 200f;
	[Export] public float MaxStartSpeedY = 50f;
	[Export] public float SpeedUpFactor = 1.03f;
	[Export] public float Radius = 2f;
	[Export] public Color Color = Colors.White;

	[Export] public NodePath LeftPaddlePath;
	[Export] public NodePath RightPaddlePath;
	
	private Vector2 _velocity = Vector2.Zero;
	private Vector2 _startPosition;
	private Paddle _leftPaddle;
	private Paddle _rightPaddle;
	public override void _Ready()
	{
		_startPosition = Position;
		_leftPaddle = GetNode<Paddle>(LeftPaddlePath);
		_rightPaddle = GetNode<Paddle>(RightPaddlePath);
		Serve(GD.Randf() < 0.5f ? -1 : 1);
	}
	public override void _Draw()
	{
		DrawCircle(Vector2.Zero, Radius, Color, antialiased: true);
	}
	public override void _PhysicsProcess(double delta)
	{
		Position += _velocity * (float)delta;
		HandleCollisions();
	}

	private static Rect2 RectOf(Node2D node, Vector2 size)
	{
		return new Rect2(node.Position - size / 2f, size);
	}
	
	private void HandleCollisions()
	{
		Vector2 ballSize = new Vector2(Radius * 2, Radius * 2);
		Rect2 ballRect = RectOf(this, ballSize);
		Rect2 leftRect = RectOf(_leftPaddle, _leftPaddle.Size);
		Rect2 rightRect = RectOf(_rightPaddle, _rightPaddle.Size);

		if (ballRect.Intersects(leftRect))
		{
			_velocity.X = -_velocity.X * SpeedUpFactor;
			Position = new Vector2(leftRect.End.X + ballSize.X / 2f, Position.Y);
			_velocity.Y = (float)GD.RandRange(-MaxStartSpeedY, MaxStartSpeedY);
		}
		else if (ballRect.Intersects(rightRect))
		{
			_velocity.X = -_velocity.X * SpeedUpFactor;
			Position = new Vector2(rightRect.Position.X - ballSize.X / 2f, Position.Y);
			_velocity.Y = (float)GD.RandRange(-MaxStartSpeedY, MaxStartSpeedY);
		}

		float viewportHeight = GetViewportRect().Size.Y;
		float halfH = ballSize.Y / 2f;
		if (Position.Y <= halfH)
		{
			Position = new Vector2(Position.X, halfH);
			_velocity.Y = -_velocity.Y;
		}
		else if (Position.Y >= viewportHeight - halfH)
		{
			Position = new Vector2(Position.X, viewportHeight - halfH);
			_velocity.Y = -_velocity.Y;
		}
	}
	
	private void Serve(int direction)
	{
		Position = _startPosition;
		float vx = (float)GD.RandRange(MinStartSpeedX, MaxStartSpeedX) * direction;
		float vy = (float)GD.RandRange(-MaxStartSpeedY, MaxStartSpeedY);
		_velocity = new Vector2(vx, vy);
	}
	
	private void Reset()
	{
		Position = _startPosition;
		_velocity = Vector2.Zero;
	}
}
