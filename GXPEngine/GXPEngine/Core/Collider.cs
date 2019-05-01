using System;

namespace GXPEngine.Core
{
	public class Collider
	{
		public Collider ()
		{
		}
		
		//------------------------------------------------------------------------------------------------------------------------
		//														HitTest()
		//------------------------------------------------------------------------------------------------------------------------		
		public virtual bool HitTest (Collider other) {
			return false;
		}
		
		//------------------------------------------------------------------------------------------------------------------------
		//														HitTest()
		//------------------------------------------------------------------------------------------------------------------------		
		public virtual bool HitTestPoint (float x, float y) {
			return false;
		}

		public virtual float TimeOfImpact (Collider other, float vx, float vy, out Vector2 normal) {
			normal = new Vector2 ();
			return float.MaxValue;
		}
	}
}

