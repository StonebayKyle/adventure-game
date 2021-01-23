﻿using UnityEngine;

public class PhysicsUtils
{
    /// <summary>
    /// Applies a force on <paramref name="rigidbody"/> to reach the target <paramref name="velocity"/> on a linear velocity line. A <paramref name="force"/> of 1 means it takes one second to reach <paramref name="velocity"/> under no special conditions (mass = 1, drag = 0, friction = 0)
    /// </summary>
    /// <param name="rigidbody"></param>
    /// <param name="velocity"></param>
    /// <param name="force"></param>
    /// <param name="mode"></param>
    /// <author>Method insprired by Youtube user DitzelGames</author>
    public static void ApplyForceToReachVelocity(Rigidbody2D rigidbody, Vector2 velocity, float force, ForceMode2D mode = ForceMode2D.Force)
    {
        if (force == 0 || velocity.magnitude == 0)
        {
            return;
        }

        velocity += velocity.normalized * .2f * rigidbody.drag + velocity.normalized * rigidbody.sharedMaterial.friction;
        // why is it clamping between masses/deltatime?
        force = Mathf.Clamp(force, -rigidbody.mass / Time.fixedDeltaTime, rigidbody.mass / Time.fixedDeltaTime);

        if (rigidbody.velocity.magnitude == 0)
        {
            rigidbody.AddForce(velocity * force, mode);
        } else
        {
            var velocityProjectedToTarget = (velocity.normalized * Vector2.Dot(velocity, rigidbody.velocity) / velocity.magnitude);
            rigidbody.AddForce((velocity - velocityProjectedToTarget) * force, mode);
        }

    }
    /// <summary>
    /// Applies a force on <paramref name="rigidbody"/> to reach <paramref name="targetVelocity"/> in (about) <paramref name="accelerationTime"/> seconds .
    /// </summary>
    /// <param name="rigidbody"></param>
    /// <param name="targetVelocity"></param>
    /// <param name="accelerationTime"></param>
    /// <value><paramref name="accelerationTime"/> exact time only applies under perfect conditions, such as no friction. Overall, a lower number means faster and a higher number means slower.</value>
    public static void ApplyForceTowards(Rigidbody2D rigidbody, float targetVelocity, float accelerationTime)
    {
        float xVelocity = 0.0f;

        //float currentVelocity = rigidbody.velocity.x;

        // clamps the current velocity used for force applying between -target and +target, so only a certain amount of force difference is allowed to happen
            // except for when the target is 0. In that case, keep it as velocity directly so a change is allowed to happen (subtracting smoothedVelocity by 0 doesn't change velocity).
        float currentVelocity = (targetVelocity == 0) ? rigidbody.velocity.x : Mathf.Clamp(rigidbody.velocity.x, -Mathf.Abs(targetVelocity), Mathf.Abs(targetVelocity));

        // calculate the target velocity for a single run through this method, which is smoothed.
        float smoothedVelocity = Mathf.SmoothDamp(currentVelocity, targetVelocity, ref xVelocity, accelerationTime, Mathf.Infinity, Time.fixedDeltaTime);

        //rigidbody.velocity = new Vector2(smoothedVelocity, rigidbody.velocity.y);
        // difMath = velNow - velBefore
        // dif = smooth - rb.vel.x

        // F = m(dv/dt)
        Vector2 force = new Vector2(rigidbody.mass * 
            ((smoothedVelocity - currentVelocity)/Time.fixedDeltaTime)
            , 0);
        rigidbody.AddForce(force);
    }


    /// <summary>
    /// Overload of <see cref="ApplyForceTowards(Rigidbody2D, float, float)"/> with <paramref name="friction"/>, which modifies accelerationTime.
    /// </summary>
    /// <param name="rigidbody"></param>
    /// <param name="targetVelocity"></param>
    /// <param name="accelerationTime"></param>
    /// <param name="friction"></param>
    public static void ApplyForceTowards(Rigidbody2D rigidbody, float targetVelocity, float accelerationTime, float friction)
    {
        ApplyForceTowards(rigidbody, targetVelocity, accelerationTime * friction);
    }
}