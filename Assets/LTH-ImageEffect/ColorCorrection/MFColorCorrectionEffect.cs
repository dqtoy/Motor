//
// By using or accessing the source codes or any other information of the Game SHADOWGUN: DeadZone ("Game"),
// you ("You" or "Licensee") agree to be bound by all the terms and conditions of SHADOWGUN: DeadZone Public
// License Agreement (the "PLA") starting the day you access the "Game" under the Terms of the "PLA".
//
// You can review the most current version of the "PLA" at any time at: http://madfingergames.com/pla/deadzone
//
// If you don't agree to all the terms and conditions of the "PLA", you shouldn't, and aren't permitted
// to use or access the source codes or any other information of the "Game" supplied by MADFINGER Games, a.s.
//

using UnityEngine;

[ExecuteInEditMode]
[AddComponentMenu("Image Effects/MADFINGER color correction")]
public class MFColorCorrectionEffect : LTHImageEffectBase {
	[System.Serializable]
	public struct ColorCorrectArg {
		public float Brightness;
		public float Contrast;
		public float Saturation;
		public float R_offs;
		public float G_offs;
		public float B_offs;

		public ColorCorrectArg(float brightness, float contrast, float saturation, float rOffs, float gOffs, float bOffs) {
			this.Brightness = brightness;
			this.Contrast = contrast;
			this.Saturation = saturation;
			this.R_offs = rOffs;
			this.G_offs = gOffs;
			this.B_offs = bOffs;
		}

		public static ColorCorrectArg MoveToward(ColorCorrectArg from, ColorCorrectArg to, float maxDelta) {
			return new ColorCorrectArg() {
				Brightness = Mathf.MoveTowards(from.Brightness, to.Brightness, maxDelta),
				Contrast = Mathf.MoveTowards(from.Contrast, to.Contrast, maxDelta),
				Saturation = Mathf.MoveTowards(from.Saturation, to.Saturation, maxDelta),
				R_offs = Mathf.MoveTowards(from.R_offs, to.R_offs, maxDelta),
				G_offs = Mathf.MoveTowards(from.G_offs, to.G_offs, maxDelta),
				B_offs = Mathf.MoveTowards(from.B_offs, to.B_offs, maxDelta),
			};
		}
	}


	public ColorCorrectArg Arg = new ColorCorrectArg(1, 1, 1, 0, 0, 0);



	Matrix4x4 CalcSaturationMatrix(float sat) {
		Matrix4x4 mat = Matrix4x4.identity;

		float a, b, c, d, e, f, g, h, i;
		float rwgt, gwgt, bwgt;

		rwgt = 0.3086f;
		gwgt = 0.6094f;
		bwgt = 0.0820f;

		a = (1 - sat) * rwgt + sat;
		b = (1 - sat) * rwgt;
		c = (1 - sat) * rwgt;
		d = (1 - sat) * gwgt;
		e = (1 - sat) * gwgt + sat;
		f = (1 - sat) * gwgt;
		g = (1 - sat) * bwgt;
		h = (1 - sat) * bwgt;
		i = (1 - sat) * bwgt + sat;

		mat[0, 0] = a;
		mat[1, 0] = b;
		mat[2, 0] = c;
		mat[3, 0] = 0;

		mat[0, 1] = d;
		mat[1, 1] = e;
		mat[2, 1] = f;
		mat[3, 1] = 0;

		mat[0, 2] = g;
		mat[1, 2] = h;
		mat[2, 2] = i;
		mat[3, 2] = 0;

		mat[0, 3] = 0;
		mat[1, 3] = 0;
		mat[2, 3] = 0;
		mat[3, 3] = 1;

		return mat;
	}

	Matrix4x4 ColorOffsetMatrix(float rOffs, float gOffs, float bOffs) {
		Matrix4x4 m = Matrix4x4.identity;

		m[0, 3] = rOffs;
		m[1, 3] = gOffs;
		m[2, 3] = bOffs;

		return m;
	}

	Matrix4x4 ColorScaleMatrix(float r, float g, float b) {
		Matrix4x4 m = Matrix4x4.identity;

		m[0, 0] = r;
		m[1, 1] = g;
		m[2, 2] = b;

		return m;
	}

	Matrix4x4 ColorContrastMatrix(float c) {
		return ColorOffsetMatrix(0.5f, 0.5f, 0.5f) * ColorScaleMatrix(c, c, c) * ColorOffsetMatrix(-0.5f, -0.5f, -0.5f);
	}

	// Called by camera to apply image effect
	void OnRenderImage(RenderTexture source, RenderTexture destination) {
		if (material) {
			Matrix4x4 Msat = CalcSaturationMatrix(this.Arg.Saturation);
			Matrix4x4 Mc = ColorContrastMatrix(this.Arg.Contrast);
			Matrix4x4 Mo = ColorOffsetMatrix(this.Arg.R_offs, this.Arg.G_offs, this.Arg.B_offs);
			Matrix4x4 Mb = ColorOffsetMatrix(this.Arg.Brightness - 1, this.Arg.Brightness - 1, this.Arg.Brightness - 1);
			Matrix4x4 m = Mb * Mc * Msat * Mo;

			material.shader = shader;
			material.SetMatrix("_ColorMatrix", m.transpose);
		}

		Graphics.Blit(source, destination, material);
	}
}