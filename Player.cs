using Godot;
using System;

public class Player : KinematicBody2D
{

	[Export]
	float jumpHeight, riseTime, fallTime, maxSpeed, accTime, stopTime;
	float gRise, gFall, jumpSpeed, accel, decel;

	Vector2 speed;

	int hp = 3;


	public override void _Ready()
	{
		// calculate jump parameters
		jumpSpeed = -jumpHeight * 2 / riseTime;
		gRise = (jumpHeight * 2) / (riseTime * riseTime);
		gFall = (jumpHeight * 2) / (fallTime * fallTime);
		// calculate motion parameters
		accel = maxSpeed / accTime;
		decel = maxSpeed / stopTime;

		GD.Print(accel.ToString() + " " + decel.ToString() + " " + jumpSpeed.ToString());
	}
	
	Vector2 gunDir = new Vector2(1,0);

	public override void _Process(float delta)
	{
		DoMovment(delta);
		DoGravity(delta);
		MoveAndSlide(speed,new Vector2(0,-1));

		if (Input.IsActionPressed("ui_right") || Input.IsActionPressed("ui_left")
		|| Input.IsActionPressed("ui_up") || Input.IsActionPressed("ui_down")) {
			gunDir.x = Input.GetActionStrength("ui_right") 
				- Input.GetActionStrength("ui_left");
			gunDir.y = Input.GetActionStrength("ui_down") 
				- Input.GetActionStrength("ui_up");
		} else if(gunDir.x == 0) {
			gunDir.x = 1;
			gunDir.y = 0;
		}
		
		if (Input.IsActionPressed("fire"))
			Shoot(gunDir);
		DoAnimation();

		if (hp <= 0)
			GetTree().ReloadCurrentScene();
	}

	void DoAnimation() {
		Vector2 dir;
		dir.x = Math.Abs(gunDir.x);
		dir.y = gunDir.y;

		bool fire = Input.IsActionPressed("fire");

		AnimatedSprite lance = GetNode<AnimatedSprite>("Lance");

		if (dir.x == 1 && dir.y == 0 && fire) {
			lance.SetAnimation("horizontal");
		} else if (dir.x == 1 && dir.y == -1 && fire) {
			lance.SetAnimation("d_up");
		} else if (dir.x == 0 && dir.y == -1 && fire) {
			lance.SetAnimation("up");
		} else if (dir.x == 1 && dir.y == 1 && fire) {
			lance.SetAnimation("d_down");
		} else if (dir.x == 0 && dir.y == 1 && fire) {
			lance.SetAnimation("down");
		} else if (Math.Abs(speed.x) > 0) {
			lance.SetAnimation("walk");
		} else {
			lance.SetAnimation("stop");
		}

		if (Math.Abs(speed.x) > 0) {
			GetNode<AnimatedSprite>("Sprite").SetAnimation("walk");
		} else {
			GetNode<AnimatedSprite>("Sprite").SetAnimation("default");
		}

		GetNode<AnimatedSprite>("Sprite").FlipH = gunDir.x < 0;
		lance.FlipH = gunDir.x < 0;
	}

	void DoMovment(float delta)
	{
		float movInput = Input.GetActionStrength("ui_right") 
				- Input.GetActionStrength("ui_left");

		if (Mathf.Abs(speed.x) > 50 && movInput == 0)
			speed.x -= Math.Sign(speed.x) * decel * delta;
		else if (movInput == 0)
			speed.x = 0;
		
		if (movInput != 0)
			speed.x += accel * movInput * delta;
		
		speed.x = Mathf.Clamp(speed.x, -maxSpeed, maxSpeed);
	}

	void DoGravity(float delta)
	{
		if(!IsOnFloor()) {
			if(speed.y < 0)
				speed.y += gRise * delta;
			else
				speed.y += gFall * delta;
		} else {
			if(Input.IsActionJustPressed("jump"))
				speed.y = jumpSpeed;
			
			if(speed.y > 5)
				speed.y = 5;
		}
	}

	void Shoot(Vector2 dir)
	{
		if(GetNode<Timer>("GunTimer").IsStopped()) {
			if(dir == Vector2.Zero) 
				dir = new Vector2(1,0);
			
			var projectile = GD.Load<PackedScene>("res://PlayerProjectile.tscn");
			PlayerProjectile projectileInst = projectile.Instance() as PlayerProjectile;
			
			(projectileInst as KinematicBody2D).Position = this.Position;
			projectileInst.speed = dir * 800 + new Vector2(speed.x,0);
			projectileInst.LookAt(dir.Normalized() * 100 + Position);
			
			GetParent().AddChild(projectileInst as KinematicBody2D);
			GetNode<Timer>("GunTimer").Start();
		}
	}

	void knockback(Vector2 enemyPos)
	{
		speed = new Vector2(Math.Sign(Position.x - enemyPos.x) * 500, -200);
	}

	public void _on_Area2D_body_entered(PhysicsBody2D boddy)
	{
		if ((boddy is JumpySlime 
		  || boddy is RedSlime 
		  || boddy is PurpleSlime
		  || boddy is PurpleBall
		  || boddy is PurpleProjectile) 
		  && GetNode<Timer>("DamageTimer").IsStopped()) {
			hp--;
			knockback(boddy.Position);
			GD.Print(hp);
			GetNode<Timer>("DamageTimer").Start();
			GetNode<AnimatedSprite>("Sprite").SetAnimation("damage");
			GetNode<ProgressBar>("../CanvasLayer/PlayerHP").Value = hp;
			GetNode<Camera>("../CameraHolder").Shake(10);
		}
	}
	
	public void _on_DamageTimer_timeout()
	{
		GetNode<AnimatedSprite>("Sprite").SetAnimation("default");
	}
}
