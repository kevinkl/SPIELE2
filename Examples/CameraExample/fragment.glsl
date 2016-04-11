#version 430 core
uniform vec3 light;

in float depthCue;
in vec3 n;

out vec4 color;

void main() 
{
	float diffuse = max(0.1, dot(light, n));
	color = vec4(vec3(diffuse), 1.0);
}