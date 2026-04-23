using Godot;

public partial class GameManager : Node2D
{
    public enum GameState { Start, Serve, Play, Done }
    public GameState State { get; private set; } = GameState.Start;
    
    [Export] public NodePath BallPath;
    [Export] public NodePath LeftGoalPath;
    [Export] public NodePath RightGoalPath;
    [Export] public NodePath Player1ScorePath;
    [Export] public NodePath Player2ScorePath;
    [Export] public NodePath MessageTextPath;

    private Ball ball;
    private Label player1ScoreLabel;
    private Label player2ScoreLabel;

    private int player1Score;
    private int player2Score;
    private Label messageLabel;
    private int servingPlayer = 1;

    public override void _Ready()
    {
        ball = GetNode<Ball>(BallPath);
        player1ScoreLabel = GetNode<Label>(Player1ScorePath);
        player2ScoreLabel = GetNode<Label>(Player2ScorePath);
        messageLabel = GetNode<Label>(MessageTextPath);

        var leftGoal = GetNode<Area2D>(LeftGoalPath);
        var rightGoal = GetNode<Area2D>(RightGoalPath);
        leftGoal.BodyEntered += OnLeftGoalEntered;
        rightGoal.BodyEntered += OnRightGoalEntered;
        
        ball.Reset();
        messageLabel.Text = "Welcome to Pong!\nPress Enter to begin";
    }

    public override void _PhysicsProcess(double delta)
    {
        if (!Input.IsActionJustPressed("ui_accept")) return;

        switch (State)
        {
            case GameState.Start:
                EnterServeState();
                break;
            case GameState.Serve:
                State = GameState.Play;
                messageLabel.Text = "";
                ball.Serve(servingPlayer == 1 ? 1 : -1);
                break;
        }
    }

    private void OnLeftGoalEntered(Node2D body)
    {
        if (body is not Ball || State != GameState.Play)
            return;

        player2Score++;
        UpdateScoreUI();
        servingPlayer = 1;
        EnterServeState();
    }
    
    private void OnRightGoalEntered(Node2D body)
    {
        if (body is not Ball || State != GameState.Play)
        {
            return;
        }

        player1Score++;
        UpdateScoreUI();
        servingPlayer = 2;
        EnterServeState();
    }

    private void EnterServeState()
    {
        ball.Reset();
        State = GameState.Serve;
        messageLabel.Text = $"Player {servingPlayer}'s serve!\nPress Enter";
    }

    private void UpdateScoreUI()
    {
        player1ScoreLabel.Text = player1Score.ToString();
        player2ScoreLabel.Text = player2Score.ToString();
    }
}