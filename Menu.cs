using Godot;
using System;

public class Menu : Control
{
	public void _on_Button_button_up()
	{
		GetTree().ChangeScene("res://Main.tscn");
	}

	public void _on_one_button_up()
	{
		GetTree().ChangeScene("res://One.tscn");
	}
}
