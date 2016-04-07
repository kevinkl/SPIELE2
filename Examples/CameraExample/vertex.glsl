#version 430 core				

uniform float time;
uniform mat4 mvp;

layout(location = 0) in vec3 in_position;
layout(location = 1) in vec3 in_speed;

out float depthCue;

void main() 
{
	vec3 pos = in_position + time * in_speed;
	depthCue = 1.0 - clamp(pos.z, 0.0, 1.0);
	gl_Position = vec4(pos, 1.0);
}