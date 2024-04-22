Shader "Cursor/3D"
{
    Properties
    {
        _Color("Main Color", Color) = (1,1,1,1)
    }

    SubShader
    {
        Tags 
        { 
            "Queue" = "Overlay+50"
            "RenderType" = "Transparent" 
        }

        ZTest Off
        ZWrite Off
        Blend One OneMinusSrcAlpha
        LOD 100

        Pass
        {
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #include "UnityCG.cginc"

            struct appdata
            {
                float4 vertex : POSITION;
            };

            struct v2f
            {
                float4 vertex : SV_POSITION;
                float4 color : COLOR;
            };

            fixed4 _Color;

            v2f vert(appdata v)
            {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.color = _Color;
                return o;
            }

            fixed4 frag(v2f i) : SV_Target 
            { 
                fixed4 c = i.color;
                c.rgb *= c.a;
                return c;
            }
            ENDCG
        }
    }
}