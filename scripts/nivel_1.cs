using Godot;
using System;

public partial class nivel_1 : Node2D
{
	private Sprite2D escenario;
	private PackedScene escenaComida;
	private PackedScene escenaComidaGrande;
	private PackedScene escenaComidaSuper;
	private Comida comidaActual;
	public Tipo tipoComida; // chica - normal - super
	private int contComidas; // Contador que indica cuando aparece una comida más grande. En 0 aparece todas las veces. En 1 aparece una sí una no. Y así sucesivamente

	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		this.escenario = GetNode<Sprite2D>("Escenario/SpriteEscenario");
		this.escenaComida = ResourceLoader.Load<PackedScene>("res://escenas/comida.tscn");
		this.escenaComidaGrande = ResourceLoader.Load<PackedScene>("res://escenas/comida-grande.tscn");
		this.escenaComidaSuper = ResourceLoader.Load<PackedScene>("res://escenas/comida-super.tscn");
		this.contComidas = 0;
		this.tipoComida = Tipo.Chica;
		var jugador = GetNode<serpiente>("Serpiente");
		jugador.ComidaRecolectada += OnComidaRecolectada;

		Vector2 screenSize = GetViewport().GetVisibleRect().Size;

		this.escenario.Position = screenSize / 2;
		this.escenario.Centered = true;

		SpawnComida();
	}

	private void OnComidaRecolectada()
	{
		if (this.comidaActual != null)
		{
			this.comidaActual.CallDeferred("queue_free");
			this.comidaActual = null;
		}

		CallDeferred(nameof(SpawnComida));
	}

	private void SpawnComida()
	{
		if (this.contComidas == 14)
		{
			this.comidaActual = this.escenaComidaSuper.Instantiate<Comida>(); // Se instancia nueva comida super
			this.tipoComida = Tipo.Super;
			this.contComidas = 0;
		}
		else if (this.contComidas == 4 || this.contComidas == 9)
		{
			this.comidaActual = this.escenaComidaGrande.Instantiate<Comida>(); // Se instancia nueva comida grande
			this.tipoComida = Tipo.Grande;
			this.contComidas++;
		}
		else
		{
			this.comidaActual = this.escenaComida.Instantiate<Comida>(); // Se instancia nueva comida
			this.tipoComida = Tipo.Chica;
			this.contComidas++;
		}

		this.comidaActual.AddToGroup("comida");
		AddChild(this.comidaActual);
	}

	public Comida GetComidaActual()
    {
        return this.comidaActual;
    }

	public enum Tipo
	{
		Chica = 1,
		Grande = 5,
		Super = 30
	}
}
