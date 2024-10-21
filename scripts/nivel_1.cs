using Godot;
using System;

public partial class nivel_1 : Node2D
{
	private Sprite2D escenario;
	private PackedScene escenaComida;
	private Comida comidaActual;

	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		this.escenario = GetNode<Sprite2D>("Escenario/SpriteEscenario");
		this.escenaComida = ResourceLoader.Load<PackedScene>("res://escenas/comida.tscn");
		var jugador = GetNode<serpiente>("Serpiente");
		jugador.ComidaRecolectada += OnComidaRecolectada;

		Vector2 screenSize = GetViewport().GetVisibleRect().Size;

		this.escenario.Position = screenSize / 2;
		this.escenario.Centered = true;

		SpawnComida();
	}

	private void OnComidaRecolectada()
	{
		GD.Print(this.comidaActual);
		if (this.comidaActual != null)
		{
			this.comidaActual.CallDeferred("queue_free");
			this.comidaActual = null;
		}
		CallDeferred(nameof(SpawnComida));
	}

	private void SpawnComida()
	{
		this.comidaActual = this.escenaComida.Instantiate<Comida>(); // Se instancia nueva comida
		this.comidaActual.AddToGroup("comida");
		AddChild(this.comidaActual);
	}

	public Comida GetComidaActual()
    {
        return this.comidaActual;
    }
}
