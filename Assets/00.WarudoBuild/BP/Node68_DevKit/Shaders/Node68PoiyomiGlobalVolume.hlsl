#ifndef NODE68_POIYOMI_GLOBAL_VOLUME_INCLUDED
#define NODE68_POIYOMI_GLOBAL_VOLUME_INCLUDED

// Warudo Node Poiyomi Volume → SetGlobalFloat (Potatoon Volume과 같은 CPU 부담 수준)
float _Node68GlobalBaseColorDimEnabled;
float _Node68GlobalBaseColorDim;

float _Node68GlobalLightingCapEnabled;
float _Node68GlobalLightingCap;

#define NODE68_APPLY_GLOBAL_BASE_COLOR_DIM(colorVar) \
    (colorVar).rgb *= lerp(1.0, _Node68GlobalBaseColorDim, saturate(_Node68GlobalBaseColorDimEnabled))

#define NODE68_APPLY_GLOBAL_LIGHTING_CAP \
    if (_Node68GlobalLightingCapEnabled > 0.5) \
    { \
        poiLight.directColor = min(poiLight.directColor, _Node68GlobalLightingCap); \
        poiLight.indirectColor = min(poiLight.indirectColor, _Node68GlobalLightingCap); \
    }

#endif
