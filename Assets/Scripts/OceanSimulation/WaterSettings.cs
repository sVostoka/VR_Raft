using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "WaterSettings", menuName = "Water/WaterSettings", order = 1)]
public class WaterSettings : ScriptableObject
{
    [Header("Vertex Settings")]
    
    public float vertexSeedIter = 1253.2131f;
    public float vertexFrequency = 1.0f;
    public float vertexFrequencyMult = 1.18f;
    public float vertexAmplitude = 1.0f;
    public float vertexAmplitudeMult = 0.82f;
    public float vertexInitialSpeed = 2.0f;
    public float vertexSpeedRamp = 1.07f;
    public float vertexDrag = 1.0f;
    public float vertexHeight = 1.0f;
    public float vertexMaxPeak = 1.0f;
    public float vertexPeakOffset = 1.0f;

    [Header("Fragment Settings")]
    public float fragmentFrequency = 1.0f;
    public float fragmentFrequencyMult = 1.18f;
    public float fragmentAmplitude = 1.0f;
    public float fragmentAmplitudeMult = 0.82f;
    public float fragmentInitialSpeed = 2.0f;
    public float fragmentSpeedRamp = 1.07f;
    public float fragmentDrag = 1.0f;
    //public float fragmentHeight = 1.0f;
    public float fragmentMaxPeak = 1.0f;
    public float fragmentPeakOffset = 1.0f;

    [Header(" ")]
    public float normalStrength = 1;

    [Header("Shader Settings")]
    [ColorUsageAttribute(false, true)]
    public Color ambient;

    [ColorUsageAttribute(false, true)]
    public Color diffuseReflectance;

    [ColorUsageAttribute(false, true)]
    public Color specularReflectance;

    public float shininess;
    public float specularNormalStrength = 1;

    [ColorUsageAttribute(false, true)]
    public Color fresnelColor;

    public bool useTextureForFresnel = false;
    public Texture environmentTexture;

    public float fresnelBias, fresnelStrength, fresnelShininess;
    public float fresnelNormalStrength = 1;

    [ColorUsageAttribute(false, true)]
    public Color tipColor;
    public float tipAttenuation;

}
