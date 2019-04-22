/// @description Insérez la description ici

if (direction > 90 and direction < 270) {
	flipX = -1
} else if (direction != 90 and direction != 270) {
	flipX = 1
}

if (dirX != 0 or dirY != 0) {
	draw_sprite_ext(s_player_run,-1,x,y,flipX,1,0,c_white,1);
} else {
	draw_sprite_ext(s_player_idle,-1,x,y,flipX,1,0,c_white,1);
}
