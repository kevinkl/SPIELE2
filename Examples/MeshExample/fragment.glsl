#version 430 core

in vec3 n;
in vec3 lightDir;

out vec4 color;

void main() 
{
	vec3 l = normalize(lightDir);
	float diffuse = max(0.07, dot(l, n));
	color = vec4(vec3(diffuse), 1.0);
}