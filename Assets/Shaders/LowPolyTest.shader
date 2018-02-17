// Shader created with Shader Forge v1.38 
// Shader Forge (c) Neat Corporation / Joachim Holmer - http://www.acegikmo.com/shaderforge/
// Note: Manually altering this data may prevent you from opening it in Shader Forge
/*SF_DATA;ver:1.38;sub:START;pass:START;ps:flbk:,iptp:0,cusa:False,bamd:0,cgin:,lico:1,lgpr:1,limd:0,spmd:1,trmd:0,grmd:0,uamb:True,mssp:True,bkdf:False,hqlp:False,rprd:False,enco:False,rmgx:True,imps:True,rpth:0,vtps:0,hqsc:True,nrmq:1,nrsp:0,vomd:0,spxs:False,tesm:0,olmd:1,culm:0,bsrc:0,bdst:1,dpts:2,wrdp:True,dith:0,atcv:False,rfrpo:True,rfrpn:Refraction,coma:15,ufog:False,aust:True,igpj:False,qofs:0,qpre:1,rntp:1,fgom:False,fgoc:False,fgod:False,fgor:False,fgmd:0,fgcr:0.5,fgcg:0.5,fgcb:0.5,fgca:1,fgde:0.01,fgrn:0,fgrf:300,stcl:False,atwp:False,stva:128,stmr:255,stmw:255,stcp:6,stps:0,stfa:0,stfz:0,ofsf:0,ofsu:0,f2p0:False,fnsp:False,fnfb:False,fsmp:False;n:type:ShaderForge.SFN_Final,id:3138,x:32968,y:32511,varname:node_3138,prsc:2|emission-7878-RGB;n:type:ShaderForge.SFN_NormalVector,id:9426,x:31273,y:32662,prsc:2,pt:False;n:type:ShaderForge.SFN_LightVector,id:3033,x:31240,y:32477,varname:node_3033,prsc:2;n:type:ShaderForge.SFN_Dot,id:6098,x:31546,y:32846,varname:node_6098,prsc:2,dt:1|A-9426-OUT,B-2832-OUT;n:type:ShaderForge.SFN_Power,id:7727,x:31776,y:32846,varname:node_7727,prsc:2|VAL-6098-OUT,EXP-8334-OUT;n:type:ShaderForge.SFN_Slider,id:6298,x:31228,y:33018,ptovrint:False,ptlb:Gloss,ptin:_Gloss,varname:node_6298,prsc:2,glob:False,taghide:False,taghdr:False,tagprd:False,tagnsco:False,tagnrm:False,min:0,cur:1,max:1;n:type:ShaderForge.SFN_ConstantLerp,id:8334,x:31546,y:33018,varname:node_8334,prsc:2,a:1,b:11|IN-6298-OUT;n:type:ShaderForge.SFN_HalfVector,id:2832,x:31270,y:32859,varname:node_2832,prsc:2;n:type:ShaderForge.SFN_Dot,id:5546,x:31546,y:32641,varname:node_5546,prsc:2,dt:1|A-3033-OUT,B-9426-OUT;n:type:ShaderForge.SFN_Color,id:3258,x:32156,y:32114,ptovrint:False,ptlb:Color,ptin:_Color,varname:node_3258,prsc:2,glob:False,taghide:False,taghdr:False,tagprd:False,tagnsco:False,tagnrm:False,c1:0.5,c2:0.5,c3:0.5,c4:1;n:type:ShaderForge.SFN_Add,id:9872,x:32008,y:32641,varname:node_9872,prsc:2|A-5385-OUT,B-6218-OUT;n:type:ShaderForge.SFN_ConstantLerp,id:6218,x:32008,y:32846,varname:node_6218,prsc:2,a:0,b:0.25|IN-7727-OUT;n:type:ShaderForge.SFN_ConstantLerp,id:5385,x:31776,y:32641,varname:node_5385,prsc:2,a:0,b:0.25|IN-5546-OUT;n:type:ShaderForge.SFN_Dot,id:7755,x:31546,y:32473,varname:node_7755,prsc:2,dt:2|A-3033-OUT,B-9426-OUT;n:type:ShaderForge.SFN_Abs,id:1016,x:31776,y:32473,varname:node_1016,prsc:2|IN-7755-OUT;n:type:ShaderForge.SFN_ConstantLerp,id:5836,x:32008,y:32473,varname:node_5836,prsc:2,a:0.5,b:0|IN-1016-OUT;n:type:ShaderForge.SFN_Add,id:6300,x:32250,y:32556,varname:node_6300,prsc:2|A-5836-OUT,B-9872-OUT;n:type:ShaderForge.SFN_Tex2d,id:7878,x:32718,y:32601,ptovrint:False,ptlb:Ramp,ptin:_Ramp,varname:node_7878,prsc:2,glob:False,taghide:False,taghdr:False,tagprd:False,tagnsco:False,tagnrm:False,tex:288193ddf34e0ee44a9d932561b55a18,ntxv:0,isnm:False|UVIN-120-OUT;n:type:ShaderForge.SFN_Append,id:120,x:32489,y:32601,varname:node_120,prsc:2|A-6300-OUT,B-1780-OUT;n:type:ShaderForge.SFN_Vector1,id:1780,x:32250,y:32694,varname:node_1780,prsc:2,v1:0;proporder:6298-3258-7878;pass:END;sub:END;*/

