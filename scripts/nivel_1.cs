using Godot;
using System;

public partial class nivel_1 : Node2D
{
	private Sprite2D escenario;

	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		this.escenario = GetNode<Sprite2D>("Escenario/SpriteEscenario");

		Vector2 screenSize = GetViewport().GetVisibleRect().Size;

		this.escenario.Position = screenSize / 2;
		this.escenario.Centered = true;
	}
}
