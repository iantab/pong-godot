using Godot;

public partial class Ball : Node2D
{
	[Export] public float MinStartSpeedX = 140f;
	[Export] public float MaxStartSpeedX = 200f;
	[Export] public float MaxStartSpeedY = 50f;
	[Export] public Vector2 Size = new Vector2(4, 4);
	[Export] public Color Color = Colors.White;
	
	private Vector2 _velocity = Vector2.Zero;
	private Vector2 _startPosition;
	public override void _Ready()
	{
		_startPosition = Position;
		Serve(GD.Randf() < 0.5f ? -1 : 1);
	}
	public override void _Draw()
	{
		Rect2 rect = new Rect2(-Size / 2f, Size);
		DrawRect(rect, Color);
	}
	public override void _PhysicsProcess(double delta)
	{
		Position += _velocity * (float)delta;
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