Shader "Shader Forge/LowPolyTest" {
    Properties {
        _Gloss ("Gloss", Range(0, 1)) = 1
        _Color ("Color", Color) = (0.5,0.5,0.5,1)
        _Ramp ("Ramp", 2D) = "white" {}
    }
    SubShader {
        Tags {
            "RenderType"="Opaque"
        }
        Pass {
            Name "FORWARD"
            Tags {
                "LightMode"="ForwardBase"
            }
            
            
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #define UNITY_PASS_FORWARDBASE
            #include "UnityCG.cginc"
            #include "AutoLight.cginc"
            #pragma multi_compile_fwdbase_fullshadows
            #pragma only_renderers d3d9 d3d11 glcore gles 
            #pragma target 3.0
            uniform float _Gloss;
            uniform sampler2D _Ramp; uniform float4 _Ramp_ST;
            struct VertexInput {
                float4 vertex : POSITION;
                float3 normal : NORMAL;
            };
            struct VertexOutput {
                float4 pos : SV_POSITION;
                float4 posWorld : TEXCOORD0;
                float3 normalDir : TEXCOORD1;
                LIGHTING_COORDS(2,3)
            };
            VertexOutput vert (VertexInput v) {
                VertexOutput o = (VertexOutput)0;
                o.normalDir = UnityObjectToWorldNormal(v.normal);
                o.posWorld = mul(unity_ObjectToWorld, v.vertex);
                o.pos = UnityObjectToClipPos( v.vertex );
                TRANSFER_VERTEX_TO_FRAGMENT(o)
                return o;
            }
            float4 frag(VertexOutput i) : COLOR {
                i.normalDir = normalize(i.normalDir);
                float3 viewDirection = normalize(_WorldSpaceCameraPos.xyz - i.posWorld.xyz);
                float3 normalDirection = i.normalDir;
                float3 lightDirection = normalize(_WorldSpaceLightPos0.xyz);
                float3 halfDirection = normalize(viewDirection+lightDirection);
////// Lighting:
////// Emissive:
                float2 node_120 = float2((lerp(0.5,0,abs(min(0,dot(lightDirection,i.normalDir))))+(lerp(0,0.25,max(0,dot(lightDirection,i.normalDir)))+lerp(0,0.25,pow(max(0,dot(i.normalDir,halfDirection)),lerp(1,11,_Gloss))))),0.0);
                float4 _Ramp_var = tex2D(_Ramp,TRANSFORM_TEX(node_120, _Ramp));
                float3 emissive = _Ramp_var.rgb;
                float3 finalColor = emissive;
                return fixed4(finalColor,1);
            }
            ENDCG
        }
        Pass {
            Name "FORWARD_DELTA"
            Tags {
                "LightMode"="ForwardAdd"
            }
            Blend One One
            
            
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #define UNITY_PASS_FORWARDADD
            #include "UnityCG.cginc"
            #include "AutoLight.cginc"
            #pragma multi_compile_fwdadd_fullshadows
            #pragma only_renderers d3d9 d3d11 glcore gles 
            #pragma target 3.0
            uniform float _Gloss;
            uniform sampler2D _Ramp; uniform float4 _Ramp_ST;
            struct VertexInput {
                float4 vertex : POSITION;
                float3 normal : NORMAL;
            };
            struct VertexOutput {
                float4 pos : SV_POSITION;
                float4 posWorld : TEXCOORD0;
                float3 normalDir : TEXCOORD1;
                LIGHTING_COORDS(2,3)
            };
            VertexOutput vert (VertexInput v) {
                VertexOutput o = (VertexOutput)0;
                o.normalDir = UnityObjectToWorldNormal(v.normal);
                o.posWorld = mul(unity_ObjectToWorld, v.vertex);
                o.pos = UnityObjectToClipPos( v.vertex );
                TRANSFER_VERTEX_TO_FRAGMENT(o)
                return o;
            }
            float4 frag(VertexOutput i) : COLOR {
                i.normalDir = normalize(i.normalDir);
                float3 viewDirection = normalize(_WorldSpaceCameraPos.xyz - i.posWorld.xyz);
                float3 normalDirection = i.normalDir;
                float3 lightDirection = normalize(lerp(_WorldSpaceLightPos0.xyz, _WorldSpaceLightPos0.xyz - i.posWorld.xyz,_WorldSpaceLightPos0.w));
                float3 halfDirection = normalize(viewDirection+lightDirection);
////// Lighting:
                float3 finalColor = 0;
                return fixed4(finalColor * 1,0);
            }
            ENDCG
        }
    }
    FallBack "Diffuse"
    CustomEditor "ShaderForgeMaterialInspector"
}
