#version 330 core
out vec4 FragColor;

in vec2 TexCoords;

uniform sampler2D hdrBuffer;
uniform sampler2D bloomBlur;
uniform sampler2D modelMask; // texture containing the mask of the models
uniform bool hdr;
uniform bool bloom;
uniform float exposure;

void main()
{
    const float gamma = 2.2;
    vec3 hdrColor = texture(hdrBuffer, TexCoords).rgb;
    vec3 bloomColor = texture(bloomBlur, TexCoords).rgb;
    vec3 modelMaskColor = texture(modelMask, TexCoords).rgb;

    if (bloom) {
        hdrColor += bloomColor;
    }

    vec3 result = vec3(0.0);
    if (hdr)
    {
        result = vec3(1.0) - exp(-hdrColor * exposure);
        result = pow(result, vec3(1.0 / gamma));
    }

    // Apply HDR effect only to the models
    result *= modelMaskColor;
    result += (1.0 - modelMaskColor) * pow(hdrColor, vec3(1.0 / gamma));

    FragColor = vec4(result, 1.0);
}