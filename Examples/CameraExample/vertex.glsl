#version 430 core				

uniform float time;
uniform mat4 mvp;

in vec3 position;
in vec3 instancePosition;
in vec3 instanceSpeed;

out float depthCue;

void main() 
{
	vec3 pos = position;
	pos += instancePosition + time * instanceSpeed;
	vec4 posCS = mvp * vec4(pos, 1.0);
	depthCue = 1.0 - clamp(posCS.z, 0.0, 1.0);
	gl_Position = posCS;
}