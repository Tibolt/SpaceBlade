using Godot;
using System;

public class ParallexBG : CanvasLayer
{
	private TextureRect _background {get; set;}
	[Export]
	private float changeRate = 2;
	private float hueChange {get; set;}
	private float satChange {get; set;}
	public override void _Ready()
	{
		_background = GetNode<TextureRect>("background");
		hueChange= changeRate;
		satChange= changeRate * 5;
	}
	public override void _Process(float delta)
	{
		var hue = _background.Modulate.h ;
		var saturation = _background.Modulate.s;
		var value = _background.Modulate.v;
		if(GlobalVariables.Score >= hueChange)
		{
			if(hueChange >= satChange)
			{
				saturation += (float)0.05;
				satChange += 10;
			}
			var color = Color.FromHsv(hue + (float)0.03, saturation, value);
			_background.SetModulate(color);
			hueChange += changeRate;
		}
	}

}
