using Godot;
using System;

public partial class menu_principal : Control
{
	Button btnJugar;
	Button btnSalir;

	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		this.btnJugar = GetNode<Button>("ContenedorBotones/BtnJugar");
		this.btnSalir = GetNode<Button>("ContenedorBotones/BtnSalir");

		this.btnJugar.Pressed += OnBotonJugarPressed;
		this.btnSalir.Pressed += OnBotonSalirPressed;
	}

	private void OnBotonJugarPressed()
	{
		GetTree().ChangeSceneToFile("res://escenas/nivel_1.tscn");
	}

	private void OnBotonSalirPressed()
	{
		GetTree().Quit();
	}
}