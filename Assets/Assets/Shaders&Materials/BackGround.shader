Shader "Custom/BackGround"
{
    Properties
    {
        _width ("Border Width", Range(0.0, 0.1)) = 0.1
        _color ("Color", Color) = (0, 0, 0, 1)
        _MainTex ("MainTexture", 2D) = "white"{}
    }
    SubShader
    {
        //Tags { "RenderType"="Transparent" "Queue"="Transparent" }
        Tags { "RenderType"="Opaque" }
        Pass
        {
            //Cull Off
            //ZWrite Off
            //Blend One One

            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #include "UnityCG.cginc"
            float _width;
            float4 _color;
            struct meshData
            {
                float4 vertex : POSITION;
                float4 normal : NORMAL;
                float2 uv : TEXCOORD0;
            };

            struct v2f
            {
                float4 vertex : SV_POSITION;
                float4 normal : NORMAL;
                float2 uv : TEXCOORD0;
                float2 lcoor : TEXCOORD1;
            };

            v2f vert (meshData v)
            {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.uv = v.uv;
                o.normal = v.normal;
                o.lcoor = float2(o.uv.x, o.uv.y);
                return o;
            }

            fixed4 frag (v2f i) : SV_Target
            {
                //return fixed4(0,i.uv, 0);
                clip((i.lcoor.x > _width/2 && i.lcoor.x < (1.0f/3 - _width/2) && i.lcoor.y > _width/2 && i.lcoor.y < (1.0f/3 - _width/2)) * -1);
                clip((i.lcoor.x > _width/2 && i.lcoor.x < (1.0f/3 - _width/2) && i.lcoor.y > (1.0f/3 + _width/2) && i.lcoor.y < (2.0f/3 - _width/2)) * -1);
                clip((i.lcoor.x > _width/2 && i.lcoor.x < (1.0f/3 - _width/2) && i.lcoor.y > (2.0f/3 + _width/2) && i.lcoor.y < (1.0f - _width/2)) * -1);
                
                clip((i.lcoor.x > (1.0f/3 + _width/2) && i.lcoor.x < (2.0f/3 - _width/2) && i.lcoor.y > _width/2 && i.lcoor.y < (1.0f/3 - _width/2)) * -1);
                clip((i.lcoor.x > (1.0f/3 + _width/2) && i.lcoor.x < (2.0f/3 - _width/2) && i.lcoor.y > (1.0f/3 + _width/2) && i.lcoor.y < (2.0f/3 - _width/2)) * -1);
                clip((i.lcoor.x > (1.0f/3 + _width/2) && i.lcoor.x < (2.0f/3 - _width/2) && i.lcoor.y > (2.0f/3 + _width/2) && i.lcoor.y < (1.0f - _width/2)) * -1);
                     
                clip((i.lcoor.x > (2.0f/3 + _width/2) && i.lcoor.x < (1.0f - _width/2) && i.lcoor.y > _width/2 && i.lcoor.y < (1.0f/3 - _width/2)) * -1);
                clip((i.lcoor.x > (2.0f/3 + _width/2) && i.lcoor.x < (1.0f - _width/2) && i.lcoor.y > (1.0f/3 + _width/2) && i.lcoor.y < (2.0f/3 - _width/2)) * -1);
                clip((i.lcoor.x > (2.0f/3 + _width/2) && i.lcoor.x < (1.0f - _width/2) && i.lcoor.y > (2.0f/3 + _width/2) && i.lcoor.y < (1.0f - _width/2)) * -1);
                return _color;
            }
            ENDCG
        }
    }
}
