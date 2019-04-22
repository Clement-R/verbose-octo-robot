/// @description Insérez la description ici

dirX = (keyboard_check(vk_right) - keyboard_check(vk_left));
dirY = (keyboard_check(vk_down) - keyboard_check(vk_up))

if (dirX != 0 or dirY !=0) {
	direction = point_direction(x,y,x+ dirX, y + dirY);	
	
	if (place_free(x+dirX*spd,y)) {
		x += lengthdir_x(spd*(abs(dirX)),direction)
	}
	
	if (place_free(x,y+dirY*spd)) {
		y += lengthdir_y(spd*(abs(dirY)),direction)
	}
}
