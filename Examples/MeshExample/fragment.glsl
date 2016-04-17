#version 430 core
in vec3 n;

out vec4 color;

void main() 
{
	//use normal as color
	color = vec4(n, 1.0);
}