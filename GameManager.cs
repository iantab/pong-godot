using Godot;

public partial class GameManager : Node2D
{
    [Export] public NodePath BallPath;
    [Export] public NodePath LeftGoalPath;
    [Export] public NodePath RightGoalPath;
    [Export] public NodePath Player1ScorePath;
    [Export] public NodePath Player2ScorePath;

    private Ball ball;
    private Label player1ScoreLabel;
    private Label player2ScoreLabel;

    private int player1Score;
    private int player2Score;

    public override void _Ready()
    {
        ball = GetNode<Ball>(BallPath);
        player1ScoreLabel = GetNode<Label>(Player1ScorePath);
        player2ScoreLabel = GetNode<Label>(Player2ScorePath);

        var leftGoal = GetNode<Area2D>(LeftGoalPath);
        var rightGoal = GetNode<Area2D>(RightGoalPath);
        leftGoal.BodyEntered += OnLeftGoalEntered;
        rightGoal.BodyEntered += OnRightGoalEntered;
    }

    private void OnLeftGoalEntered(Node2D body)
    {
        if (body is not Ball)
        {
            return;
        }

        player2Score++;
        UpdateScoreUI();
        ball.Serve(1);
    }
    
    private void OnRightGoalEntered(Node2D body)
    {
        if (body is not Ball)
        {
            return;
        }

        player1Score++;
        UpdateScoreUI();
        ball.Serve(-1);
    }

    private void UpdateScoreUI()
    {
        player1ScoreLabel.Text = player1Score.ToString();
        player2ScoreLabel.Text = player2Score.ToString();
    }
}