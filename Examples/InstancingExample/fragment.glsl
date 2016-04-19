#version 430 core

in vec3 n;

out vec4 color;

void main() 
{
	color = vec4(vec3(n), 1.0);
}