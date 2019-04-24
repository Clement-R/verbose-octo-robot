/// @description Insérez la description ici

//draw_sprite_ext(s_sword_def,-1,x,y,flipX,1,direction,c_white,1);

draw_sprite_ext(h_heavy,-1,x,y-yoffset,flipX,1,direction,c_white,1);
draw_sprite_ext(g_heavy,-1,x+(flipX*(sprite_get_width(h_heavy)/2)),y-yoffset,flipX,1,direction,c_white,1);
draw_sprite_ext(b_heavy,-1,x+(flipX*(sprite_get_width(g_heavy))),y-yoffset,flipX,1,direction,c_white,1);
