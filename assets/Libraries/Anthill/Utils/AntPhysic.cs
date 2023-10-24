using UnityEngine;

namespace Anthill.Utils
{
	public static class AntPhysic
	{
		/// <summary>
		/// Applies the force of the explosion to the target body.
		/// </summary>
		/// <param name="aBody">The body to which the force is applied.</param>
		/// <param name="aPosition">Explosion point.</param>
		/// <param name="aForce">Explosion force.</param>
		/// <param name="aRadius">Explosion radius.</param>
		/// <returns></returns>
		public static void AddExplosionForce(Rigidbody2D aBody, Vector3 aPosition, float aForce, float aRadius)
		{
			var dir = aBody.transform.position - aPosition;
			float calc = 1 - (dir.magnitude / aRadius);
			calc = (calc <= 0f) ? 0f : calc;
			aBody.AddForce(dir.normalized * aForce * calc);
		}

		/// <summary>
		/// Calculates the force when two physical bodies collide.
		/// </summary>
		/// <param name="aBody">Moving body.</param>
		/// <param name="aCollider">The obstacle with which the collision occurred.</param>
		/// <returns>Impact force.</returns>
		public static float GetCollisionForce(Rigidbody2D aBody, Collider2D aCollider)
		{
			float impactVelocityX = aBody.velocity.x;
			float impactVelocityY = aBody.velocity.y;
			float impactVelocity;
			float impactForce;
			float impactMass = 1f;

			if (aCollider.attachedRigidbody != null)
			{
				impactVelocityX -= aCollider.attachedRigidbody.velocity.x;
				impactVelocityY -= aCollider.attachedRigidbody.velocity.y;
				impactMass = aCollider.attachedRigidbody.mass;
			}

			impactVelocityX *= Mathf.Sign(impactVelocityX);
			impactVelocityY *= Mathf.Sign(impactVelocityY);
			impactVelocity = impactVelocityX * impactVelocityY;
			impactForce = impactVelocity * aBody.mass * impactMass;
			impactForce *= Mathf.Sign(impactForce);

			return impactForce;

			/*var impactVelocityX = rigidbody.velocity.x - contact.otherCollider.rigidbody.velocity.x;
			impactVelocityX *= Mathf.Sign(impactVelocityX);
			var impactVelocityY = rigidbody.velocity.y - contact.otherCollider.rigidbody.velocity.y;
			impactVelocityY *= Mathf.Sign(impactVelocityY);
			var impactVelocity = impactVelocityX + impactVelocityY;
			var impactForce = impactVelocity * rigidbody.mass * contact.otherCollider.rigidbody.mass;
			impactForce *= Mathf.Sign(impactForce);*/
		}

		public static bool IsInLayerMask(GameObject aObject, LayerMask aLayerMask)
		{
			return ((aLayerMask.value & (1 << aObject.layer)) > 0);
		}

		// Check the point getting inside the body
		// Collider2D collider.bounds.Contains(node.stats.ImpactBullet.ImpactPoint)

	}
}