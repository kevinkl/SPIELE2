#version 430 core

in float depthCue;

out vec4 color;

void main() 
{
	color = vec4(depthCue, 0.0, 0.0, 1.0);
}