#version 330 core
in vec2 fUv;

uniform sampler2D uTexture0;
uniform vec3 uTint;
uniform float uAlpha;

out vec4 FragColor;

void main()
{
    vec4 pixelColor = texture(uTexture0, fUv) * vec4(uTint, uAlpha);
    FragColor = pixelColor;
}