using Godot;
using System;

public partial class Comida : StaticBody2D
{
	private int posX;
	private int posY;

	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		this.posX = new Random().Next(16, 1280);
		this.posY = new Random().Next(16, 704);

		this.Position = acomodarPosicionCuadricula(this.posX, this.posY);
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
	}

	private Vector2 acomodarPosicionCuadricula(int posX, int posY)
	{
		Vector2 posicionCuadrada = new Vector2();

		// Se acomoda la posición X para que esté centrada en la cuadrícula del mapa
		for (int i = 0; i < 16; i++)
		{
			if (posX % 16 == 0)
			{
				break;
			}

			posX -= 1;
		}

		// Se acomoda la posición Y para que esté centrada en la cuadrícula del mapa
		for (int i = 0; i < 16; i++)
		{
			if (posY % 16 == 0)
			{
				break;
			}

			posY -= 1;
		}

		posicionCuadrada.X = posX;
		posicionCuadrada.Y = posY;
		
		return posicionCuadrada;
	}
}
