using Godot;

public partial class SerpienteSegmentos : Area2D
{
    [Signal]
    public delegate void ColisionConSegmentoEventHandler();

    public SerpienteSegmentos(Texture2D texture)
    {
        this.AddToGroup("serpiente");

        Sprite2D sprite = new Sprite2D();
        sprite.Texture = texture;
        AddChild(sprite);

        CollisionShape2D collisionShape = new CollisionShape2D();
        RectangleShape2D shape = new RectangleShape2D();
        shape.Size = new Vector2(16, 16);
        collisionShape.Shape = shape;
        AddChild(collisionShape);
    }

    private void OnAreaEntered(Area2D area)
    {
        if (area is SerpienteSegmentos && area.IsInGroup("serpiente"))
		{
            EmitSignal(SignalName.ColisionConSegmento);
		}
    }
}