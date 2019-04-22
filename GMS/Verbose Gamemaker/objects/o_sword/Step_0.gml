/// @description Insérez la description ici

if (object_exists(user)) {
	x = user.x;
	y = user.y-1;
	flipX = user.flipX;
}

if (keyboard_check_pressed(vk_space) && !isAttacking) {
	swipe = TweenEasyTurn(direction, -120*flipX, 0, 5,EaseInOutCirc, TWEEN_MODE_BOUNCE);
	isAttacking = true
	//TweenPlay(swipeAnim);
}

if (isAttacking) {
	if (!TweenExists(swipe)) {
		isAttacking = false;
	}
} else {
	direction = baseDirection * flipX
}

//direction -= .5

//
