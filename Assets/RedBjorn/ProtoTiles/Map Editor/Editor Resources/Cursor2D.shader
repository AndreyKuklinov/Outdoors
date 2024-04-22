Shader "Cursor/2D"
{
    Properties
    {
        [PerRendererData] _MainTex("Sprite Texture", 2D) = "white" {}
        [HideInInspector] _Color("Tint", Color) = (1,1,1,1)
        [MaterialToggle] PixelSnap("Pixel snap", Float) = 0
        [HideInInspector] _RendererColor("RendererColor", Color) = (1,1,1,1)
        [HideInInspector] _Flip("Flip", Vector) = (1,1,1,1)
        [PerRendererData] _AlphaTex("External Alpha", 2D) = "white" {}
        [PerRendererData] _EnableExternalAlpha("Enable External Alpha", Float) = 0
    }

    SubShader
    {
        Tags
        {
            "Queue" = "Transparent"
            "IgnoreProjector" = "True"
            "RenderType" = "Transparent"
            "PreviewType" = "Plane"
        }

        ZWrite Off
        Cull Off
        Lighting Off
        Blend One OneMinusSrcAlpha

        Pass
        {
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #pragma target 2.0
            #pragma multi_compile_instancing
            #pragma multi_compile_local _ PIXELSNAP_ON
            #pragma multi_compile _ ETC1_EXTERNAL_ALPHA
            #include "UnitySprites.cginc"

            struct appdata
            {
                float4 vertex : POSITION;
            };

            struct v2f_loc
            {
                float4 vertex : SV_POSITION;
                fixed4 color : COLOR;
            };

            v2f_loc vert(appdata v)
            {
                v2f_loc o;

                o.vertex = UnityFlipSprite(v.vertex, _Flip);
                o.vertex = UnityObjectToClipPos(o.vertex);
                o.color = _Color;

                #ifdef PIXELSNAP_ON
                o.vertex = UnityPixelSnap(OUT.vertex);
                #endif

                return o;
            }

            fixed4 frag(v2f_loc i) : SV_Target
            {
                fixed4 c = i.color;
                c.rgb *= c.a;
                return c;
            }
            ENDCG
        }
    }
}