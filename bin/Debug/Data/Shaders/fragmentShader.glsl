#version 400 core

in vec2 pass_textureCoords;
in vec3 surfaceNormal;
in vec3 toLightVector;

out vec4 out_Color;

uniform sampler2D textureSampler;
uniform vec3 lightColor;
uniform float ambienceIntencivity;
uniform float time;

void main (void)
{
	vec4 textureColor = texture(textureSampler, pass_textureCoords);
	
	float nDotl = dot (surfaceNormal, toLightVector);
	float brightness = max(nDotl, ambienceIntencivity);
	
	out_Color = textureColor;
}



