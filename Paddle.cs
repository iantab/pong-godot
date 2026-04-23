using Godot;

public partial class Paddle : Node2D
{
	[Export] public Key UpKey = Key.W;
	[Export] public Key DownKey = Key.S;
	[Export] public float Speed = 200f;
	[Export] public Vector2 Size = new Vector2(5, 20);
	[Export] public Color Color = Colors.White;
	
	public override void _Draw()
	{
		// Draw rectangle centered on this node's position
		Rect2 rect = new Rect2(-Size / 2f, Size);
		DrawRect(rect, Color);
	}
	
	public override void _PhysicsProcess(double delta)
	{
		float direction = 0f;
		if (Input.IsKeyPressed(UpKey)) direction = -1f;
		else if (Input.IsKeyPressed(DownKey)) direction = 1f;
		Vector2 pos = Position;
		pos.Y += direction * Speed * (float)delta;
		
		float viewportHeight = GetViewportRect().Size.Y;
		float halfH = Size.Y / 2f;
		pos.Y = Mathf.Clamp(pos.Y, halfH, viewportHeight - halfH);
		
		Position = pos;
	}
}
