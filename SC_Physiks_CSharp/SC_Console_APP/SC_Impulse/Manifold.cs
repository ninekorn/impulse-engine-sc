/*
    Copyright (c) 2013 Randy Gaul http://RandyGaul.net
    This software is provided 'as-is', without any express or implied
    warranty. In no event will the authors be held liable for any damages
    arising from the use of this software.
    Permission is granted to anyone to use this software for any purpose,
    including commercial applications, and to alter it and redistribute it
    freely, subject to the following restrictions:
      1. The origin of this software must not be misrepresented; you must not
         claim that you wrote the original software. If you use this software
         in a product, an acknowledgment in the product documentation would be
         appreciated but is not required.
      2. Altered source versions must be plainly marked as such, and must not be
         misrepresented as being the original software.
      3. This notice may not be removed or altered from any source distribution.
      
    Port to Java by Philip Diffenderfer http://magnos.org
    Port to C# by Steve Chassé  //https://twitter.com/sccoresystems1 //https://ninekorn.imgbb.com/ //https://www.youtube.com/watch?v=yWspu7zvbBU //https://www.twitch.tv/ninekorn
*/



using System;

namespace SCCoreSystems
{

    public class Manifold
    {
        public Body A;
        public Body B;
        public float penetration;
        public Vec2 normal = new Vec2();
        public Vec2[] contacts = { new Vec2(), new Vec2() };
        public int contactCount;
        public float e;
        public float df;
        public float sf;

        //CollisionCircleCircle colHandleCC = new CollisionCircleCircle();
        //CollisionPolygonCircle colHandlePC = new CollisionPolygonCircle();
        //CollisionCirclePolygon colHandleCP = new CollisionCirclePolygon();
        //CollisionPolygonPolygon colHandlePP = new CollisionPolygonPolygon();




        public Manifold(Body a, Body b)
        {
            A = a;
            B = b;
        }

        public void solve()
        {

            /*int ia = A.shape.getType().ordinal();
            int ib = B.shape.getType().ordinal();

            Collisions.dispatch[ia][ib].handleCollision(this, A, B);*/

            int ia = -1;
            if (A.shape.getType() == Shape.Type.Circle)
            {
                ia = 0;
            }
            else if (A.shape.getType() == Shape.Type.Poly)
            {
                ia = 1;
            }
            /*else if (A.shape.getType() == Shape.Type.Count)
            {
                ia = 2;
            }*/

            int ib = -1;
            if (B.shape.getType() == Shape.Type.Circle)
            {
                ib = 0;
            }
            else if (B.shape.getType() == Shape.Type.Poly)
            {
                ib = 1;
            }
            /*else if (B.shape.getType() == Shape.Type.Count)
            {
                ib = 2;
            }*/

            //Collisions.dispatch[ia][ib].handleCollision(this, A, B);








            //int ia = 1;
            //int ib = 1;

            //int ia = A.shape.getType();//.ordinal();
            //int ib = B.shape.getType();//.ordinal();

            //Collisions.dispatch[ia][ib].handleCollision(this, A, B);

            /*if (ia == 0 && ib == 0)
            {
                CollisionCircleCircle colHandleCC = new CollisionCircleCircle();
         
                colHandleCC.handleCollision(this, A, B);
            }
            else if (ia == 1 && ib == 0)
            {
                CollisionPolygonCircle colHandlePC = new CollisionPolygonCircle();
                colHandlePC.handleCollision(this, A, B);
            }
            else if (ia == 0 && ib == 1)
            {
                CollisionCirclePolygon colHandleCP = new CollisionCirclePolygon();
                colHandleCP.handleCollision(this, A, B);
            }*/
            if (ia == 1 && ib == 1)
            {
                CollisionPolygonPolygon colHandlePP = new CollisionPolygonPolygon();
                colHandlePP.handleCollision(this, A, B);
            }
        }

