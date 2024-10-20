using Godot;
using System;

public partial class serpiente : Node2D
{
	private Vector2 direccion;
	private int cellSize = 16;

	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		Vector2 screenSize = GetViewport().GetVisibleRect().Size;
		this.Position = screenSize / 2;

		this.direccion = Vector2.Right;
		this.Mover();
	}

    public override void _Process(double delta)
    {
        if (Input.IsActionJustPressed("ui_right") && direccion != Vector2.Left)
		{
			direccion = Vector2.Right;
		}
		else if (Input.IsActionJustPressed("ui_left") && direccion != Vector2.Right)
		{
			direccion = Vector2.Left;
		}
		else if (Input.IsActionJustPressed("ui_up") && direccion != Vector2.Down)
		{
			direccion = Vector2.Up;
		}
		else if (Input.IsActionJustPressed("ui_down") && direccion != Vector2.Up)
		{
			direccion = Vector2.Down;
		}
    }

	private void Mover()
	{
		Vector2 nuevaPos = this.Position + direccion * cellSize;
		this.Position = nuevaPos;
	}
}
