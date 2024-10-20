using Godot;
using System;
using System.Collections.Generic;

public partial class Serpiente : Node2D
{
	private List<Vector2> viboraSegmentos;
	private List<SerpienteSegmentos> segmentosSprite;
	private Texture2D texturaSegmentos;
	private Direccion direccionActual;
	private float tiempoMovimiento;
	private const int CellSize = 16;
	private const float MoveDelay = 0.2f;

	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		this.viboraSegmentos = new List<Vector2>();
		this.direccionActual = Direccion.Right;
		this.tiempoMovimiento = 0f;
		this.segmentosSprite = new List<SerpienteSegmentos>();
		this.texturaSegmentos = GD.Load<Texture2D>("res://sprites/jugador.png");

		Vector2 screenSize = GetViewport().GetVisibleRect().Size;
		Vector2 startPosition = screenSize / 2;

		//this.viboraSegmentos.Add(new Vector2(this.Position.X, this.Position.Y));
		for (int i = 0; i < 3; i++)
		{
			Vector2 posicionInicial = new Vector2(startPosition.X - (i * CellSize), startPosition.Y);
			AgregarSegmento(posicionInicial);
		}
	}

    public override void _Process(double delta)
    {
        HandleInput();

		this.tiempoMovimiento += (float)delta;
		if (this.tiempoMovimiento >= MoveDelay)
		{
			MoveSnake();
			this.tiempoMovimiento = 0;
		}
    }

	private void MoveSnake()
	{
		Vector2 nuevaPosCabeza = this.viboraSegmentos[0] + GetDirectionVector() * CellSize;
		this.viboraSegmentos.Insert(0, nuevaPosCabeza);
		this.viboraSegmentos.RemoveAt(this.viboraSegmentos.Count - 1);

		// Actualizar la posición de todos los sprites
		for (int i = 0; i < this.segmentosSprite.Count; i++)
        {
            this.segmentosSprite[i].Position = this.viboraSegmentos[i];
        }
	}

	private void AgregarSegmento(Vector2 posicion)
    {
        this.viboraSegmentos.Add(posicion);
        
        var segmento = new SerpienteSegmentos(this.texturaSegmentos);
        segmento.Position = posicion;
        AddChild(segmento);
        this.segmentosSprite.Add(segmento);
    }

	private Vector2 GetDirectionVector()
	{
		switch (this.direccionActual)
		{
			case Direccion.Right:
			{
				return new Vector2(1, 0);
			}
			case Direccion.Left:
			{
				return new Vector2(-1, 0);
			}
			case Direccion.Up:
			{
				return new Vector2(0, -1);
			}
			case Direccion.Down:
			{
				return new Vector2(0, 1);
			}
			default:
			{
				return Vector2.Zero;
			}
		}
	}

	private void HandleInput()
	{
		if (Input.IsActionJustPressed("move_right") && this.direccionActual != Direccion.Left)
		{
			this.direccionActual = Direccion.Right;
		}
		else if (Input.IsActionJustPressed("move_left") && this.direccionActual != Direccion.Right)
		{
			this.direccionActual = Direccion.Left;
		}
		else if (Input.IsActionJustPressed("move_up") && this.direccionActual != Direccion.Down)
		{
			this.direccionActual = Direccion.Up;
		}
		else if (Input.IsActionJustPressed("move_down") && this.direccionActual != Direccion.Up)
		{
			this.direccionActual = Direccion.Down;
		}
	}
}

public enum Direccion
{
	Up,
	Down,
	Left,
	Right
}