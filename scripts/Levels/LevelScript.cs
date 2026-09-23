using Godot;
using Godot.Collections;
using System;

public partial class LevelScript : Node2D
{
	[Export]
    public PackedScene EnemyScene;

    private Array<Node> enemySpawnPoints;
    private Timer spawnTimer;
	private Random random = new();

	public override void _Ready()
	{
		enemySpawnPoints = GetTree().GetNodesInGroup("enemy spawn point");
		spawnTimer = new Timer();
		spawnTimer.WaitTime = GD.RandRange(5f, 10f);
		AddChild(spawnTimer);
		spawnTimer.Start();
		spawnTimer.Timeout += _on_spawnTimer_timeout;
	}

    private void _on_spawnTimer_timeout()
    {
		var spawnPoint = (Node2D)enemySpawnPoints[random.Next(enemySpawnPoints.Count)];
		var enemy = (Node2D)EnemyScene.Instantiate();
		enemy.GlobalPosition = spawnPoint.GlobalPosition;
		AddChild(enemy);
		spawnTimer.WaitTime = GD.RandRange(1f, 5f);
		spawnTimer.Start();
    }

}
