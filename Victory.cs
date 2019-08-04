using Godot;
using System;

public class Victory : Control
{
	public void _on_Button_button_up()
	{
		GetTree().ChangeScene("res://Menu.tscn");
	}
}
