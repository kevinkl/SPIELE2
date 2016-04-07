#version 430 core				

uniform float time;

layout(location = 0) in vec2 in_position;
layout(location = 1) in vec2 in_speed;

void main() 
{
	gl_Position = vec4(in_position + time * in_speed, 0.0, 1.0);
}