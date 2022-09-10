using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Config {
	namespace Player {
		public static class Camera {
			public static Vector2 PivotPoint = new Vector3(0,0);
			public static int FOV = 75;
			public static float NearClipPlane = .01F;
		}
		public static class Movement {
			public static float WalkSpeed = 32;
			public static float MaxWalkSpeed = 5;

			public static float RunSpeed = 40;
			public static float MaxRunSpeed = 8;

			public static float StopSpeed = 8;
			public static class Dash {
				public static float DashForce = 13;
				public static float DashMaxForce = 12F;
				public static float DashCooldown = 2;
				public static float DashLength = .2F;
			}
			public static class Jump {
				public static float JumpForce = 6.5F;
				public static float JumpMaxForce = 12F;
				public static float JumpCooldown = 2;
			}
		}
		public static class Controls {
			public static class Movement {
				public static KeyStroke DashRight = new KeyStroke(new List<(KeyCode, float)> {(KeyCode.D, .25F),(KeyCode.D, .25F)} );
				public static KeyStroke DashLeft = new KeyStroke(new List<(KeyCode, float)> {(KeyCode.A, .25F),(KeyCode.A, .25F)} );
			}
		}
	}
}
