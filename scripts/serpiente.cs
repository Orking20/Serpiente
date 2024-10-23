using Godot;
using System;
using System.Collections.Generic;

public partial class serpiente : Node2D
{
	private List<Vector2> viboraSegmentos; // Guarda las posiciones
	private List<SerpienteSegmentos> segmentosSprite; // Guarda los sprites vizuales
	private Texture2D texturaSegmentos; // Convierte la textura en el sprite que se va a ver en pantalla
	private Direccion direccionActual;
	private Direccion direccionSiguiente;
	private float tiempoMovimiento; // Cuando esta variable sea más grande que el MoveDelay se mueve la serpiente
	private const int CellSize = 16;
	private const float MoveDelay = 0.1f;
	private Area2D areaColision;
	private Label gameOver;
	private Button btnReiniciar;
	private nivel_1 nivel;
	[Signal]
	public delegate void ComidaRecolectadaEventHandler();

	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		this.viboraSegmentos = new List<Vector2>();
		this.direccionActual = Direccion.Right;
		this.direccionSiguiente = this.direccionActual;
		this.tiempoMovimiento = 0f;
		this.segmentosSprite = new List<SerpienteSegmentos>();
		this.texturaSegmentos = GD.Load<Texture2D>("res://sprites/jugador.png");
		
		this.areaColision = GetNode<Area2D>("Cuerpo");
		this.areaColision.Monitorable = true;
        this.areaColision.Monitoring = true;
		this.gameOver = GetNode<Label>("/root/Nivel1/GameOver");
		this.btnReiniciar = GetNode<Button>("/root/Nivel1/BtnReiniciar");
		this.gameOver.Hide();
		this.btnReiniciar.Hide();

		this.nivel = GetParent<nivel_1>();

		this.btnReiniciar.Pressed += OnBotonReiniciarPressed;

		// Conectar la señal de colisión
        this.areaColision.BodyEntered += OnBodyEntered; // Chequea colisiones de comida y escenario
		this.areaColision.AreaEntered += OnBodyEntered; // Chequea colisiones de su propio cuerpo

		Vector2 screenSize = GetViewport().GetVisibleRect().Size;
		Vector2 startPosition = screenSize / 2;

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
		this.direccionActual = this.direccionSiguiente;

		Vector2 nuevaPosCabeza = this.viboraSegmentos[0] + GetDirectionVector() * CellSize;
		this.viboraSegmentos.Insert(0, nuevaPosCabeza);
		this.viboraSegmentos.RemoveAt(this.viboraSegmentos.Count - 1);

		// Actualizar la posición de todos los sprites
		for (int i = 0; i < this.segmentosSprite.Count; i++)
        {
            this.segmentosSprite[i].Position = this.viboraSegmentos[i];
        }

		// Actualizar la posición del área de colisión
        this.areaColision.Position = new Vector2(this.viboraSegmentos[0].X - 8, this.viboraSegmentos[0].Y - 8);
	}

	private void AgregarSegmento(Vector2 posicion)
    {
        this.viboraSegmentos.Add(posicion);
        
        var segmento = new SerpienteSegmentos(this.texturaSegmentos);
        segmento.Position = posicion;
		segmento.AddToGroup("serpiente");
        AddChild(segmento);
        this.segmentosSprite.Add(segmento);
    }

	private void OnBodyEntered(Node2D body)
    {
		if (body.IsInGroup("comida"))
		{
			Comer();
		}
		else if (body.IsInGroup("Escenario"))
		{
			GameOver();
		}
		else if (body is SerpienteSegmentos && body != segmentosSprite[0])
		{
			GameOver();
		}
    }

	public void GameOver()
    {
		this.gameOver.Show();
		this.btnReiniciar.Show();
		GetTree().Paused = true;
    }

	private void OnColisionConSegmento()
    {
        GameOver();
    }

	private void Comer()
	{
		EmitSignal(SignalName.ComidaRecolectada);

		Vector2 nuevaPosicion = new Vector2(this.Position.X, this.Position.Y);
		CallDeferred(nameof(AgregarSegmento), nuevaPosicion);
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
		if (this.direccionActual == this.direccionSiguiente)
		{
			if (Input.IsActionJustPressed("move_right") && this.direccionActual != Direccion.Left)
			{
				this.direccionSiguiente = Direccion.Right;
			}
			else if (Input.IsActionJustPressed("move_left") && this.direccionActual != Direccion.Right)
			{
				this.direccionSiguiente = Direccion.Left;
			}
			else if (Input.IsActionJustPressed("move_up") && this.direccionActual != Direccion.Down)
			{
				this.direccionSiguiente = Direccion.Up;
			}
			else if (Input.IsActionJustPressed("move_down") && this.direccionActual != Direccion.Up)
			{
				this.direccionSiguiente = Direccion.Down;
			}
		}
	}

	private void OnBotonReiniciarPressed()
	{
		GetTree().Paused = false;
		GetTree().ChangeSceneToFile("res://escenas/menu_principal.tscn");
	}
}

public enum Direccion
{
	Up,
	Down,
	Left,
	Right
}