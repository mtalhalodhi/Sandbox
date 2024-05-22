shader_type canvas_item;

uniform sampler2D palette;
uniform sampler2D SCREEN_TEXTURE: hint_screen_texture, filter_linear_mipmap;

void fragment() {
	//vec4 color = texture(TEXTURE, SCREEN_UV);
	vec3 color = textureLod(SCREEN_TEXTURE, SCREEN_UV, 0.0).rgb;
	
	vec4 closest_color = vec4(0.0);
	float min_distance = 10000.0;
	
	int p_len = textureSize(palette, 0).x;
	
	// Iterate through the palette colors
    for (int i = 0; i < p_len; i++) {
        // Sample the palette color
        vec4 palette_color = texture(palette, vec2(float(i) / float(p_len), 0.5));

        float dist = distance(color.rgb, palette_color.rgb);

        // Update the closest color if the distance is smaller
        if (dist < min_distance) {
            min_distance = dist;
            closest_color = palette_color;
        }
    }

    COLOR = closest_color;
}