        public void initialize()
        {
            // Calculate average restitution
            // e = std::min( A->restitution, B->restitution );
            e = Math.Min(A.restitution, B.restitution);

            // Calculate static and dynamic friction
            // sf = std::sqrt( A->staticFriction * A->staticFriction );
            // df = std::sqrt( A->dynamicFriction * A->dynamicFriction );
            sf = (float)Math.Sqrt(A.staticFriction * A.staticFriction  + B.staticFriction * B.staticFriction);
            df = (float)Math.Sqrt(A.dynamicFriction * A.dynamicFriction + B.dynamicFriction * B.dynamicFriction);


            for (int i = 0; i < contactCount; ++i)
            {
                // Calculate radii from COM to contact
                // Vec2 ra = contacts[i] - A->position;
                // Vec2 rb = contacts[i] - B->position;
                Vec2 ra = contacts[i].sub(A.position);
                Vec2 rb = contacts[i].sub(B.position);

                // Vec2 rv = B->velocity + Cross( B->angularVelocity, rb ) -
                // A->velocity - Cross( A->angularVelocity, ra );
                Vec2 rv = B.velocity.add(Vec2.cross(B.angularVelocity, rb, new Vec2())).subi(A.velocity).subi(Vec2.cross(A.angularVelocity, ra, new Vec2()));

                // Determine if we should perform a resting collision or not
                // The idea is if the only thing moving this object is gravity,
                // then the collision should be performed without any restitution
                // if(rv.LenSqr( ) < (dt * gravity).LenSqr( ) + EPSILON)

                //Console.WriteLine(rv.lengthSq());
                if (rv.lengthSq() < ImpulseMath.RESTING) //ImpulseMath.RESTING
                {
                    e = 0.0f;
                }
            }
        }


        int veloA = 0;
        int veloB = 0;

        public void applyImpulse()
        {
            /*veloA = 0;
            veloB = 0;

            if (Math.Abs(A.velocity.x) + Math.Abs(A.velocity.y) < 0.03f)//0.01f)
            {
                A.velocity.x = 0;
                A.velocity.y = 0;
                A.angularVelocity = 0;
                A.force.x = 0;
                A.force.y = 0;
                A.torque = 0;

                veloA = 1;
            }

            if (Math.Abs(B.velocity.x) + Math.Abs(B.velocity.y) < 0.03f)//0.01f)
            {
                B.velocity.x = 0;
                B.velocity.y = 0;
                B.angularVelocity = 0;
                B.force.x = 0;
                B.force.y = 0;
                B.torque = 0;

                veloB = 1;
            }

            if (veloA == 1 && veloB == 1)
            {
                return;
            }*/

            /*if (A.torque < ImpulseMath.EPSILON)//0.01f)
            {
                A.velocity.x = 0;
                A.velocity.y = 0;
                A.angularVelocity = 0;
                A.force.x = 0;
                A.force.y = 0;
                A.torque = 0;

                veloA = 1;
            }
            if (B.torque < ImpulseMath.EPSILON)//0.01f)
            {
                B.velocity.x = 0;
                B.velocity.y = 0;
                B.angularVelocity = 0;
                B.force.x = 0;
                B.force.y = 0;
                B.torque = 0;

                veloB = 1;
            }*/



            /*if (Math.Abs(A.force.x) + Math.Abs(A.force.y) < ImpulseMath.EPSILON)//0.01f)
            {
                A.velocity.x = 0;
                A.velocity.y = 0;
                A.angularVelocity = 0;
                A.force.x = 0;
                A.force.y = 0;
                A.torque = 0;

                veloA = 1;
            }

            if (Math.Abs(B.force.x) + Math.Abs(B.force.y) < ImpulseMath.EPSILON)//0.01f)
            {
                B.velocity.x = 0;
                B.velocity.y = 0;
                B.angularVelocity = 0;
                B.force.x = 0;
                B.force.y = 0;
                B.torque = 0;

                veloB = 1;
            }*/





            // Early out and positional correct if both objects have infinite mass
            // if(Equal( A->im + B->im, 0 ))
            if (ImpulseMath.equal(A.invMass + B.invMass, 0))
            {
                infiniteMassCorrection();
                return;
            }

            for (int i = 0; i < contactCount; ++i)
            {
                // Calculate radii from COM to contact
                // Vec2 ra = contacts[i] - A->position;
                // Vec2 rb = contacts[i] - B->position;
                Vec2 ra = contacts[i].sub(A.position);
                Vec2 rb = contacts[i].sub(B.position);

                // Relative velocity
                // Vec2 rv = B->velocity + Cross( B->angularVelocity, rb ) -
                // A->velocity - Cross( A->angularVelocity, ra );
                Vec2 rv = B.velocity.add(Vec2.cross(B.angularVelocity, rb, new Vec2())).subi(A.velocity).subi(Vec2.cross(A.angularVelocity, ra, new Vec2()));

                // Relative velocity along the normal
                // real contactVel = Dot( rv, normal );
                float contactVel = Vec2.dot(rv, normal);

                // Do not resolve if velocities are separating
                if (contactVel > 0)
                {
                    return;
                }

                float raCrossN = Vec2.cross(ra, normal);
                float rbCrossN = Vec2.cross(rb, normal);
                float invMassSum = A.invMass + B.invMass + (raCrossN * raCrossN) * A.invInertia + (rbCrossN * rbCrossN) * B.invInertia;

                // Calculate impulse scalar
                float j = -(1.0f + e) * contactVel;
                j /= invMassSum;
                j /= contactCount;

                // Apply impulse
                Vec2 impulse = normal.mul(j);
                A.applyImpulse(impulse.neg(), ra);
                B.applyImpulse(impulse, rb);

                // Friction impulse
                rv = B.velocity.add(Vec2.cross(B.angularVelocity, rb, new Vec2())).subi(A.velocity).subi(Vec2.cross(A.angularVelocity, ra, new Vec2()));

                Vec2 t = new Vec2(rv);
                t.addsi(normal, -Vec2.dot(rv, normal));
                t.normalize();

                // j tangent magnitude
                float jt = -Vec2.dot(rv, t);
                jt /= invMassSum;
                jt /= contactCount;

                // Don't apply tiny friction impulses
                if (ImpulseMath.equal(jt, 0.0f))
                {
                    return;
                }

                // Coulumb's law
                Vec2 tangentImpulse = new Vec2(0, 0);
                if (Math.Abs(jt) < j * sf) // j * sf
                {
                    //Console.WriteLine(jt + " ___ " + sf);
                    tangentImpulse = t.mul(jt);
                }
                else
                {
                    tangentImpulse = t.mul(j).muli(-df);
                }

                // Apply friction impulse
                A.applyImpulse(tangentImpulse.neg(), ra);
                B.applyImpulse(tangentImpulse, rb);
            }
        }

