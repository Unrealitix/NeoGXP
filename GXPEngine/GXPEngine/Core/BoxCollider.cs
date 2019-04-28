using System;

namespace GXPEngine.Core
{
	public class BoxCollider : Collider
	{
		private Sprite _owner;
		
		//------------------------------------------------------------------------------------------------------------------------
		//														BoxCollider()
		//------------------------------------------------------------------------------------------------------------------------		
		public BoxCollider(Sprite owner) {
			_owner = owner;
		}
		
		//------------------------------------------------------------------------------------------------------------------------
		//														HitTest()
		//------------------------------------------------------------------------------------------------------------------------		
		public override bool HitTest (Collider other) {
			if (other is BoxCollider) {
				Vector2[] c = _owner.GetExtents();
				if (c == null) return false;
				Vector2[] d = ((BoxCollider)other)._owner.GetExtents();
				if (d == null) return false;
				if (!areaOverlap(c, d)) return false;
				return areaOverlap(d, c);
			} else {
				return false;
			}
		}

		//------------------------------------------------------------------------------------------------------------------------
		//														HitTest()
		//------------------------------------------------------------------------------------------------------------------------		
		public override bool HitTestPoint (float x, float y) {
			Vector2[] c = _owner.GetExtents();
			if (c == null) return false;
			Vector2 p = new Vector2(x, y);
			return pointOverlapsArea(p, c);
		}
		
		//------------------------------------------------------------------------------------------------------------------------
		//														areaOverlap()
		//------------------------------------------------------------------------------------------------------------------------
		private bool areaOverlap(Vector2[] c, Vector2[] d) {
			// normal 1:
			float ny = c[1].x - c[0].x;
			float nx = c[0].y - c[1].y;
			// own 'depth' in direction of this normal:
			float dx = c[3].x - c[0].x;
			float dy = c[3].y - c[0].y;
			float dot = (dy * ny + dx * nx);

			if (dot == 0.0f) dot = 1.0f;

			float t, minT, maxT;

			t = ((d[0].x - c[0].x) * nx + (d[0].y - c[0].y) * ny) / dot;
			maxT = t; minT = t;

			t = ((d[1].x - c[0].x) * nx + (d[1].y - c[0].y) * ny) / dot;
			minT = Math.Min(minT,t); maxT = Math.Max(maxT,t);

			t = ((d[2].x - c[0].x) * nx + (d[2].y - c[0].y) * ny) / dot;
			minT = Math.Min(minT,t); maxT = Math.Max(maxT,t);

			t = ((d[3].x - c[0].x) * nx + (d[3].y - c[0].y) * ny) / dot;
			minT = Math.Min(minT,t); maxT = Math.Max(maxT,t);

			if ((minT >= 1) || (maxT <= 0)) return false;

			// second normal:
			ny = dx;
			nx = -dy;
			dx = c[1].x - c[0].x;
			dy = c[1].y - c[0].y;
			dot = (dy * ny + dx * nx);

			if (dot == 0.0f) dot = 1.0f;

			t = ((d[0].x - c[0].x) * nx + (d[0].y - c[0].y) * ny) / dot;
			maxT = t; minT = t;

			t = ((d[1].x - c[0].x) * nx + (d[1].y - c[0].y) * ny) / dot;
			minT = Math.Min(minT,t); maxT = Math.Max(maxT,t);

			t = ((d[2].x - c[0].x) * nx + (d[2].y - c[0].y) * ny) / dot;
			minT = Math.Min(minT,t); maxT = Math.Max(maxT,t);

			t = ((d[3].x - c[0].x) * nx + (d[3].y - c[0].y) * ny) / dot;
			minT = Math.Min(minT,t); maxT = Math.Max(maxT,t);

			if ((minT >= 1) || (maxT <= 0)) return false;

			return true;
		}
		
		//------------------------------------------------------------------------------------------------------------------------
		//														pointOverlapsArea()
		//------------------------------------------------------------------------------------------------------------------------
		//ie. for hittestpoint and mousedown/up/out/over
		private bool pointOverlapsArea(Vector2 p, Vector2[] c) {
			float dx1 = c[1].x - c[0].x;
			float dy1 = c[1].y - c[0].y;
			float dx2 = c[3].x - c[0].x;
			float dy2 = c[3].y - c[0].y;
			// first: take delta1 as normal:
			float dot = dy2 * dx1 - dx2 * dy1; 

			float t;

			t = ((p.y - c[0].y) * dx1 - (p.x - c[0].x) * dy1) / dot;
			if ((t > 1) || (t < 0))	return false;

			// next: take delta2 as normal:
			dot = -dot;

			t = ((p.y - c[0].y) * dx2 - (p.x - c[0].x) * dy2) / dot;

			if ((t > 1) || (t < 0)) return false;

			return true;			
		}	
	}
}


