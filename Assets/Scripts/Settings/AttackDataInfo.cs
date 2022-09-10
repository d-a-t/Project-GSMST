using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace AttackDataInfo {
    public static class Fighter {
		public static AttackData Light = new AttackData(
			rawDamage: 10,
			blockDamage: 1,

			hitstun: .25F,
			blockstun: .05F,

			heatGain: .1F,

			hitForce: Vector2.right * 250,
			blockForce: Vector2.right * 5
		);

		public static AttackData Medium = new AttackData(
			rawDamage: 10,
			blockDamage: 1,
			hitstun: .4F,
			blockstun: .1F,
			heatGain: .15F,
			hitForce: Vector2.right * 250,
			blockForce: Vector2.right * 5
		);

		public static AttackData Heavy = new AttackData(
			rawDamage: 10,
			blockDamage: 1,
			hitstun: .4F,
			blockstun: .1F,
			heatGain: .1F,
			hitForce: Vector2.right * 250,
			blockForce: Vector2.right * 5
		);

		public static AttackData DP = new AttackData(
			rawDamage: 25,
			blockDamage: 5,
			hitstun: .5F,
			blockstun: .25F,
			heatGain: .1F,
			hitForce: (Vector2.right * 350) + (Vector2.up * 750),
			blockForce: Vector2.right * 10
		);
	}
}