        public void positionalCorrection()
        {
            float correction = Math.Max(penetration - ImpulseMath.PENETRATION_ALLOWANCE, 0.0f) / (A.invMass + B.invMass) * ImpulseMath.PENETRATION_CORRECTION;

            /*Vec2 ACor = normal;//
            Vec2 BCor = normal;//

            ACor.x *= ImpulseMath.PENETRATION_CORRECTION;
            ACor.y *= ImpulseMath.PENETRATION_CORRECTION;
            BCor.x *= ImpulseMath.PENETRATION_CORRECTION;
            BCor.y *= ImpulseMath.PENETRATION_CORRECTION;


            ACor.x *= A.invMass* correction;
            ACor.y *= A.invMass* correction;
            BCor.x *= B.invMass * correction;
            BCor.y *= B.invMass * correction;


            A.position.sub(ACor);
            B.position.add(BCor);*/



            /*ACor.x *= correction;
            ACor.y *= correction;
            ACor.x *= A.invMass;
            ACor.y *= A.invMass;


            BCor.x *= correction;
            BCor.y *= correction;
            BCor.x *= B.invMass;
            BCor.y *= B.invMass;

            A.position.subi(normal);
            B.position.x += normal.x;
            B.position.y += normal.y;*/
            //A.position.add

            A.position.addsi(normal, -A.invMass * correction);
            B.position.addsi(normal, B.invMass * correction);
        }

        public void infiniteMassCorrection()
        {
            //A.force.set(0, 0);
            //A.torque = 0;
            //B.force.set(0, 0);
            //B.torque = 0;

            A.velocity.set(0, 0);
            B.velocity.set(0, 0);
            //A.angularVelocity = 0;
            //B.angularVelocity = 0;
        }
    }
}
