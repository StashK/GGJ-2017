Shader "AlpacaSound/RetroPixel9" 
{
	Properties
	{
		_Color0 ("Color 0", Color) = (0.00, 0.00, 0.00, 1.0)	
		_Color1 ("Color 1", Color) = (0.14, 0.14, 0.14, 1.0)	
		_Color2 ("Color 2", Color) = (0.29, 0.29, 0.29, 1.0)	
		_Color3 ("Color 3", Color) = (0.43, 0.43, 0.43, 1.0)	
		_Color4 ("Color 4", Color) = (0.57, 0.57, 0.57, 1.0)	
		_Color5 ("Color 5", Color) = (0.71, 0.71, 0.71, 1.0)	
		_Color6 ("Color 6", Color) = (0.86, 0.86, 0.86, 1.0)	
		_Color7 ("Color 7", Color) = (1.00, 1.00, 1.00, 1.0)
		_Color0("Color 8", Color) = (0.00, 0.00, 0.00, 1.0)
		_Color1("Color 9", Color) = (0.14, 0.14, 0.14, 1.0)
		_Color2("Color 10", Color) = (0.29, 0.29, 0.29, 1.0)
		_Color3("Color 11", Color) = (0.43, 0.43, 0.43, 1.0)
		_Color4("Color 12", Color) = (0.57, 0.57, 0.57, 1.0)
		_Color5("Color 13", Color) = (0.71, 0.71, 0.71, 1.0)
		_Color6("Color 14", Color) = (0.86, 0.86, 0.86, 1.0)
		_Color7("Color 15", Color) = (1.00, 1.00, 1.00, 1.0)
	 	_MainTex ("", 2D) = "white" {}
	}
	 
	SubShader
	{
		Lighting Off
		ZTest Always
		Cull Off
		ZWrite Off
		Fog { Mode Off }
	 
	 	Pass
	 	{
	  		CGPROGRAM
	  		#pragma exclude_renderers flash
	  		#pragma vertex vert_img
	  		#pragma fragment frag
			#pragma fragmentoption ARB_precision_hint_fastest
	  		#include "UnityCG.cginc"
	    
			uniform fixed4 _Color0;
			uniform fixed4 _Color1;
			uniform fixed4 _Color2;
			uniform fixed4 _Color3;
			uniform fixed4 _Color4;
			uniform fixed4 _Color5;
			uniform fixed4 _Color6;
			uniform fixed4 _Color7;
			uniform fixed4 _Color8;
			uniform fixed4 _Color9;
			uniform fixed4 _Color10;
			uniform fixed4 _Color11;
			uniform fixed4 _Color12;
			uniform fixed4 _Color13;
			uniform fixed4 _Color14;
			uniform fixed4 _Color15;
	  		uniform sampler2D _MainTex;
	    
	  		fixed4 frag (v2f_img i) : COLOR
	  		{
	   			fixed3 original = tex2D (_MainTex, i.uv).rgb;

	   			fixed dist0 = distance (original, _Color0.rgb);
	   			fixed dist1 = distance (original, _Color1.rgb);
	   			fixed dist2 = distance (original, _Color2.rgb);
	   			fixed dist3 = distance (original, _Color3.rgb);
	   			fixed dist4 = distance (original, _Color4.rgb);
	   			fixed dist5 = distance (original, _Color5.rgb);
	   			fixed dist6 = distance (original, _Color6.rgb);
	   			fixed dist7 = distance (original, _Color7.rgb);
				fixed dist8 = distance(original, _Color8.rgb);
				fixed dist9 = distance(original, _Color9.rgb);
				fixed dist10 = distance(original, _Color10.rgb);
				fixed dist11 = distance(original, _Color11.rgb);
				fixed dist12 = distance(original, _Color12.rgb);
				fixed dist13 = distance(original, _Color13.rgb);
				fixed dist14 = distance(original, _Color14.rgb);
				fixed dist15 = distance(original, _Color15.rgb);
	   			
	   			fixed4 col = fixed4 (0,0,0,0);
	   			fixed dist = 10.0;

				if (dist0 < dist)
				{
					dist = dist0;
					col = _Color0;
				}
				
				if (dist1 < dist)
				{
					dist = dist1;
					col = _Color1;
				}
				
				if (dist2 < dist)
				{
					dist = dist2;
					col = _Color2;
				}
				
				if (dist3 < dist)
				{
					dist = dist3;
					col = _Color3;
				}

				if (dist4 < dist)
				{
					dist = dist4;
					col = _Color4;
				}

				if (dist5 < dist)
				{
					dist = dist5;
					col = _Color5;
				}

				if (dist6 < dist)
				{
					dist = dist6;
					col = _Color6;
				}

				if (dist7 < dist)
				{
					dist = dist7;
					col = _Color7;
				}

				if (dist8 < dist)
				{
					dist = dist8;
					col = _Color8;
				}

				if (dist9 < dist)
				{
					dist = dist9;
					col = _Color9;
				}

				if (dist10 < dist)
				{
					dist = dist10;
					col = _Color10;
				}

				if (dist11 < dist)
				{
					dist = dist11;
					col = _Color11;
				}

				if (dist12 < dist)
				{
					dist = dist12;
					col = _Color12;
				}

				if (dist13 < dist)
				{
					dist = dist13;
					col = _Color13;
				}

				if (dist14 < dist)
				{
					dist = dist14;
					col = _Color14;
				}

				if (dist15 < dist)
				{
					dist = dist15;
					col = _Color15;
				}

				return col;
	  		}
	  		
	  		ENDCG
	 	}
	}
	
	FallBack "Diffuse"
}
