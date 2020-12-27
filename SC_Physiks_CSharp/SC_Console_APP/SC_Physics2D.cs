using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using System.Threading;
using System;
using System.Drawing;

using SharpDX;

//https://gamedev.stackexchange.com/questions/136073/how-does-one-calculate-the-surface-normal-in-2d-collisions
//https://stackoverflow.com/questions/2034540/calculating-area-of-irregular-polygon-in-c-sharp
//https://www.c-sharpcorner.com/code/407/calculate-area-for-rectangle-square-triangle-circle-in-c-sharp.aspx
//https://en.wikipedia.org/wiki/Impulse_(physics)
//https://www.khanacademy.org/science/ap-physics-1/ap-linear-momentum/introduction-to-linear-momentum-and-impulse-ap/v/introduction-to-momentum

using Segment = SC_Physics.SC_Segment.Segment;

namespace SC_Physics
{
    public class SC_Physics2D 
    {
        SC_RigidInfo rect1;
        SC_RigidInfo rect2;
        SC_RigidInfo rect3;

        public SC_RigidInfo[] arrayOfGameObjects;
        public SC_RigidInfo[] arrayOfGhostObjects;

        Stopwatch testStopWatch = new Stopwatch();

        public bool sendTriggerMessage = false;

        double dt = 0;
        float gravity = -1.81f; //-9.81f

        Vector2? lastNormVec;

        float highExtent = 0.9999f;
        float lowExtent = 0.0001f;

        void Start()
        {
            testStopWatch.Stop();
            testStopWatch.Reset();
            testStopWatch.Start();

            arrayOfGameObjects = new SC_RigidInfo[3];
            rect1 = new SC_RigidInfo();
            //rect1.currentObject = one;
            rect1.position = one.transform.position;
            rect1.velocity = new Vector2(0,0);
            rect1.mass = 10;
            rect1.isStatic = 0;
            rect1.lastFramePos = rect1.position;
            rect1.lerp = 0;
            rect1.lerpPos = null;
            rect1.timeToHit = 0;
            rect1.indexToCollideWith = -1;
            rect1.collideNorm = null;
            rect1.mainIndex = -1;
            rect1.collidePoint = null;
            rect1.lastFrameVelo = new Vector2(0, 0);
            rect1.extendedVelocity = new Vector2(0, 0);
            rect1.restitution = 0.25f;
            rect1.isStaticTemp = 0;
            rect1.angle = one.transform.rotation.eulerAngles.z;
            rect1.angularVelocity = 0;
            rect1.angularAcceleration = 0;
            rect1.torque = 0;
            rect1.inertia = 1000;
            rect1.invMass = (rect1.mass != 0.0f) ? 1.0f / rect1.mass : 0.0f;
            rect1.orientation = 0;
            rect1.invInertia = (rect1.inertia != 0.0f) ? 1.0f / rect1.inertia : 0.0f;
            rect1.angularVel = new Vector2(0, 0);

            //UnityEngine.Debug.Log(rect1.invMass);

            rect2 = new SC_RigidInfo();
            //rect2.currentObject = two;
            rect2.position = two.transform.position;
            rect2.velocity = new Vector2(0, 0);
            rect2.mass = 10;
            rect2.isStatic = 1;
            rect2.lastFramePos = rect2.position;
            rect2.lerp = 1;
            rect2.lerpPos = null;
            rect2.timeToHit = 0;
            rect2.indexToCollideWith = -1;
            rect2.collideNorm = null;
            rect2.mainIndex = -1;
            rect2.collidePoint = null;
            rect2.lastFrameVelo = new Vector2(0, 0);
            rect2.extendedVelocity = new Vector2(0, 0);
            rect2.restitution = 0.25f;
            rect2.isStaticTemp = 0;
            rect2.angle = 0;
            rect2.angularVelocity = 0;
            rect2.angularAcceleration = 0;
            rect2.torque = 0;
            rect2.inertia = 1000;
            rect2.invMass = (rect2.mass != 0.0f) ? 1.0f / rect2.mass : 0.0f;
            rect2.orientation = 0;
            rect2.invInertia = (rect2.inertia != 0.0f) ? 1.0f / rect2.inertia : 0.0f;



            rect3 = new SC_RigidInfo();
            //rect3.currentObject = floor;
            rect3.position = floor.transform.position;
            rect3.velocity = new Vector2(0, 1);
            rect3.mass = 10000;
            rect3.isStatic = 1;
            rect3.lastFramePos = rect3.position;
            rect3.lerp = 0;
            rect3.lerpPos = null;
            rect3.timeToHit = 0;
            rect3.indexToCollideWith = -1;
            rect3.collideNorm = null;
            rect3.mainIndex = -1;
            rect3.collidePoint = null;
            rect3.lastFrameVelo = new Vector2(0, 1);
            rect3.extendedVelocity = new Vector2(0, 1);
            rect3.restitution = 0.25f;
            rect3.isStaticTemp = 0;
            rect3.angle = 0;
            rect3.angularVelocity = 0;
            rect3.angularAcceleration = 0;
            rect3.torque = 0;
            rect3.inertia = 1000;
            rect3.invMass = (rect3.mass != 0.0f) ? 1.0f / rect3.mass : 0.0f;
            rect3.orientation = 0;
            rect3.invInertia = (rect3.inertia != 0.0f) ? 1.0f / rect3.inertia : 0.0f;




            arrayOfGameObjects[0] = rect1;
            arrayOfGameObjects[1] = rect2;
            arrayOfGameObjects[2] = rect3;

            for (int i = 0; i < arrayOfGameObjects.Length; i++)
            {
                arrayOfGameObjects[i].vertices = new Vector2[4];
            }

            for (int i = 0; i < arrayOfGameObjects.Length; i++)
            {
                var lowestX = arrayOfGameObjects[i].currentObject.GetComponent<MeshFilter>().mesh.vertices.OrderBy(x => x.x).FirstOrDefault();
                var highestX = arrayOfGameObjects[i].currentObject.GetComponent<MeshFilter>().mesh.vertices.OrderBy(x => x.x).Last();

                var lowestY = arrayOfGameObjects[i].currentObject.GetComponent<MeshFilter>().mesh.vertices.OrderBy(y => y.y).FirstOrDefault();
                var highestY = arrayOfGameObjects[i].currentObject.GetComponent<MeshFilter>().mesh.vertices.OrderBy(y => y.y).Last();

                arrayOfGameObjects[i].boundsLowX = lowestX;
                arrayOfGameObjects[i].boundsHighX = highestX;
                arrayOfGameObjects[i].boundsLowY = lowestY;
                arrayOfGameObjects[i].boundsHighY = highestY;

                //arrayOfGameObjects[i].boundMinX = lowestX;
                //arrayOfGameObjects[i].boundMaxX = highestX;

                arrayOfGameObjects[i].mainIndex = i;
            }

            for (int i = 0; i < arrayOfGameObjects.Length; i++)
            {
                Vector3 cornerBound1 = arrayOfGameObjects[i].boundsLowX;
                if (arrayOfGameObjects[i].boundsLowX.y > arrayOfGameObjects[i].boundsLowY.y)
                {
                    cornerBound1 = new Vector3(arrayOfGameObjects[i].boundsLowX.x, arrayOfGameObjects[i].boundsLowY.y);
                }
                Vector3 cornerBound2 = arrayOfGameObjects[i].boundsLowY;
                if (arrayOfGameObjects[i].boundsLowY.x < arrayOfGameObjects[i].boundsHighX.x)
                {
                    cornerBound2 = new Vector3(arrayOfGameObjects[i].boundsHighX.x, arrayOfGameObjects[i].boundsLowY.y);
                }
                Vector3 cornerBound3 = arrayOfGameObjects[i].boundsHighX;
                if (arrayOfGameObjects[i].boundsHighX.y < arrayOfGameObjects[i].boundsHighY.y)
                {
                    cornerBound3 = new Vector3(arrayOfGameObjects[i].boundsHighX.x, arrayOfGameObjects[i].boundsHighY.y);
                }

                Vector3 cornerBound4 = arrayOfGameObjects[i].boundsHighY;
                if (arrayOfGameObjects[i].boundsHighY.x > arrayOfGameObjects[i].boundsLowX.x)
                {
                    cornerBound4 = new Vector3(arrayOfGameObjects[i].boundsLowX.x, arrayOfGameObjects[i].boundsHighY.y);
                }
                arrayOfGameObjects[i].corner1 = cornerBound1;
                arrayOfGameObjects[i].corner2 = cornerBound2;
                arrayOfGameObjects[i].corner3 = cornerBound3;
                arrayOfGameObjects[i].corner4 = cornerBound4;

                //arrayOfGameObjects[i].vertices[0] = arrayOfGameObjects[i].corner1;// arrayOfGameObjects[i].currentObject.transform.TransformPoint(arrayOfGameObjects[i].corner1);
                //arrayOfGameObjects[i].vertices[1] = arrayOfGameObjects[i].corner2;//arrayOfGameObjects[i].currentObject.transform.TransformPoint(arrayOfGameObjects[i].corner2);
                //arrayOfGameObjects[i].vertices[2] = arrayOfGameObjects[i].corner3;//arrayOfGameObjects[i].currentObject.transform.TransformPoint(arrayOfGameObjects[i].corner3);
                //arrayOfGameObjects[i].vertices[3] = arrayOfGameObjects[i].corner4;//arrayOfGameObjects[i].currentObject.transform.TransformPoint(arrayOfGameObjects[i].corner4);

                arrayOfGameObjects[i].vertices[0] = arrayOfGameObjects[i].currentObject.transform.TransformPoint(arrayOfGameObjects[i].corner1);
                arrayOfGameObjects[i].vertices[1] = arrayOfGameObjects[i].currentObject.transform.TransformPoint(arrayOfGameObjects[i].corner2);
                arrayOfGameObjects[i].vertices[2] = arrayOfGameObjects[i].currentObject.transform.TransformPoint(arrayOfGameObjects[i].corner3);
                arrayOfGameObjects[i].vertices[3] = arrayOfGameObjects[i].currentObject.transform.TransformPoint(arrayOfGameObjects[i].corner4);

                Segment one1 = new Segment();
                one1.Start = arrayOfGameObjects[i].corner1;
                one1.End = arrayOfGameObjects[i].corner2;
                one1.Normal = (arrayOfGameObjects[i].corner2 - arrayOfGameObjects[i].corner1).normalized;
                Vector2 tester = one1.Normal;
                one1.Normal.x = one1.Normal.y;
                one1.Normal.y = -tester.x;


                Segment two1 = new Segment();
                two1.Start = arrayOfGameObjects[i].corner2;
                two1.End = arrayOfGameObjects[i].corner3;
                two1.Normal = (arrayOfGameObjects[i].corner3 - arrayOfGameObjects[i].corner2).normalized;
                tester = two1.Normal;
                two1.Normal.x = two1.Normal.y;
                two1.Normal.y = -tester.x;

                Segment three1 = new Segment();
                three1.Start = arrayOfGameObjects[i].corner3;
                three1.End = arrayOfGameObjects[i].corner4;
                three1.Normal = (arrayOfGameObjects[i].corner4 - arrayOfGameObjects[i].corner3).normalized;
                tester = three1.Normal;
                three1.Normal.x = three1.Normal.y;
                three1.Normal.y = -tester.x;

                Segment four1 = new Segment();
                four1.Start = arrayOfGameObjects[i].corner4;
                four1.End = arrayOfGameObjects[i].corner1;
                four1.Normal = (arrayOfGameObjects[i].corner1 - arrayOfGameObjects[i].corner4).normalized;
                tester = four1.Normal;
                four1.Normal.x = four1.Normal.y;
                four1.Normal.y = -tester.x;

                Segment[] arrayOfBoundsSegments00 = new Segment[4];
                arrayOfBoundsSegments00[0] = one1;
                arrayOfBoundsSegments00[1] = two1;
                arrayOfBoundsSegments00[2] = three1;
                arrayOfBoundsSegments00[3] = four1;

                arrayOfGameObjects[i].edgeSegments = arrayOfBoundsSegments00;
            }

            tsk = DoWork(1);
        }

        float theGoalDist = 0;
        float nextPosGoalDist = 0;


        SC_RigidInfo obj11;
        SC_RigidInfo obj22;

        Vector2 vTmp;

        Vector2 CrossProduct(Vector2 v)
        {
            return new Vector2(v.y, -v.x);
        }

        public float cpvcross(Vector2 v1, Vector2 v2)
        {
            return v1.x * v2.y - v1.y * v2.x;
        }


        int ResolveCollision(SC_RigidInfo aObject, SC_RigidInfo bObject, out float? aVeloX, out float? aVeloY, float theTime)
        {
            aVeloX = null;
            aVeloY = null;

            Vector2 relativeVel = aObject.velocity - bObject.velocity;
            relativeVel.Normalize();


            /*float someDot = Dot(relativeVel.x, relativeVel.y, aObject.collideNorm.Value.x, aObject.collideNorm.Value.y);

            if(someDot > 0)
            {
                //UnityEngine.Debug.Log("velocities are separating?");
                return 0;
            }*/

            float aMass = aObject.invMass;
            float bMass = bObject.invMass;

            Vector2 beforeColA = (aMass * aObject.velocity) + (bMass * bObject.velocity);
            Vector2 afterColA = (beforeColA / (aMass + bMass));
            Vector2 someNewVelo = (((aMass * (-afterColA)) - (aMass * (aObject.velocity)))) * theTime;

            float someMag = afterColA.magnitude;
            someNewVelo += (aObject.collideNorm.Value.normalized * someMag);
            someNewVelo *= aObject.restitution;

            aVeloX = someNewVelo.x;
            aVeloY = someNewVelo.y;

            aObject.velocity = someNewVelo;



            //https://en.wikipedia.org/wiki/Angular_velocity
            Vector2 pos = aObject.currentObject.transform.position;
            var tangent = aObject.collidePoint.Value - pos;
            var someR = new Vector2(-tangent.y, tangent.x);
            var dist = Vector2.Distance(aObject.currentObject.transform.position, aObject.collidePoint.Value);
            var test = (someR / dist) * aObject.velocity;

            aObject.angularVel = test;



            //var vTmp = aObject.currentObject.transform.position;
            //aObject.angularVelocity = (cross(vTmp, someNewVelo) / aObject.inertia);
            //aObject.angle += aObject.angularVelocity;
            //aObject.angularVelocity += aObject.angularAcceleration;
            //aObject.angularAcceleration = aObject.torque / aObject.inertia;

            //aObject.angularVel *= aObject.angularAcceleration;
            //aObject.angularAcceleration = aObject.torque / aObject.inertia;



            //var vTmp = aObject.currentObject.transform.position;
            //aObject.angularVelocity = (cross(vTmp, someNewVelo) / aObject.inertia);
            //aObject.angle += aObject.angularVelocity;
            //aObject.angularVelocity += aObject.angularAcceleration;
            //aObject.angularAcceleration = aObject.torque / aObject.inertia;












            //w = v/r
            //var W = aObject.velocity / Vector2.Distance(aObject.currentObject.transform.position, aObject.collidePoint.Value);
            //UnityEngine.Debug.Log(W);




            //vTmp = (someNewVelo);
            //vTmp *= aObject.invMass;// (1 / aObject.mass);
            //vTmp = aObject.currentObject.transform.position;
            //var crossProd = cpvcross(vTmp, someNewVelo);

            //aObject.angularVelocity += (crossProd / aObject.inertia);       
            //aObject.angle += aObject.angularVelocity;
            //aObject.angularVelocity += aObject.angularAcceleration;
            //aObject.angularAcceleration = aObject.torque / aObject.inertia;
            //UnityEngine.Debug.Log(aObject.angularVelocity);

            //L = Inertia * angular speed
            //linear momentum = mass * velocity
            //moment of inertia = mass * r*r * angular speed
            //L = m*v*r*sinTheta
            //float r = Vector2.Distance(aObject.currentObject.transform.position, aObject.collidePoint.Value);
            //var l = aObject.mass * aObject.velocity * lowExtent;
            //UnityEngine.Debug.Log(l);










            //aObject.orientation += (float)(aObject.angularVelocity * theTime);
            //UnityEngine.Debug.Log(aObject.orientation);
            //double degrees = (180 / Mathf.PI) * aObject.orientation;
            //UnityEngine.Debug.Log(aObject.angle);
            //arrayOfGameObjects[i].angle += arrayOfGameObjects[i].angularVelocity;
            //arrayOfGameObjects[i].angularVelocity += arrayOfGameObjects[i].angularAcceleration;
            //arrayOfGameObjects[i].angularAcceleration = arrayOfGameObjects[i].torque / arrayOfGameObjects[i].inertia;

            ///float Angle = arrayOfGameObjects[i].currentObject.transform.rotation.eulerAngles.z;
            //Quaternion someAddedRot = Quaternion.AngleAxis(Angle + -arrayOfGameObjects[i].angle, Vector3.forward);
            //arrayOfGameObjects[i].currentObject.transform.rotation = Quaternion.Slerp(arrayOfGameObjects[i].currentObject.transform.rotation, someAddedRot, theTime*10);


            //UnityEngine.Debug.Log(aObject.angularVelocity);




            /*float rx = (float)Mathf.Cos(aObject.angle) * 1;// c.radius;
            float ry = (float)Mathf.Sin(aObject.angle) * 1;//c.radius;

            Vector2 dirToPoint = new Vector2(aObject.currentObject.transform.position.x + rx, aObject.currentObject.transform.position.y + ry) - new Vector2(aObject.currentObject.transform.position.x, aObject.currentObject.transform.position.y);
            dirToPoint.Normalize();

            Vector2 originPosPoint = new Vector2(aObject.currentObject.transform.position.x, aObject.currentObject.transform.position.y) + new Vector2(aObject.currentObject.transform.up.x, aObject.currentObject.transform.up.y);
            Vector2 originVert = originPosPoint - new Vector2(aObject.currentObject.transform.position.x, aObject.currentObject.transform.position.y);
            originVert.Normalize();

            float angleNorm = Vector2.SignedAngle(originVert, dirToPoint);

            float someDoter = Dot(originVert.x, originVert.y, dirToPoint.x, dirToPoint.y);

            Vector3 eulerAngles = aObject.currentObject.transform.rotation.eulerAngles;

            if (someDoter < 0)
            {
                //UnityEngine.Debug.Log("test0");
                Quaternion tester = Quaternion.Euler(eulerAngles.x, eulerAngles.y, eulerAngles.z - angleNorm);
                aObject.currentObject.transform.rotation = tester;// Quaternion.Slerp(b.shape.gameObject.transform.rotation, tester, 1);
            }
            else if (someDoter > 0)
            {
                //UnityEngine.Debug.Log("test1");
                Quaternion tester = Quaternion.Euler(eulerAngles.x, eulerAngles.y, eulerAngles.z + angleNorm);
                aObject.currentObject.transform.rotation = tester;// Quaternion.Slerp(b.shape.gameObject.transform.rotation, tester, 1);
            }*/










            return 1;
        }












        /*public void applyForce(Vec2 f, Vec2 worldSpacePosition)
        {
            // linear
            this.force.add(f);

            // angular
            vTmp.set(worldSpacePosition);
            vTmp.sub(position);
            applyTorque(vTmp.cross(f));
        }*/
        /*public Vector2 cross(Vector2 v, float a)
        {
            Vector2 outVec;
            outVec.x = v.y * a;
            outVec.y = v.x * -a;
            return outVec;
        }*/









        public float cross(Vector2 a, Vector2 b)
        {
            return a.x * b.y - a.y * b.x;
        }


        public Vector2 crossScale(float a, Vector2 v)
        {
            Vector2 temp;
            temp.x = -a * v.y;
            temp.y = a * v.x;
            return temp;
        }

        public int resolveCollision(SC_RigidInfo aObject, SC_RigidInfo bObject, out float? aVeloX, out float? aVeloY, float theTime)
        {
            aVeloX = null;
            aVeloY = null;

            Vector2 relativeVel = aObject.velocity - bObject.velocity;
            relativeVel.Normalize();


            /*float someDot = Dot(relativeVel.x, relativeVel.y, aObject.collideNorm.Value.x, aObject.collideNorm.Value.y);

            if (someDot > 0)
            {
                //UnityEngine.Debug.Log("velocities are separating?");
                return 0;
            }*/

            var momentumObjectA = aObject.invMass * aObject.velocity;
            var momentumObjectB = bObject.invMass * bObject.velocity;
            var initialVelo = momentumObjectA + momentumObjectB;
            var finalVelo = initialVelo / (aObject.invMass + bObject.invMass);
            var changeInMomentumA = (aObject.invMass * finalVelo) - (aObject.invMass * initialVelo); // as per Wiki
            var netForceA = (changeInMomentumA / theTime);
            var impulseA = netForceA * theTime;
          
            float someMag = finalVelo.magnitude;
            impulseA += aObject.collideNorm.Value.normalized * someMag;
            impulseA *= aObject.restitution;
            aVeloX = impulseA.x;
            aVeloY = impulseA.y;




            //aObject.angularVelocity += aObject.invInertia * cross(aObject.collideNorm.Value, impulseA);

            var vTmp = aObject.currentObject.transform.position;
            aObject.angularVelocity += (cross(vTmp,impulseA) / aObject.inertia);
            aObject.angle += aObject.angularVelocity;
            aObject.angularVelocity += aObject.angularAcceleration;
            aObject.angularAcceleration = aObject.torque / aObject.inertia;





            //UnityEngine.Debug.Log(aObject.angle);



            return 1;










            //var changeInVelocity = finalVelo - initialVelo;
            //var netForceA = aObject.invMass * (changeInVelocity / theTime);
            //var netForceA = (aObject.invMass * changeInVelocity) / theTime;
            //var Impulse = netForceA * theTime;
            //var netForceA = (changeInVelocity / theTime);
            //UnityEngine.Debug.Log(Impulse);
            //netForceA *= -1;
            //aVeloX = netForceA.x;
            //aVeloY = netForceA.y;
            //UnityEngine.Debug.Log(netForceA);

            //newton second law 
            //Fnet = mass * acceleration
            //a = change in velocity / change in time
            //Velocity final = velocity initial * change in time
            //------------------------------------------------------Vf-Vi = change in time;
            //-----------------------------------------------------(VF-VI)/change intime = acceleration
            //F = mass * (change in velo / change in time)
            //Impulse = Force * change in time
            //Impulse = change in momentum
            //F = change in momentum / change in time

            /*aVeloX = null;
            aVeloY = null;

            // vap1
            Vector2 vap1 = new Vector2();
            vap1 = aObject.velocity;

            Vector2 r1 = new Vector2();
            r1 = aObject.collidePoint.Value;
            r1 -= new Vector2(aObject.currentObject.transform.position.x, aObject.currentObject.transform.position.y);

            Vector2 val1 = new Vector2();
            val1 = crossScale(aObject.angularVelocity, r1);
            vap1 += val1;

            // vap2
            Vector2 vap2 = new Vector2();
            vap2 = bObject.velocity;

            Vector2 r2 = new Vector2();
            r2 = aObject.collidePoint.Value;
            r2 -= new Vector2(bObject.currentObject.transform.position.x, bObject.currentObject.transform.position.y);

            Vector2 val2 = new Vector2();
            val2 = crossScale(bObject.angularVelocity, r2);
            vap2 += val2;

            // relative velocity
            Vector2 vrel = new Vector2();
            vrel = vap1;
            vrel -= vap2;

            /*if (Dot(vrel.x, vrel.y, aObject.collideNorm.Value.x, aObject.collideNorm.Value.y) < 0) //vrel.dot(normal)
            {
                return;
            }

            float e = 0.5f; // restitution hard coded

            float raxn = cpvcross(r1, aObject.collideNorm.Value);
            float rbxn = cpvcross(r2, -aObject.collideNorm.Value);

            float totalMass = (1 / aObject.mass) + (1 / bObject.mass) + ((raxn * raxn) / aObject.inertia) + ((rbxn * rbxn) / bObject.inertia);

            float j = (float)((-(1 + e) * Dot(vrel.x, vrel.y, aObject.collideNorm.Value.x, aObject.collideNorm.Value.y)) / totalMass);

            Vector2 impulse = new Vector2();
            impulse = aObject.collideNorm.Value;
            impulse *= (j);


            var curentVel = aObject.velocity;

            //impulse *= (-1);
            Vector2 vTmp = (impulse);
            vTmp *= (1 / aObject.mass);
            aObject.velocity += vTmp;

            aVeloX = aObject.velocity.x;
            aVeloY = aObject.velocity.y;*/
            //UnityEngine.Debug.Log(aVeloX + " __ " + aVeloY);

            //Vector2 vTmp = (impulse);
            //vTmp *= (1 / aObject.mass);
            //aObject.velocity = new Vector2(0, 0);
            //aObject.velocity += vTmp;

            //aVeloX = aObject.velocity.x;
            //aVeloY = aObject.velocity.y;
            //UnityEngine.Debug.Log(aVeloX + " __ " + aVeloY);


            //impulse *= (-1);

            //vTmp = (impulse);
            //vTmp *= (1 / aObject.mass);
            //aObject.velocity += vTmp;
            //vTmp = aObject.currentObject.transform.position;
            //var crossProd = cpvcross(vTmp, impulse);// vTmp.cross(impulse)
            //aObject.angularVelocity += (crossProd / aObject.inertia);

            //aObject.velocity = adds(aObject.velocity, impulse, aObject.invMass);
            //aVeloX = aObject.velocity.x;
            //aVeloY = aObject.velocity.y;
            //UnityEngine.Debug.Log(aVeloX + " __ " + aVeloY);



            //impulse *= (-1);
            //applyImpulse(impulse, bObject.collidePoint.Value, bObject);

            //var newVel = aObject.velocity;

            //float velAlongNormalA = Dot(newVel.x, newVel.y, curentVel.x, curentVel.y);
            //aVeloX = aObject.velocity.x;
            //aVeloY = aObject.velocity.y;

            /*if (velAlongNormalA >= 0)
            {
                UnityEngine.Debug.Log("Pushing inside of other Object");

                if (aVeloX > 0 && curentVel.x > 0 ||
                    aVeloX <= 0 && curentVel.x <= 0)
                {
                    aVeloX *= -1;
                }

                if (aVeloY > 0 && curentVel.y > 0 ||
                    aVeloY <= 0 && curentVel.y <= 0)
                {
                    aVeloY *= -1;
                }

                /*aObject.lastImpulse = new Vector2(aVeloX.Value, aVeloY.Value);

                if (aObject.lastImpulse.HasValue)
                {
                    var diff = aObject.lastImpulse.Value - someNewVelo;

                    if (diff.magnitude <= aObject.restitution * 0.001f)
                    {
                        UnityEngine.Debug.Log("too low impulse");
                        //aObject.isStaticTemp = 1;
                        return 2;
                    }
                }
                return 0;*
            }*/
        }

        public void applyImpulse(Vector2 impulse, Vector2 worldSpacePosition, SC_RigidInfo rigidBod)
        {
            // linear
            vTmp = (impulse);
            vTmp *= (1 / rigidBod.mass);
            rigidBod.velocity += vTmp;
            //vTmp = rigidBod.currentObject.transform.position;
            //var crossProd = cpvcross(vTmp, impulse);// vTmp.cross(impulse)
            //rigidBod.angularVelocity += (crossProd / rigidBod.inertia);
            // angular
            //vTmp.set(worldSpacePosition);
            //vTmp.sub(position);
            //angularVelocity += (vTmp.cross(impulse) / inertia);


            //velocity.addsi(impulse, invMass);
            //angularVelocity += invInertia * Vec2.cross(contactVector, impulse);
        }


        /*public Vector2 adds(Vector2 vel, Vector2 impulse, float s)
        {
            Vector2 outVec;
            outVec.x = vel.x + impulse.x * s;
            outVec.y = vel.y + impulse.y * s;
            return outVec;
        }


        public static Matrix4x4 Translate(Matrix4x4 matr,Vector3 aPosition)
        {
            //var m = Matrix4x4.identity; // 1   0   0   x
            matr.m03 = aPosition.x;        // 0   1   0   y
            matr.m13 = aPosition.y;        // 0   0   1   z
            matr.m23 = aPosition.z;        // 0   0   0   1
            return matr;
        }


        public static Matrix4x4 Translator(Vector3 aPosition)
        {
            var m = Matrix4x4.identity; // 1   0   0   x
            m.m03 = aPosition.x;        // 0   1   0   y
            m.m13 = aPosition.y;        // 0   0   1   z
            m.m23 = aPosition.z;        // 0   0   0   1
            return m;
        }*/







        /*//https://stackoverflow.com/questions/47785716/rotate-gameobject-around-pivot-point-over-time
        Vector3 RotatePointAroundPivot(Vector3 point, Vector3 pivot, Vector3 angles)
        {
            Vector3 dir = point - pivot; // get point direction relative to pivot
            dir = Quaternion.Euler(angles) * dir; // rotate it
            point = dir + pivot; // calculate rotated point

            return point;
        }*/



        /*public Matrix4x4 convertMatrix(Matrix matrixToConvert)
        {
            Matrix4x4 newMatrix;
            newMatrix.m00 = matrixToConvert


            return newMatrix;
        }*/






        private class SupportingPointResult
        {
            public Vector2? supportingPoint;
            public Vector2? normal;
            public double minDistance;

            public void set(Vector2? supportingPoint, Vector2? normal, double minDistance)
            {
                this.supportingPoint = supportingPoint;
                this.normal = normal;
                this.minDistance = minDistance;
            }
        }

        /*//private static SupportingPointResult SUP_POINT_RESULT_TMP = new SupportingPointResult();
        private static Vector2 V_TMP = new Vector2();
        private SupportingPointResult getClosestSupportingPoint(SC_RigidInfo boxA, SC_RigidInfo boxB) 
        {
            SupportingPointResult SUP_POINT_RESULT_TMP = new SupportingPointResult();

            SupportingPointResult supportingPointResult = new SupportingPointResult();
            supportingPointResult.set(null, null, -Mathf.NegativeInfinity); //-Mathf.NegativeInfinity

            for (int e = 0; e < boxA.edgeSegments.Length; e++)
            {
                SUP_POINT_RESULT_TMP.set(null, null, 0);
                Vector2 normal = boxA.edgeSegments[e].Normal;
                bool allPositive = true;

                for (int v = 0; v < boxB.vertices.Length; v++)
                {
                    V_TMP = boxB.vertices[v];
                    V_TMP = V_TMP - boxA.edgeSegments[e].Start;
                    float distance = Dot(normal.x, normal.y, V_TMP.x, V_TMP.y); //Vector2.Distance(normal, V_TMP);// 
                    if (distance < SUP_POINT_RESULT_TMP.minDistance)
                    {
                        UnityEngine.Debug.Log(distance);
                        allPositive = false;
                        SUP_POINT_RESULT_TMP.set(boxB.vertices[v], normal, distance);
                    }
                }

                if (allPositive)
                {
                    //UnityEngine.Debug.Log("0000");
                    supportingPointResult.set(null, null, 0);
                    return supportingPointResult;
                }

                if (SUP_POINT_RESULT_TMP.minDistance > supportingPointResult.minDistance)
                {
                    UnityEngine.Debug.Log("tester");
                    supportingPointResult.set(SUP_POINT_RESULT_TMP.supportingPoint, SUP_POINT_RESULT_TMP.normal, SUP_POINT_RESULT_TMP.minDistance);
                }
            }

            return supportingPointResult;










            /*foreach (Segment edge in boxA.edgeSegments)
            {
                SUP_POINT_RESULT_TMP.set(null, null, 0);
                Vector2 normal = edge.Normal;
                bool allPositive = true;
                foreach (Vector2 v in boxB.vertices)
                {
                    V_TMP = v;
                    V_TMP = V_TMP - edge.Start;

                    float distance = Dot(normal.x, normal.y, V_TMP.x, V_TMP.y);

                    if (distance < SUP_POINT_RESULT_TMP.minDistance)
                    {
                        allPositive = false;
                        SUP_POINT_RESULT_TMP.set(v, normal, distance);
                    }

                    if (allPositive)
                    {
                        supportingPointResult.set(null, null, 0);
                        return supportingPointResult;
                    }
                    else if (SUP_POINT_RESULT_TMP.minDistance > supportingPointResult.minDistance)
                    {
                        supportingPointResult.set(SUP_POINT_RESULT_TMP.supportingPoint, SUP_POINT_RESULT_TMP.normal, SUP_POINT_RESULT_TMP.minDistance);
                    }
                }
            }

            return supportingPointResult;

            /*for (Box.Edge edge : boxA.edges)
            {
                SUP_POINT_RESULT_TMP.set(null, null, 0);
                Vec2 normal = edge.normal;
                boolean allPositive = true;
                for (Vec2 v : boxB.vertices)
                {
                    V_TMP.set(v);
                    V_TMP.sub(edge.a.x, edge.a.y);
                    double distance = normal.dot(V_TMP);
                    if (distance < SUP_POINT_RESULT_TMP.minDistance)
                    {
                        allPositive = false;
                        SUP_POINT_RESULT_TMP.set(v, normal, distance);
                    }
                }
                // not collides - all vertices dot normal are positive
                if (allPositive)
                {
                    supportingPointResult.set(null, null, 0);
                    return;
                }
                else if (SUP_POINT_RESULT_TMP.minDistance > supportingPointResult.minDistance)
                {
                    supportingPointResult.set(SUP_POINT_RESULT_TMP.supportingPoint
                            , SUP_POINT_RESULT_TMP.normal, SUP_POINT_RESULT_TMP.minDistance);
                }
            }
        }*/
        int resolveAngularVelocity(SC_RigidInfo aObject, SC_RigidInfo bObject, float theTime)
        {
            //Vector2 acceleration = (aObject.lastFrameVelo - aObject.velocity) * theTime;
            //var TranslationalInertia = aObject.mass * acceleration;
            //inertiaVisual.transform.position = TranslationalInertia;


            //float smallr = Vector2.Distance(aObject.currentObject.transform.position, aObject.collidePoint.Value);
            //Vector2 F = smallr * aObject.mass * aObject.velocity;
            //var torque  = F * smallr;
            //UnityEngine.Debug.Log("torque: " + torque);


            //Quaternion rotation = Quaternion.LookRotation(aObject.currentObject.transform.forward, aObject.currentObject.transform.forward + new Vector3(0,0,2));
            //aObject.currentObject.transform.rotation = rotation;



            //float smallr = Vector2.Distance(aObject.currentObject.transform.position, aObject.collidePoint.Value);
            //Vector2 w = (aObject.mass * aObject.velocity) / ((1/3) * bObject.mass * smallr);
            //float smallr = Vector2.Distance(aObject.currentObject.transform.position, aObject.collidePoint.Value);
            //Vector2 F = smallr * aObject.mass * aObject.velocity;
            //Vector3 someF = new Vector3(F.x, F.y, 0);

            //float mag = someF.magnitude;
            //UnityEngine.Debug.Log("mag: " + mag);
            //Vector3 rotTowards = Vector3.RotateTowards(aObject.currentObject.transform.forward, someF, theTime, 0.0f);
            //Quaternion rotation = Quaternion.LookRotation(rotTowards);
            //aObject.currentObject.transform.rotation = rotation;



            //Quaternion rotation = Quaternion.LookRotation(someF, Vector3.forward);
            //aObject.currentObject.transform.rotation = rotation;

            //Vector2 angularVelo = aObject.mass * aObject.velocity * 
            //Vector2 acceleration = (aObject.lastVelocity - aObject.velocity)/ theTime;
            //Vector2 F = r * m * v;
            //float smallr = Vector2.Distance(aObject.currentObject.transform.position, aObject.collidePoint.Value);
            //Vector2 F = smallr * aObject.mass * aObject.velocity;
            //Vector3 someF = new Vector3(F.x, F.y, 0);
            //UnityEngine.Debug.Log("F: " + F);

            //aObject.currentObject.transform.eulerAngles = new Vector3(F.x,F.y,0);

            //aObject.currentObject.transform.Rotate(F, Space.Self);
            //aObject.currentObject.transform.RotateAround(aObject.currentObject.transform.position,F,);
            //Quaternion.Slerp(,,theTime);
            //aObject.currentObject.transform.rotation = Quaternion.FromToRotation(aObject.currentObject.transform.position, someF);

            //Vector3 rotEulerAngles = aObject.currentObject.transform.rotation.eulerAngles;
            //Vector3 rotTowards = Vector3.RotateTowards(rotEulerAngles, someF, theTime, 0.0f);
            //aObject.currentObject.transform.rotation.SetEulerAngles(rotTowards.x, rotTowards.y, rotTowards.z);
            //aObject.currentObject.transform.rotation = Quaternion.Euler(rotTowards.x, rotTowards.y, rotTowards.z);

            //aObject.currentObject.transform.rotation = Quaternion.Euler(someF.x, someF.y, someF.z);



            return 1;
        }





        /*private Matrix RotateAroundPoint(float angle, Vector2 center)
        {
            // Translate the point to the origin.
            Matrix result = new Matrix();
            result.RotateAt(angle, center.x, center.y);
            return result;
        }*/



        int killTask = 0;
        Task tsk;

        int someFrameCounter = 0;


        Vector2? intersectorPoint;
        //float theGoalDist = 0;

        public async Task DoWork(int timeOut)
        {
            float theTime;

        _threadLoop:

            theTime = Time.deltaTime;

            /*if (mainWatchSwitch == 0)
            {
                mainWatch.Stop();
                mainWatch.Reset();
                mainWatch.Start();
                mainWatchSwitch = 1;
            }

            if (mainWatch.Elapsed.TotalSeconds >= 1)
            {
                UnityEngine.Debug.Log(mainFrameCounter);
                mainFrameCounter = 0;
                mainWatch.Stop();
                mainWatch.Reset();
                mainWatch.Start();
            }*/

            for (int i = 0; i < arrayOfGameObjects.Length; i++)
            {
                if (arrayOfGameObjects[i].isStatic == 0 && arrayOfGameObjects[i].isStaticTemp == 0) // && arrayOfGameObjects[i].isStaticTemp == 0
                {
                    Vector2 somePos = arrayOfGameObjects[i].position;
                    Vector2 someVelo = new Vector3(arrayOfGameObjects[i].velocity.x, arrayOfGameObjects[i].velocity.y);

                    SC_RigidInfo dummyObject = new SC_RigidInfo();
                    dummyObject.position = arrayOfGameObjects[i].position;
                    dummyObject.velocity = someVelo;
                    UpdateObject(dummyObject.position, dummyObject.velocity, gravity, theTime, dummyObject);
                    Vector2 someOtherVelo = dummyObject.velocity;

                    arrayOfGameObjects[i].position = dummyObject.position;
                    arrayOfGameObjects[i].velocity = dummyObject.velocity;


                    //float Angle = arrayOfGameObjects[i].currentObject.transform.rotation.eulerAngles.z;
                    //float currentAngle = arrayOfGameObjects[i].angle * (float)(180.0 / Mathf.PI);
                    //Quaternion someAddedRot = Quaternion.AngleAxis(currentAngle, Vector3.forward);
                    //arrayOfGameObjects[i].currentObject.transform.rotation = Quaternion.Slerp(arrayOfGameObjects[i].currentObject.transform.rotation, someAddedRot, theTime*10000);



                    //arrayOfGameObjects[i].angle += arrayOfGameObjects[i].angularVelocity;
                    //arrayOfGameObjects[i].angularVelocity += arrayOfGameObjects[i].angularAcceleration;
                    //arrayOfGameObjects[i].angularAcceleration = arrayOfGameObjects[i].torque / arrayOfGameObjects[i].inertia;


                    arrayOfGameObjects[i].position = dummyObject.position;
                    arrayOfGameObjects[i].velocity = dummyObject.velocity;              

                    /*Vector2 poser = arrayOfGameObjects[i].currentObject.transform.position;
                    var dir = poser;
                    dir.y = arrayOfGameObjects[i].angularVel.y;
                    dir.Normalize();

                    UnityEngine.Debug.DrawRay(poser, dir, UnityEngine.Color.green);

                    dir = poser;
                    dir.x = arrayOfGameObjects[i].angularVel.x;
                    dir.Normalize();

                    UnityEngine.Debug.DrawRay(poser, dir, UnityEngine.Color.red);


                    if (arrayOfGameObjects[i].collidePoint.HasValue)
                    {
                        float Angle = arrayOfGameObjects[i].currentObject.transform.rotation.eulerAngles.z;

                        poser = arrayOfGameObjects[i].currentObject.transform.position;
                        var tangent = arrayOfGameObjects[i].collidePoint.Value - poser;
                        var someR = new Vector2(-tangent.y, tangent.x);
                        var dist = Vector2.Distance(arrayOfGameObjects[i].currentObject.transform.position, arrayOfGameObjects[i].collidePoint.Value);

                        float someAngle = Vector2.Angle(tangent, dir);
                        someAngle *= -1;

                        Vector3 someCurrentPivot = new Vector3(arrayOfGameObjects[i].collidePoint.Value.x, arrayOfGameObjects[i].collidePoint.Value.y, 0); //arrayOfGameObjects[i].currentObject.transform.position - 
                        Quaternion someAddedRot = Quaternion.AngleAxis(someAngle, Vector3.forward);

                        //someCurrentPivot.y = arrayOfGameObjects[i].currentObject.transform.up.y;
                        //Quaternion rotatePivot = Quaternion.FromToRotation(arrayOfGameObjects[i].angularVel, someCurrentPivot);

                        //Quaternion rotation = Quaternion.LookRotation(someCurrentPivot, Vector3.right);

                        Vector3 veloc = new Vector3(arrayOfGameObjects[i].angularVel.x, 0, 0);
                        Vector2 colPointer = new Vector2(arrayOfGameObjects[i].collidePoint.Value.x, arrayOfGameObjects[i].collidePoint.Value.y);
                        Quaternion rot = new Quaternion();


                        //UnityEngine.Debug.Log(arrayOfGameObjects[i].angularVel);

                        //arrayOfGameObjects[i].angularVel.normalized
                        Vector2 somePoser = arrayOfGameObjects[i].currentObject.transform.position;
                        Vector2 upper = new Vector2(arrayOfGameObjects[i].currentObject.transform.up.x, arrayOfGameObjects[i].currentObject.transform.up.y);
                        //upper.x = arrayOfGameObjects[i].angularVel.x;

                        upper.x += arrayOfGameObjects[i].angularVel.x;

                        rot.SetLookRotation(arrayOfGameObjects[i].currentObject.transform.forward, (upper).normalized);//(arrayOfGameObjects[i].currentObject.transform.position - colPointer).normalized);// ;

                        //(somePoser - colPointer + (arrayOfGameObjects[i].angularVel)
                        //(somePoser - colPointer + (arrayOfGameObjects[i].angularVel)

                        //Quaternion rotter = arrayOfGameObjects[i].currentObject.transform.rotation;
                        arrayOfGameObjects[i].currentObject.transform.rotation = Quaternion.Slerp(arrayOfGameObjects[i].currentObject.transform.rotation, rot, theTime);

                        //arrayOfGameObjects[i].currentObject.transform.rotation = rot;
                        //Quaternion.Lerp(rot, someAddedRot, theTime);








                        //var colPointer = new Vector3(arrayOfGameObjects[i].collidePoint.Value.x, arrayOfGameObjects[i].collidePoint.Value.y, 0);
                        //arrayOfGameObjects[i].currentObject.transform.RotateAround(arrayOfGameObjects[i].currentObject.transform.position - colPointer, Vector3.forward, theTime);


                        /*Quaternion wantedRot = Quaternion.Slerp(arrayOfGameObjects[i].currentObject.transform.rotation, someAddedRot, theTime);

                        Matrix4x4 someMatrix = Matrix4x4.TRS(arrayOfGameObjects[i].currentObject.transform.position, wantedRot, Vector3.one);


                        var sometester = new Vector3(arrayOfGameObjects[i].collidePoint.Value.x, arrayOfGameObjects[i].collidePoint.Value.y, 0);

                        someMatrix = Translate(someMatrix, sometester);

                        arrayOfGameObjects[i].currentObject.transform.rotation = someMatrix.rotation;
                        */

                        //var sometester = new Vector3(arrayOfGameObjects[i].collidePoint.Value.x, arrayOfGameObjects[i].collidePoint.Value.y, 0);

                        //Matrix4x4 someMatrix = Matrix4x4.Translate(sometester);








                        //arrayOfGameObjects[i].currentObject.transform.rotation
                        //Matrix4x4 someMatrix = new Matrix4x4();
                        //someMatrix = Matrix4x4.Translate(someCurrentPivot);

                        //someMatrix.rotation = Matrix4x4.Rotate(wantedRot);
                        //someMatrix = Matrix4x4.Rotate(wantedRot);
                        //Matrix4x4 someMatrix =  new Matrix4x4();
                        //someMatrix = Translator(someCurrentPivot);
                        //someMatrix = Matrix4x4.Rotate(wantedRot);
                        //someMatrix.MultiplyPoint3x4(someCurrentPivot);
                        //arrayOfGameObjects[i].currentObject.transform.rotation = someMatrix.rotation;

                        //someMatrix = Matrix4x4.Rotate(wantedRot);
                        //someMatrix = Translate(someMatrix, someCurrentPivot);

                        //someMatrix.MultiplyPoint3x4(someCurrentPivot);
                        //someMatrix.MultiplyVector(someCurrentPivot);





                        //Vector3 someCurrentPos = arrayOfGameObjects[i].currentObject.transform.position;                  
                        //someCurrentPos += (arrayOfGameObjects[i].currentObject.transform.rotation * someCurrentPivot);








                        //arrayOfGameObjects[i].currentObject.transform.RotateAround(arrayOfGameObjects[i].collidePoint.Value, arrayOfGameObjects[i].angularVel, someAngle);

                        /*int areIntersecting = isIntersecting(out obj11, out obj22, 0, arrayOfGameObjects[i], out intersectorPoint, theTime); //out obj11, out obj22, 

                        if (areIntersecting == 0)
                        {

                        }
                        else
                        {

                        }
                    }*/


                    int? someResulter = isColliding(out obj1, out obj2, 0, arrayOfGameObjects[i], theTime);

                    //UnityEngine.Debug.Log(someResulter.HasValue);

                    if (someResulter.HasValue)
                    {
                        //resolveAngularVelocity(arrayOfGameObjects[i], arrayOfGameObjects[arrayOfGameObjects[i].indexToCollideWith],theTime);
                        if (Mathf.Abs(Mathf.Abs(arrayOfGameObjects[i].lerpPos.Value.x) - Mathf.Abs(arrayOfGameObjects[i].currentObject.transform.position.x)) <
                            Mathf.Abs(Mathf.Abs(arrayOfGameObjects[i].lerpPos.Value.y) - Mathf.Abs(arrayOfGameObjects[i].currentObject.transform.position.y)))
                        {
                            theGoalDist = arrayOfGameObjects[i].lerpPos.Value.y;
                        }
                        else
                        {
                            theGoalDist = arrayOfGameObjects[i].lerpPos.Value.x;
                        }

                        bool timeToHit = ProjectileHelper.ComputeTimeToHitGround(arrayOfGameObjects[i].currentObject.transform.position, dummyObject.velocity, theGoalDist, gravity, out arrayOfGameObjects[i].timeToHit);
                        Vector3 positionGoal = ProjectileHelper.ComputePositionAtTimeAhead(arrayOfGameObjects[i].currentObject.transform.position, dummyObject.velocity, (float)gravity, arrayOfGameObjects[i].timeToHit);
                        Vector3 velocityGoal = ProjectileHelper.ComputeVelocityAtTimeAhead(arrayOfGameObjects[i].currentObject.transform.position, dummyObject.velocity, (float)gravity, arrayOfGameObjects[i].timeToHit);
                        Vector2 posGoal = new Vector2(positionGoal.x, positionGoal.y);
                        Vector2 veloGoal = new Vector2(velocityGoal.x, velocityGoal.y);

                        Vector2 someCurrentDir0 = arrayOfGameObjects[i].lerpPos.Value - new Vector2(arrayOfGameObjects[i].currentObject.transform.position.x, arrayOfGameObjects[i].currentObject.transform.position.y); //somePos
                        someCurrentDir0.Normalize();

                        someOtherVelo = dummyObject.velocity;
                        Vector2 someNewFramePos = Vector2.SmoothDamp(arrayOfGameObjects[i].currentObject.transform.position, arrayOfGameObjects[i].lerpPos.Value, ref someOtherVelo, arrayOfGameObjects[i].timeToHit);

                        Vector2 someDirOther = arrayOfGameObjects[i].lerpPos.Value - someNewFramePos;
                        someDirOther.Normalize();
                        float someDotOther = Dot(someDirOther.x, someDirOther.y, someCurrentDir0.x, someCurrentDir0.y);

                        //Vector2 someDir = arrayOfGameObjects[i].lerpPos.Value - dummyObject.position;
                        //someDir.Normalize();
                        //float someDot0 = Dot(someDir.x, someDir.y, someCurrentDir0.x, someCurrentDir0.y);

                        if (someDotOther <= 0)
                        {
                            //Instantiate(pointer1, arrayOfGameObjects[i].lerpPos.Value,Quaternion.identity);
                            arrayOfGameObjects[i].position = dummyObject.position;
                            arrayOfGameObjects[i].velocity = dummyObject.velocity;

                            int areIntersecting = isIntersecting(out obj11, out obj22, 0, arrayOfGameObjects[i], out intersectorPoint, theTime); //out obj11, out obj22, 

                            arrayOfGameObjects[i].position = arrayOfGameObjects[i].currentObject.transform.position;
                            arrayOfGameObjects[i].velocity = someVelo;

                            if (areIntersecting == 0)
                            {








                                someOtherVelo = dummyObject.velocity;
                                Vector2 someNewFramePos1 = Vector2.SmoothDamp(arrayOfGameObjects[i].currentObject.transform.position, arrayOfGameObjects[i].lerpPos.Value, ref someOtherVelo, arrayOfGameObjects[i].timeToHit);
                                someOtherVelo = dummyObject.velocity;
                                Vector2 someNewFramePos2 = Vector2.SmoothDamp(arrayOfGameObjects[i].currentObject.transform.position, posGoal, ref someOtherVelo, arrayOfGameObjects[i].timeToHit);

                                Vector2 someDir1 = (arrayOfGameObjects[i].lerpPos.Value - someNewFramePos1);
                                Vector2 someDir2 = (posGoal - someNewFramePos2);

                                float someDot1 = Dot(someDir1.x, someDir1.y, someCurrentDir0.x, someCurrentDir0.y);
                                float someDot2 = Dot(someDir2.x, someDir2.y, someCurrentDir0.x, someCurrentDir0.y);

                                Vector2 someDir3 = (arrayOfGameObjects[i].lerpPos.Value - somePos);
                                float someDot3 = Dot(someDir3.x, someDir3.y, someCurrentDir0.x, someCurrentDir0.y);

                                if (someDot1 <= 0 || someDot2 <= 0 || someDot3 <= 0)
                                {
                                    someOtherVelo = dummyObject.velocity;

                                    bool timeToHiter = ProjectileHelper.ComputeTimeToHitGround(arrayOfGameObjects[i].currentObject.transform.position, someOtherVelo, theGoalDist, gravity, out arrayOfGameObjects[i].timeToHit);
                                    Vector2 someNewFramePoser = Vector2.SmoothDamp(arrayOfGameObjects[i].currentObject.transform.position, arrayOfGameObjects[i].lerpPos.Value, ref someOtherVelo, arrayOfGameObjects[i].timeToHit);

                                    Vector2 lastFrameDir = arrayOfGameObjects[i].lerpPos.Value - someNewFramePoser;

                                    float lastDot = Dot(lastFrameDir.x, lastFrameDir.y, someCurrentDir0.x, someCurrentDir0.y);

                                    if (lastDot <= 0)
                                    {
                                        float? aVeloX;
                                        float? aVeloY;



                                        /*resolveCollision(arrayOfGameObjects[i], arrayOfGameObjects[arrayOfGameObjects[i].indexToCollideWith], out aVeloX, out aVeloY,theTime);

                                        if (aVeloX.HasValue && !double.IsNaN(aVeloX.Value) && aVeloY.HasValue && !double.IsNaN(aVeloY.Value)) //&& !double.IsNaN(newVelo.Value.y)
                                        {
                                            var someNewVelo = new Vector2(aVeloX.Value, aVeloY.Value);

                                            arrayOfGameObjects[i].velocity = someNewVelo;
                                            arrayOfGameObjects[i].lerpPos = null;
                                            arrayOfGameObjects[i].collideNorm = null;
                                            arrayOfGameObjects[i].lerp = 0;

                                            //UnityEngine.Debug.Log(arrayOfGameObjects[i].orientation);
                                        }*/




                                        int someResult = ResolveCollision(arrayOfGameObjects[i], arrayOfGameObjects[arrayOfGameObjects[i].indexToCollideWith], out aVeloX, out aVeloY, theTime);
                                        //UnityEngine.Debug.Log(arrayOfGameObjects[arrayOfGameObjects[i].indexToCollideWith].currentObject.name);
                                        if (aVeloX.HasValue && !double.IsNaN(aVeloX.Value) && aVeloY.HasValue && !double.IsNaN(aVeloY.Value)) //&& !double.IsNaN(newVelo.Value.y)
                                        {
                                            var someNewVelo = new Vector2(aVeloX.Value, aVeloY.Value);

                                            arrayOfGameObjects[i].velocity = someNewVelo;
                                            arrayOfGameObjects[i].lerpPos = null;
                                            arrayOfGameObjects[i].collideNorm = null;
                                            arrayOfGameObjects[i].lerp = 0;

                                            //UnityEngine.Debug.Log(arrayOfGameObjects[i].orientation);
                                        }
                                    }
                                    else
                                    {
                                        arrayOfGameObjects[i].currentObject.transform.position = someNewFramePoser;
                                        arrayOfGameObjects[i].position = someNewFramePoser;
                                        arrayOfGameObjects[i].velocity = dummyObject.velocity;
                                        //arrayOfGameObjects[i].lerpPos = null;
                                        //arrayOfGameObjects[i].collideNorm = null;
                                        //arrayOfGameObjects[i].lerp = 0;
                                    }
                                }
                                else
                                {
                                    if (someDot1 < lowExtent && someDot2 > lowExtent)
                                    {
                                        //UnityEngine.Debug.Log("TESTER10");
                                        arrayOfGameObjects[i].currentObject.transform.position = someNewFramePos2;
                                        arrayOfGameObjects[i].position = someNewFramePos2;
                                        arrayOfGameObjects[i].velocity = dummyObject.velocity;
                                        //arrayOfGameObjects[i].lerpPos = null;
                                        //arrayOfGameObjects[i].collideNorm = null;
                                        //arrayOfGameObjects[i].lerp = 0;
                                    }
                                    else if (someDot1 > lowExtent && someDot2 < lowExtent)
                                    {
                                        //UnityEngine.Debug.Log("TESTER20");
                                        arrayOfGameObjects[i].currentObject.transform.position = someNewFramePos1;
                                        arrayOfGameObjects[i].position = someNewFramePos1;
                                        arrayOfGameObjects[i].velocity = dummyObject.velocity;

                                        //arrayOfGameObjects[i].lerpPos = null;
                                        //arrayOfGameObjects[i].collideNorm = null;
                                        //arrayOfGameObjects[i].lerp = 0;
                                    }
                                    else if (someDot1 > lowExtent && someDot2 > lowExtent)
                                    {
                                        //UnityEngine.Debug.Log("TESTER30");
                                        arrayOfGameObjects[i].currentObject.transform.position = someNewFramePos2; //someNewFramePos2
                                        arrayOfGameObjects[i].position = someNewFramePos2;//someNewFramePos2
                                        arrayOfGameObjects[i].velocity = dummyObject.velocity;
                                        //arrayOfGameObjects[i].lerpPos = null;
                                        //arrayOfGameObjects[i].collideNorm = null;
                                        //arrayOfGameObjects[i].lerp = 0;
                                    }
                                    else
                                    {
                                        //UnityEngine.Debug.Log("TESTER40");
                                        someOtherVelo = dummyObject.velocity;

                                        bool timeToHiter = ProjectileHelper.ComputeTimeToHitGround(arrayOfGameObjects[i].currentObject.transform.position, someOtherVelo, theGoalDist, gravity, out arrayOfGameObjects[i].timeToHit);
                                        Vector2 someNewFramePoser = Vector2.SmoothDamp(arrayOfGameObjects[i].currentObject.transform.position, arrayOfGameObjects[i].lerpPos.Value, ref someOtherVelo, arrayOfGameObjects[i].timeToHit);

                                        Vector2 lastFrameDir = arrayOfGameObjects[i].lerpPos.Value - someNewFramePoser;

                                        float lastDot = Dot(lastFrameDir.X, lastFrameDir.Y, someCurrentDir0.X, someCurrentDir0.Y);

                                        if (lastDot <= 0)
                                        {
                                            float? aVeloX;
                                            float? aVeloY;
                                            /*resolveCollision(arrayOfGameObjects[i], arrayOfGameObjects[arrayOfGameObjects[i].indexToCollideWith], out aVeloX, out aVeloY,theTime);

                                            if (aVeloX.HasValue && !double.IsNaN(aVeloX.Value) && aVeloY.HasValue && !double.IsNaN(aVeloY.Value)) //&& !double.IsNaN(newVelo.Value.y)
                                            {
                                                var someNewVelo = new Vector2(aVeloX.Value, aVeloY.Value);

                                                arrayOfGameObjects[i].velocity = someNewVelo;
                                                arrayOfGameObjects[i].lerpPos = null;
                                                arrayOfGameObjects[i].collideNorm = null;
                                                arrayOfGameObjects[i].lerp = 0;

                                                //UnityEngine.Debug.Log(arrayOfGameObjects[i].orientation);
                                            }*/

                                            int someResult = ResolveCollision(arrayOfGameObjects[i], arrayOfGameObjects[arrayOfGameObjects[i].indexToCollideWith], out aVeloX, out aVeloY, theTime);
                                            //UnityEngine.Debug.Log(arrayOfGameObjects[arrayOfGameObjects[i].indexToCollideWith].currentObject.name);
                                            if (aVeloX.HasValue && !double.IsNaN(aVeloX.Value) && aVeloY.HasValue && !double.IsNaN(aVeloY.Value)) //&& !double.IsNaN(newVelo.Value.y)
                                            {
                                                var someNewVelo = new Vector2(aVeloX.Value, aVeloY.Value);

                                                arrayOfGameObjects[i].velocity = someNewVelo;
                                                arrayOfGameObjects[i].lerpPos = null;
                                                arrayOfGameObjects[i].collideNorm = null;
                                                arrayOfGameObjects[i].lerp = 0;

                                                //UnityEngine.Debug.Log(arrayOfGameObjects[i].orientation);
                                            }
                                        }
                                        else
                                        {
                                            arrayOfGameObjects[i].currentObject.transform.position = someNewFramePoser;
                                            arrayOfGameObjects[i].position = someNewFramePoser;
                                            arrayOfGameObjects[i].velocity = dummyObject.velocity;
                                            //arrayOfGameObjects[i].lerpPos = null;
                                            //arrayOfGameObjects[i].collideNorm = null;
                                            //arrayOfGameObjects[i].lerp = 0;
                                        }
                                    }
                                }

                            }
                            else // to REVISE as right now, a collision repulsion is done the moment that next frame would intersect which is good
                            {    // BUT the velocity is bigger the faster the object is or the higher it falls from so I need to be able to make
                                 // sure that the velocity will never be too big otherwise the collision will happen too soon in "AIR" without ever
                                 // getting close to the object it's colliding with.

                                float? aVeloX;
                                float? aVeloY;

                                /*resolveCollision(arrayOfGameObjects[i], arrayOfGameObjects[arrayOfGameObjects[i].indexToCollideWith], out aVeloX, out aVeloY,theTime);

                                if (aVeloX.HasValue && !double.IsNaN(aVeloX.Value) && aVeloY.HasValue && !double.IsNaN(aVeloY.Value)) //&& !double.IsNaN(newVelo.Value.y)
                                {
                                    var someNewVelo = new Vector2(aVeloX.Value, aVeloY.Value);

                                    arrayOfGameObjects[i].velocity = someNewVelo;
                                    arrayOfGameObjects[i].lerpPos = null;
                                    arrayOfGameObjects[i].collideNorm = null;
                                    arrayOfGameObjects[i].lerp = 0;

                                    //UnityEngine.Debug.Log(arrayOfGameObjects[i].orientation);
                                }*/




                                int someResult = ResolveCollision(arrayOfGameObjects[i], arrayOfGameObjects[arrayOfGameObjects[i].indexToCollideWith], out aVeloX, out aVeloY, theTime);
                                //UnityEngine.Debug.Log(arrayOfGameObjects[arrayOfGameObjects[i].indexToCollideWith].currentObject.name);
                                if (aVeloX.HasValue && !double.IsNaN(aVeloX.Value) && aVeloY.HasValue && !double.IsNaN(aVeloY.Value)) //&& !double.IsNaN(newVelo.Value.y)
                                {
                                    var someNewVelo = new Vector2(aVeloX.Value, aVeloY.Value);

                                    arrayOfGameObjects[i].velocity = someNewVelo;
                                    arrayOfGameObjects[i].lerpPos = null;
                                    arrayOfGameObjects[i].collideNorm = null;
                                    arrayOfGameObjects[i].lerp = 0;

                                    //UnityEngine.Debug.Log(arrayOfGameObjects[i].orientation);
                                }
                            }
                        }
                        else
                        {
                            //UnityEngine.Debug.Log("test01");
                            arrayOfGameObjects[i].currentObject.transform.position = someNewFramePos;// dummyObject.position;
                            arrayOfGameObjects[i].position = someNewFramePos;// dummyObject.position;
                            arrayOfGameObjects[i].velocity = dummyObject.velocity;
                            //arrayOfGameObjects[i].lerpPos = null;
                            //arrayOfGameObjects[i].collideNorm = null;
                            //arrayOfGameObjects[i].lerp = 0;
                        }
                    }
                    else
                    {
                     
                        //UnityEngine.Debug.Log("no value");
                        arrayOfGameObjects[i].currentObject.transform.position = dummyObject.position;
                        arrayOfGameObjects[i].position = dummyObject.position;
                        arrayOfGameObjects[i].velocity = dummyObject.velocity;
                        //arrayOfGameObjects[i].lerpPos = null;
                        //arrayOfGameObjects[i].collideNorm = null;
                        //arrayOfGameObjects[i].lerp = 0;
                    }
                }
            }

            for (int i = 0; i < arrayOfGameObjects.Length; i++)
            {
                if (arrayOfGameObjects[i].isStatic == 0)
                {
                    arrayOfGameObjects[i].lastFramePos = arrayOfGameObjects[i].currentObject.transform.position;
                    arrayOfGameObjects[i].lastFrameVelo = arrayOfGameObjects[i].velocity;
                    //arrayOfGameObjects[i].lastFramePos = arrayOfGameObjects[i].currentObject.transform.position;
                    //arrayOfGameObjects[i].lastFrameVelo = arrayOfGameObjects[i].velocity;
                    //arrayOfGameObjects[i].lastTimeToHit = arrayOfGameObjects[i].timeToHit;

                    if (arrayOfGameObjects[i].lerpPos.HasValue)
                    {
                        arrayOfGameObjects[i].lastLerpPos = arrayOfGameObjects[i].lerpPos.Value;
                    }
                }
                else
                {

                }
            }

            //lastMainWatchTime = mainWatch.Elapsed.TotalSeconds;
            mainFrameCounter++;
            await Task.Delay(timeOut);
            Thread.Sleep(1);
            //await Task.Yield();
            //_tme.WaitOne();
            if (killTask == 0)
            {
                goto _threadLoop;
            }
        }

        private void OnDestroy()
        {
            killTask = 1;
            tsk.Dispose();
        }
        private void OnDisable()
        {
            killTask = 1;
            tsk.Dispose();
        }

        List<Vector2> arrayOfSomeVectorsIntersect = new List<Vector2>();
        List<Segment> arrayOfSomeVectorsObject1 = new List<Segment>();
        List<Segment> arrayOfSomeVectorsObject2 = new List<Segment>();

        List<SC_RigidInfo> arrayOfSomeGameobjects1 = new List<SC_RigidInfo>();
        List<SC_RigidInfo> arrayOfSomeGameobjects2 = new List<SC_RigidInfo>();
        List<float> arrayOfFloaters = new List<float>();
        List<float> arrayOfFloaters2 = new List<float>();
        List<float> arrayOfFloaters3 = new List<float>();

        List<SC_RigidInfo> arrayOfSomeGameobjects3 = new List<SC_RigidInfo>();
        List<SC_RigidInfo> arrayOfSomeGameobjects4 = new List<SC_RigidInfo>();

        float timeToHit;
        int stopSomeStuff = 0;
        Vector2? intersector0;
        int hasGotCol = 0;
        int setTimeToHitAgain = 0;
        Vector2? intersector00;

        SC_RigidInfo obj1;
        SC_RigidInfo obj2;

        List<SC_RigidInfo> someRigidList = new List<SC_RigidInfo>();

        /*SC_RigidInfo resetObject(SC_RigidInfo currentFallingObject)
        {
            currentFallingObject.velocity = Vector2.zero;
            currentFallingObject.currentObject.transform.position = currentFallingObject.originalPosition;
            currentFallingObject.position = originalPos;
            currentFallingObject.lerp = 0;
            currentFallingObject.lerpPos = null;
            currentFallingObject.collideNorm = null;
            currentFallingObject.isBusy = 0;
            //currentFallingObject.isStatic = currentFallingObject.isStatic;
            currentFallingObject.timeToHit = 0;
            currentFallingObject.hasWatchStarted = 0;
            currentFallingObject.fallingWatch.Stop();
            currentFallingObject.fallingWatch.Reset();
            currentFallingObject.extendedVelocity = Vector2.zero;
            currentFallingObject.lastFramePos = originalPos;
            currentFallingObject.lastFrameTime = 0;
            currentFallingObject.lastFrameVelo = Vector2.zero;
            currentFallingObject.lastImpulseVelocity = null;
            currentFallingObject.hasWatchStarted = 0;

            return currentFallingObject;
        }*/



        /*SC_RigidInfo resetObjectStopWatch(SC_RigidInfo currentFallingObject)
        {
            currentFallingObject.fallingWatch.Stop();
            currentFallingObject.fallingWatch.Reset();
            return currentFallingObject;
        }

        SC_RigidInfo restartObjectStopWatch(SC_RigidInfo currentFallingObject)
        {
            currentFallingObject.fallingWatch.Stop();
            currentFallingObject.fallingWatch.Reset();
            currentFallingObject.fallingWatch.Start();
            return currentFallingObject;
        }*/

        int someTestingSwitch = 0;


        Stopwatch mainWatch = new Stopwatch();
        int mainWatchSwitch = 0;
        double lastMainWatchTime = 0;
        int mainFrameCounter = 0;

        List<Vector2> arrayOfSomeVectorsIntersect2 = new List<Vector2>();
        List<Segment> arrayOfSomeVectorsObject3 = new List<Segment>();
        List<Segment> arrayOfSomeVectorsObject4 = new List<Segment>();

        List<Vector2> arrayOfNormals = new List<Vector2>();










        int? isColliding(out SC_RigidInfo object1, out SC_RigidInfo object2, int checkOne, SC_RigidInfo singleObject, float theTime) //SC_RigidInfo[]
        {
            object1 = singleObject;
            object2 = null;

            arrayOfSomeVectorsIntersect = new List<Vector2>();
            arrayOfSomeVectorsObject1 = new List<Segment>();
            arrayOfSomeVectorsObject2 = new List<Segment>();
            arrayOfSomeGameobjects1 = new List<SC_RigidInfo>();
            arrayOfSomeGameobjects2 = new List<SC_RigidInfo>();
            arrayOfFloaters = new List<float>();

            arrayOfSomeVectorsIntersect2 = new List<Vector2>();
            arrayOfSomeGameobjects3 = new List<SC_RigidInfo>();
            arrayOfSomeGameobjects4 = new List<SC_RigidInfo>();
            arrayOfSomeVectorsObject3 = new List<Segment>();
            arrayOfSomeVectorsObject4 = new List<Segment>();
            arrayOfFloaters2 = new List<float>();
            arrayOfNormals = new List<Vector2>();

            if (singleObject.isStatic == 0) // && singleObject.isBusy == 0
            {
                Vector2[] arrayOfAllCorners = new Vector2[4];

                arrayOfAllCorners[0] = singleObject.currentObject.transform.TransformPoint(singleObject.corner1);
                arrayOfAllCorners[1] = singleObject.currentObject.transform.TransformPoint(singleObject.corner2);
                arrayOfAllCorners[2] = singleObject.currentObject.transform.TransformPoint(singleObject.corner3);
                arrayOfAllCorners[3] = singleObject.currentObject.transform.TransformPoint(singleObject.corner4);

                var lowestX = arrayOfAllCorners.OrderBy(x => x.X).FirstOrDefault();
                var highestX = arrayOfAllCorners.OrderBy(x => x.X).Last();

                var lowestY = arrayOfAllCorners.OrderBy(y => y.Y).FirstOrDefault();
                var highestY = arrayOfAllCorners.OrderBy(y => y.Y).Last();

                Vector2 currentPos = new Vector2(singleObject.currentObject.transform.position.x, singleObject.currentObject.transform.position.y);
                Vector3 dir = (currentPos + (singleObject.velocity * theTime)) - currentPos;

                /*float offsetOfDir = 0;

                if (Mathf.Abs(lowestX.x - highestX.x) < Mathf.Abs(lowestY.y - highestY.y))
                {
                    offsetOfDir = Mathf.Abs(lowestY.y - highestY.y);
                }
                else
                {
                    offsetOfDir = Mathf.Abs(lowestX.x - highestX.x);
                }

                Vector2 supposedVelo = singleObject.velocity;

                if (singleObject.velocity.magnitude < Vector2.one.magnitude)
                {
                    supposedVelo = singleObject.velocity.normalized;
                }


                if (Mathf.Abs(singleObject.velocity.x) < Mathf.Abs(singleObject.velocity.y))
                {
                    offsetOfDir = Mathf.Abs(lowestX.x - highestX.x);
                }
                else
                {
                    offsetOfDir = Mathf.Abs(lowestY.y - highestY.y);
                }


                Vector3 dir = (supposedVelo * (offsetOfDir * 1.5f));
                singleObject.extendedVelocity = supposedVelo * (offsetOfDir * 1.5f);
                */

                hasGotCol = 0;
                intersector0 = null;

                //UnityEngine.Debug.DrawRay(singleObject.currentObject.transform.TransformPoint(singleObject.corner1), dir, Color.green, 0.1f);
                //UnityEngine.Debug.DrawRay(singleObject.currentObject.transform.TransformPoint(singleObject.corner2), dir, Color.green, 0.1f);
                //UnityEngine.Debug.DrawRay(singleObject.currentObject.transform.TransformPoint(singleObject.corner3), dir, Color.green, 0.1f);
                //UnityEngine.Debug.DrawRay(singleObject.currentObject.transform.TransformPoint(singleObject.corner4), dir, Color.green, 0.1f);

                Segment one0 = new Segment();
                one0.Start = singleObject.currentObject.transform.TransformPoint(singleObject.corner1);
                one0.End = singleObject.currentObject.transform.TransformPoint(singleObject.corner1) + dir;

                Segment two0 = new Segment();
                two0.Start = singleObject.currentObject.transform.TransformPoint(singleObject.corner2);
                two0.End = singleObject.currentObject.transform.TransformPoint(singleObject.corner2) + dir;

                Segment three0 = new Segment();
                three0.Start = singleObject.currentObject.transform.TransformPoint(singleObject.corner3);
                three0.End = singleObject.currentObject.transform.TransformPoint(singleObject.corner3) + dir;

                Segment four0 = new Segment();
                four0.Start = singleObject.currentObject.transform.TransformPoint(singleObject.corner4);
                four0.End = singleObject.currentObject.transform.TransformPoint(singleObject.corner4) + dir;

                Segment[] arrayOfCornerForCollision00 = new Segment[4];
                arrayOfCornerForCollision00[0] = one0;
                arrayOfCornerForCollision00[1] = two0;
                arrayOfCornerForCollision00[2] = three0;
                arrayOfCornerForCollision00[3] = four0;

                Segment one1 = new Segment();
                one1.Start = singleObject.currentObject.transform.TransformPoint(singleObject.corner1);
                one1.End = singleObject.currentObject.transform.TransformPoint(singleObject.corner2);

                Segment two1 = new Segment();
                two1.Start = singleObject.currentObject.transform.TransformPoint(singleObject.corner2);
                two1.End = singleObject.currentObject.transform.TransformPoint(singleObject.corner3);

                Segment three1 = new Segment();
                three1.Start = singleObject.currentObject.transform.TransformPoint(singleObject.corner3);
                three1.End = singleObject.currentObject.transform.TransformPoint(singleObject.corner4);

                Segment four1 = new Segment();
                four1.Start = singleObject.currentObject.transform.TransformPoint(singleObject.corner4);
                four1.End = singleObject.currentObject.transform.TransformPoint(singleObject.corner1);

                Segment[] arrayOfBoundsSegments00 = new Segment[4];
                arrayOfBoundsSegments00[0] = one1;
                arrayOfBoundsSegments00[1] = two1;
                arrayOfBoundsSegments00[2] = three1;
                arrayOfBoundsSegments00[3] = four1;

                for (int ii = 0; ii < arrayOfGameObjects.Length; ii++)
                {
                    if (arrayOfGameObjects[ii].currentObject != singleObject.currentObject)
                    {
                        if (arrayOfGameObjects[ii].velocity != new Vector2(0, 0) || singleObject.velocity != new Vector2(0, 0))
                        {
                            var one00 = new Segment();
                            one00.Start = arrayOfGameObjects[ii].currentObject.transform.TransformPoint(arrayOfGameObjects[ii].corner1);
                            one00.End = arrayOfGameObjects[ii].currentObject.transform.TransformPoint(arrayOfGameObjects[ii].corner1) + -dir;

                            var two00 = new Segment();
                            two00.Start = arrayOfGameObjects[ii].currentObject.transform.TransformPoint(arrayOfGameObjects[ii].corner2);
                            two00.End = arrayOfGameObjects[ii].currentObject.transform.TransformPoint(arrayOfGameObjects[ii].corner2) + -dir;

                            var three00 = new Segment();
                            three00.Start = arrayOfGameObjects[ii].currentObject.transform.TransformPoint(arrayOfGameObjects[ii].corner3);
                            three00.End = arrayOfGameObjects[ii].currentObject.transform.TransformPoint(arrayOfGameObjects[ii].corner3) + -dir;

                            var four00 = new Segment();
                            four00.Start = arrayOfGameObjects[ii].currentObject.transform.TransformPoint(arrayOfGameObjects[ii].corner4);
                            four00.End = arrayOfGameObjects[ii].currentObject.transform.TransformPoint(arrayOfGameObjects[ii].corner4) + -dir;

                            Segment[] arrayOfCornerForCollision01 = new Segment[4];
                            arrayOfCornerForCollision01[0] = one00;
                            arrayOfCornerForCollision01[1] = two00;
                            arrayOfCornerForCollision01[2] = three00;
                            arrayOfCornerForCollision01[3] = four00;

                            var one11 = new Segment();
                            one11.Start = arrayOfGameObjects[ii].currentObject.transform.TransformPoint(arrayOfGameObjects[ii].corner1);
                            one11.End = arrayOfGameObjects[ii].currentObject.transform.TransformPoint(arrayOfGameObjects[ii].corner2);

                            var two11 = new Segment();
                            two11.Start = arrayOfGameObjects[ii].currentObject.transform.TransformPoint(arrayOfGameObjects[ii].corner2);
                            two11.End = arrayOfGameObjects[ii].currentObject.transform.TransformPoint(arrayOfGameObjects[ii].corner3);

                            var three11 = new Segment();
                            three11.Start = arrayOfGameObjects[ii].currentObject.transform.TransformPoint(arrayOfGameObjects[ii].corner3);
                            three11.End = arrayOfGameObjects[ii].currentObject.transform.TransformPoint(arrayOfGameObjects[ii].corner4);

                            var four11 = new Segment();
                            four11.Start = arrayOfGameObjects[ii].currentObject.transform.TransformPoint(arrayOfGameObjects[ii].corner4);
                            four11.End = arrayOfGameObjects[ii].currentObject.transform.TransformPoint(arrayOfGameObjects[ii].corner1);

                            Segment[] arrayOfBoundsSegments01 = new Segment[4];
                            arrayOfBoundsSegments01[0] = one11;
                            arrayOfBoundsSegments01[1] = two11;
                            arrayOfBoundsSegments01[2] = three11;
                            arrayOfBoundsSegments01[3] = four11;

                            for (int c0 = 0; c0 < arrayOfCornerForCollision00.Length; c0++)
                            {
                                for (int c1 = 0; c1 < arrayOfBoundsSegments01.Length; c1++)
                                {
                                    intersector0 = SC_Segment.Intersects(arrayOfBoundsSegments01[c1], arrayOfCornerForCollision00[c0]);

                                    if (intersector0.HasValue && !double.IsNaN(intersector0.Value.X) && !double.IsNaN(intersector0.Value.Y))
                                    {

                                        /*//Vector2 diff = singleObject.position - new Vector2(singleObject.currentObject.transform.position.x, singleObject.currentObject.transform.position.y) ;
                                        Vector2 currentPos = new Vector2(singleObject.currentObject.transform.position.x, singleObject.currentObject.transform.position.y);
                                        Vector2 diff = (currentPos + (singleObject.velocity * theTime)) - currentPos;

                                        Vector2 currentCollisionPos = intersector0.Value;
                                        Segment currentSegment = arrayOfBoundsSegments01[c1];
                                        Vector2 currentPoint = arrayOfCornerForCollision00[c0].Start;
                                        var someOtherDir = (arrayOfCornerForCollision00[c0].Start - currentCollisionPos);

                                        Vector2 beforePosOfPoint = currentPoint;
                                        Vector2 afterPosOfPoint = currentPoint + diff;// (singleObject.velocity.normalized * offsetOfDir);


                                        bool initialDot = NSEW(currentSegment.Start, currentSegment.End, beforePosOfPoint);// Dot(beforeDirPos.x, beforeDirPos.y, dirOfSegment.x, dirOfSegment.y);
                                        bool endDot = NSEW(currentSegment.Start, currentSegment.End, afterPosOfPoint);

                                        if (initialDot != endDot)
                                        {
                                            UnityEngine.Debug.Log("here00");
                                            arrayOfSomeGameobjects1.Add(singleObject);
                                            arrayOfSomeGameobjects2.Add(arrayOfGameObjects[ii]);

                                            arrayOfSomeVectorsIntersect.Add(currentCollisionPos);
                                            arrayOfSomeVectorsObject1.Add(arrayOfCornerForCollision00[c0]);
                                            arrayOfSomeVectorsObject2.Add(arrayOfBoundsSegments01[c1]);

                                            float dist = Vector2.Distance(arrayOfCornerForCollision00[c0].Start, currentCollisionPos);
                                            arrayOfFloaters.Add(dist);
                                        }*/

                                        Vector2 currentCollisionPos = intersector0.Value;
                                        arrayOfSomeGameobjects1.Add(singleObject);
                                        arrayOfSomeGameobjects2.Add(arrayOfGameObjects[ii]);

                                        arrayOfSomeVectorsIntersect.Add(currentCollisionPos);
                                        arrayOfSomeVectorsObject1.Add(arrayOfCornerForCollision00[c0]);
                                        arrayOfSomeVectorsObject2.Add(arrayOfBoundsSegments01[c1]);

                                        //Instantiate(pointer1,arrayOfCornerForCollision00[c0].Start,Quaternion.identity);
                                        //Instantiate(pointer1, currentCollisionPos, Quaternion.identity);

                                        float dist = Vector2.Distance(arrayOfCornerForCollision00[c0].Start, currentCollisionPos);
                                        arrayOfFloaters.Add(dist);
                                    }
                                }
                            }

                            /*int minIndex = arrayOfFloaters.IndexOf(arrayOfFloaters.Min());
                            object1 = arrayOfGameObjects[arrayOfSomeGameobjects1[minIndex].mainIndex];
                            object2 = arrayOfGameObjects[arrayOfSomeGameobjects2[minIndex].mainIndex];
                            float minDist = arrayOfFloaters.Min();

                            var intersecter = arrayOfSomeVectorsIntersect[minIndex];
                            var collisionVertex = arrayOfSomeVectorsObject1[minIndex];
                            var collisionSegment = arrayOfSomeVectorsObject2[minIndex];*/

                            for (int c0 = 0; c0 < arrayOfCornerForCollision01.Length; c0++)
                            {
                                for (int c1 = 0; c1 < arrayOfBoundsSegments00.Length; c1++)
                                {
                                    intersector00 = SC_Segment.Intersects(arrayOfBoundsSegments00[c1], arrayOfCornerForCollision01[c0]);

                                    if (intersector00.HasValue && !double.IsNaN(intersector00.Value.X) && !double.IsNaN(intersector00.Value.Y))
                                    {
                                        /*Vector2 currentPos = new Vector2(singleObject.currentObject.transform.position.x, singleObject.currentObject.transform.position.y);
                                        Vector2 diff = (currentPos + (singleObject.velocity * theTime)) - currentPos;
                                        //UnityEngine.Debug.DrawRay(currentPos, diff, Color.magenta, 0.1f);
                                        Vector2 currentCollisionPos = intersector00.Value;
                                        var someOtherDir = (arrayOfCornerForCollision01[c0].Start - currentCollisionPos);
                                        var colliderPos = arrayOfCornerForCollision01[c0].Start - (someOtherDir * highExtent);
                                        Vector2 currentPoint = colliderPos;
                                        Vector2 beforePosOfPoint = currentPoint;
                                        Vector2 afterPosOfPoint = currentPoint + diff;// (singleObject.velocity.normalized * offsetOfDir);

                                        //Vector2 someInitialPos = beforePosOfPoint - arrayOfBoundsSegments00[c1].Start;
                                        //Vector2 someAfterPos = afterPosOfPoint - arrayOfBoundsSegments00[c1].Start;

                                        Vector2 someFuckingPoint = arrayOfCornerForCollision01[c0].Start;

                                        bool initialDot = NSEW(beforePosOfPoint, arrayOfBoundsSegments00[c1].Start, someFuckingPoint);
                                        bool endDot = NSEW(afterPosOfPoint, arrayOfBoundsSegments00[c1].Start, someFuckingPoint);

                                        //Instantiate(pointer1, someFuckingPoint, Quaternion.identity);
                                        //Instantiate(pointer2, currentSegment.End, Quaternion.identity);

                                        if (initialDot != endDot)
                                        {
                                            //UnityEngine.Debug.Log("WTF");

                                            arrayOfSomeGameobjects3.Add(singleObject);
                                            arrayOfSomeGameobjects4.Add(arrayOfGameObjects[ii]);

                                            arrayOfSomeVectorsIntersect2.Add(currentCollisionPos);
                                            arrayOfSomeVectorsObject3.Add(arrayOfBoundsSegments00[c1]);
                                            arrayOfSomeVectorsObject4.Add(arrayOfCornerForCollision01[c0]);

                                            float dist = Vector2.Distance(arrayOfCornerForCollision01[c0].Start, currentCollisionPos);
                                            arrayOfFloaters2.Add(dist);
                                        }*/

                                        Vector2 currentCollisionPos = intersector00.Value;
                                        arrayOfSomeGameobjects3.Add(singleObject);
                                        arrayOfSomeGameobjects4.Add(arrayOfGameObjects[ii]);

                                        arrayOfSomeVectorsIntersect2.Add(currentCollisionPos);
                                        arrayOfSomeVectorsObject3.Add(arrayOfBoundsSegments00[c1]);
                                        arrayOfSomeVectorsObject4.Add(arrayOfCornerForCollision01[c0]);

                                        float dist = Vector2.Distance(arrayOfCornerForCollision01[c0].Start, currentCollisionPos);
                                        arrayOfFloaters2.Add(dist);
                                    }
                                }
                            }
                        }
                    }
                }
            }

            //UnityEngine.Debug.Log("1: " + arrayOfFloaters.Count + " 2: " + arrayOfFloaters2.Count);

            if (arrayOfFloaters.Count > 0)
            {
                //UnityEngine.Debug.Log("collision 00");
                int minIndex = arrayOfFloaters.IndexOf(arrayOfFloaters.Min());
                object1 = arrayOfGameObjects[arrayOfSomeGameobjects1[minIndex].mainIndex];
                object2 = arrayOfGameObjects[arrayOfSomeGameobjects2[minIndex].mainIndex];
                float minDist = arrayOfFloaters.Min();

                if (arrayOfFloaters2.Count > 0)
                {
                    var minIndex2 = arrayOfFloaters2.IndexOf(arrayOfFloaters2.Min());
                    float minDist2 = arrayOfFloaters2.Min();

                    if (minDist < minDist2)
                    {
                        /*if (arrayOfSomeGameobjects2[minIndex].mainIndex == arrayOfSomeGameobjects4[minIndex2].mainIndex)
                        {
                            object1 = arrayOfGameObjects[arrayOfSomeGameobjects3[minIndex2].mainIndex];
                            object2 = arrayOfGameObjects[arrayOfSomeGameobjects4[minIndex2].mainIndex];

                            object1.indexToCollideWith = arrayOfSomeGameobjects4[minIndex2].mainIndex;
                            object2.indexToCollideWith = arrayOfSomeGameobjects3[minIndex2].mainIndex;


                            var someOtherDir = (arrayOfSomeVectorsObject3[minIndex2].Start - arrayOfSomeVectorsIntersect[minIndex]);
                            var intersectorValue = arrayOfSomeVectorsIntersect[minIndex] + (someOtherDir.normalized * lowExtent);
                            //Instantiate(pointer1, intersectorValue, Quaternion.identity);
                            Vector2 dirToColPoint = arrayOfSomeVectorsObject3[minIndex2].Start - new Vector2(object1.currentObject.transform.position.x, object1.currentObject.transform.position.y);
                            //object1.lerpPos = intersectorValue - dirToColPoint;

                            float currentOffsetX = (-dirToColPoint.x);
                            float currentOffsetY = (-dirToColPoint.y);


                            object1.lerpPos = intersectorValue;

                            Vector2 oriLerp = object1.lerpPos.Value;
                            oriLerp.x += currentOffsetX;
                            oriLerp.y += currentOffsetY;
                            object1.lerpPos = oriLerp;


                            Vector2 normalOfCollision = arrayOfSomeVectorsObject4[minIndex2].End - arrayOfSomeVectorsObject4[minIndex2].Start;
                            Vector2 normalCol = normalOfCollision;
                            normalOfCollision.x = normalCol.y;
                            normalOfCollision.y = -normalCol.x;
                            lastNormVec = normalOfCollision;

                            object1.collideNorm = lastNormVec.Value;

                            object1.colSegment = arrayOfSomeVectorsObject4[minIndex2];
                            UnityEngine.Debug.DrawRay(arrayOfSomeVectorsIntersect2[minIndex2], lastNormVec.Value, Color.cyan, 10);
                            UnityEngine.Debug.Log("collision 00");*/
                        /*object1 = arrayOfGameObjects[arrayOfSomeGameobjects3[minIndex2].mainIndex];
                        object2 = arrayOfGameObjects[arrayOfSomeGameobjects4[minIndex2].mainIndex];

                        object1.indexToCollideWith = arrayOfSomeGameobjects4[minIndex2].mainIndex;
                        object2.indexToCollideWith = arrayOfSomeGameobjects3[minIndex2].mainIndex;

                        Vector2 currentCollisionPos = arrayOfSomeVectorsIntersect2[minIndex2];

                        var someOtherDir = (arrayOfSomeVectorsObject4[minIndex2].Start - currentCollisionPos);
                        var colliderPos = arrayOfSomeVectorsIntersect2[minIndex2] + (someOtherDir * highExtent);

                        var intersectorValue = arrayOfSomeVectorsIntersect2[minIndex2] - new Vector2(object1.currentObject.transform.position.x, object1.currentObject.transform.position.y);


                        //Instantiate(pointer1, colliderPos, Quaternion.identity);


                        float currentOffsetX = (-intersectorValue.x);
                        float currentOffsetY = (-intersectorValue.y);

                        object1.lerpPos = colliderPos;

                        Vector2 oriLerp = object1.lerpPos.Value;

                        oriLerp.x += currentOffsetX;
                        oriLerp.y += currentOffsetY;

                        object1.lerpPos = oriLerp;

                        Vector2 normalOfCollision = arrayOfSomeVectorsObject3[minIndex2].End - arrayOfSomeVectorsObject3[minIndex2].Start;
                        Vector2 normalCol = normalOfCollision;
                        normalOfCollision.x = -normalCol.y;
                        normalOfCollision.y = normalCol.x;

                        lastNormVec = normalOfCollision;

                        object1.collideNorm = lastNormVec.Value;

                        UnityEngine.Debug.DrawRay(arrayOfSomeVectorsIntersect2[minIndex2], lastNormVec.Value, Color.blue, 10);
                        UnityEngine.Debug.Log("collision 00");
                        return 0;
                    }
                    else
                    {
                        object1 = arrayOfGameObjects[arrayOfSomeGameobjects1[minIndex].mainIndex];
                        object2 = arrayOfGameObjects[arrayOfSomeGameobjects2[minIndex].mainIndex];

                        object1.indexToCollideWith = arrayOfSomeGameobjects2[minIndex].mainIndex;
                        object2.indexToCollideWith = arrayOfSomeGameobjects1[minIndex].mainIndex;

                        var someOtherDir = (arrayOfSomeVectorsObject1[minIndex].Start - arrayOfSomeVectorsIntersect[minIndex]);
                        var intersectorValue = arrayOfSomeVectorsIntersect[minIndex] + (someOtherDir.normalized * lowExtent);

                        //Instantiate(pointer1, intersectorValue, Quaternion.identity);


                        Vector2 dirToColPoint = arrayOfSomeVectorsObject1[minIndex].Start - new Vector2(object1.currentObject.transform.position.x, object1.currentObject.transform.position.y);
                        //object1.lerpPos = intersectorValue - dirToColPoint;

                        float currentOffsetX = (-dirToColPoint.x);
                        float currentOffsetY = (-dirToColPoint.y);


                        object1.lerpPos = intersectorValue;

                        Vector2 oriLerp = object1.lerpPos.Value;
                        oriLerp.x += currentOffsetX;
                        oriLerp.y += currentOffsetY;
                        object1.lerpPos = oriLerp;


                        Vector2 normalOfCollision = arrayOfSomeVectorsObject2[minIndex].End - arrayOfSomeVectorsObject2[minIndex].Start;
                        Vector2 normalCol = normalOfCollision;
                        normalOfCollision.x = normalCol.y;
                        normalOfCollision.y = -normalCol.x;
                        lastNormVec = normalOfCollision;

                        object1.collideNorm = lastNormVec.Value;

                        object1.colSegment = arrayOfSomeVectorsObject2[minIndex];
                        UnityEngine.Debug.DrawRay(arrayOfSomeVectorsIntersect[minIndex], lastNormVec.Value, Color.cyan, 10);
                        UnityEngine.Debug.Log("collision 01");
                        return 1;
                    }*/

                        object1 = arrayOfGameObjects[arrayOfSomeGameobjects1[minIndex].mainIndex];
                        object2 = arrayOfGameObjects[arrayOfSomeGameobjects2[minIndex].mainIndex];

                        object1.indexToCollideWith = arrayOfSomeGameobjects2[minIndex].mainIndex;
                        object2.indexToCollideWith = arrayOfSomeGameobjects1[minIndex].mainIndex;

                        var someOtherDir = (arrayOfSomeVectorsObject1[minIndex].Start - arrayOfSomeVectorsIntersect[minIndex]);
                        var intersectorValue = arrayOfSomeVectorsIntersect[minIndex] + (someOtherDir.normalized * lowExtent);

                        //Instantiate(pointer1, intersectorValue, Quaternion.identity);


                        Vector2 dirToColPoint = arrayOfSomeVectorsObject1[minIndex].Start - new Vector2(object1.currentObject.transform.position.x, object1.currentObject.transform.position.y);
                        //object1.lerpPos = intersectorValue - dirToColPoint;

                        float currentOffsetX = (-dirToColPoint.X);
                        float currentOffsetY = (-dirToColPoint.Y);


                        object1.lerpPos = intersectorValue;

                        Vector2 oriLerp = object1.lerpPos.Value;
                        oriLerp.X += currentOffsetX;
                        oriLerp.Y += currentOffsetY;
                        object1.lerpPos = oriLerp;


                        Vector2 normalOfCollision = arrayOfSomeVectorsObject2[minIndex].End - arrayOfSomeVectorsObject2[minIndex].Start;
                        Vector2 normalCol = normalOfCollision;
                        normalOfCollision.X = normalCol.Y;
                        normalOfCollision.Y = -normalCol.X;
                        lastNormVec = normalOfCollision;


                        Vector2 reflectedCollision = Vector2.Reflect(object1.velocity.normalized, lastNormVec.Value.normalized);
                        object1.collideNorm = reflectedCollision;
                        object1.collidePoint = intersectorValue;
                        //object1.colSegment = arrayOfSomeVectorsObject2[minIndex];
                        //UnityEngine.Debug.DrawRay(arrayOfSomeVectorsIntersect[minIndex], reflectedCollision, UnityEngine.Color.cyan, 10);
                        //UnityEngine.Debug.Log("collision 01");
                        return 1;
                    }
                    else
                    {
                        object1 = arrayOfGameObjects[arrayOfSomeGameobjects3[minIndex2].mainIndex];
                        object2 = arrayOfGameObjects[arrayOfSomeGameobjects4[minIndex2].mainIndex];

                        object1.indexToCollideWith = arrayOfSomeGameobjects4[minIndex2].mainIndex;
                        object2.indexToCollideWith = arrayOfSomeGameobjects3[minIndex2].mainIndex;

                        Vector2 currentCollisionPos = arrayOfSomeVectorsIntersect2[minIndex2];

                        var someOtherDir = (arrayOfSomeVectorsObject4[minIndex2].Start - currentCollisionPos);
                        var colliderPos = arrayOfSomeVectorsIntersect2[minIndex2] + (someOtherDir * highExtent);

                        var intersectorValue = arrayOfSomeVectorsIntersect2[minIndex2] - new Vector2(object1.currentObject.transform.position.x, object1.currentObject.transform.position.y);


                        //Instantiate(pointer1, colliderPos, Quaternion.identity);


                        float currentOffsetX = (-intersectorValue.x);
                        float currentOffsetY = (-intersectorValue.y);

                        object1.lerpPos = colliderPos;

                        Vector2 oriLerp = object1.lerpPos.Value;

                        oriLerp.X += currentOffsetX;
                        oriLerp.Y += currentOffsetY;

                        object1.lerpPos = oriLerp;

                        Vector2 normalOfCollision = arrayOfSomeVectorsObject3[minIndex2].End - arrayOfSomeVectorsObject3[minIndex2].Start;
                        Vector2 normalCol = normalOfCollision;
                        normalOfCollision.X = -normalCol.Y;
                        normalOfCollision.Y = normalCol.X;

                        lastNormVec = normalOfCollision;

                        Vector2 reflectedCollision = Vector2.Reflect(object1.velocity.normalized, lastNormVec.Value.normalized);
                        object1.collideNorm = reflectedCollision;
                        object1.collidePoint = -intersectorValue;
                        //UnityEngine.Debug.DrawRay(arrayOfSomeVectorsIntersect2[minIndex2], reflectedCollision, UnityEngine.Color.blue, 10);
                        //UnityEngine.Debug.DrawRay(arrayOfSomeVectorsIntersect2[minIndex2], lastNormVec.Value, Color.blue, 10);
                        //UnityEngine.Debug.Log("collision 02");
                        return 2;
                    }
                }
                else
                {
                    object1.indexToCollideWith = arrayOfSomeGameobjects2[minIndex].mainIndex;
                    object2.indexToCollideWith = arrayOfSomeGameobjects1[minIndex].mainIndex;

                    var someOtherDir = (arrayOfSomeVectorsObject1[minIndex].Start - arrayOfSomeVectorsIntersect[minIndex]);
                    var intersectorValue = arrayOfSomeVectorsIntersect[minIndex] + (someOtherDir.normalized * lowExtent);

                    Vector2 dirToColPoint = arrayOfSomeVectorsObject1[minIndex].Start - new Vector2(object1.currentObject.transform.position.x, object1.currentObject.transform.position.y);
                    object1.lerpPos = intersectorValue - dirToColPoint;

                    Vector2 normalOfCollision = arrayOfSomeVectorsObject2[minIndex].End - arrayOfSomeVectorsObject2[minIndex].Start;
                    Vector2 normalCol = normalOfCollision;
                    normalOfCollision.X = normalCol.Y;
                    normalOfCollision.Y = -normalCol.X;
                    lastNormVec = normalOfCollision;


                    Vector2 reflectedCollision = Vector2.Reflect(object1.velocity, lastNormVec.Value.normalized);
                    object1.collideNorm = reflectedCollision;
                    object1.collidePoint = intersectorValue;

                    //object1.collidePoint = arrayOfSomeVectorsIntersect[minIndex];
                    //object1.otherCollidePoint = null;

                    //UnityEngine.Debug.DrawRay(arrayOfSomeVectorsIntersect[minIndex], reflectedCollision, UnityEngine.Color.white, 10);
                    //UnityEngine.Debug.DrawRay(arrayOfSomeVectorsIntersect[minIndex], lastNormVec.Value, Color.magenta, 10);
                    //Instantiate(pointer1, arrayOfSomeVectorsIntersect[minIndex], Quaternion.identity);
                    //UnityEngine.Debug.Log("collision 03");
                    return 3;
                }
            }
            else
            {
                if (arrayOfFloaters2.Count > 0)
                {
                    var minIndex2 = arrayOfFloaters2.IndexOf(arrayOfFloaters2.Min());
                    float minDist2 = arrayOfFloaters2.Min();
                    object1 = arrayOfGameObjects[arrayOfSomeGameobjects3[minIndex2].mainIndex];
                    object2 = arrayOfGameObjects[arrayOfSomeGameobjects4[minIndex2].mainIndex];

                    object1.indexToCollideWith = arrayOfSomeGameobjects4[minIndex2].mainIndex;
                    object2.indexToCollideWith = arrayOfSomeGameobjects3[minIndex2].mainIndex;

                    Vector2 currentCollisionPos = arrayOfSomeVectorsIntersect2[minIndex2];

                    var someOtherDir = (arrayOfSomeVectorsObject4[minIndex2].Start - currentCollisionPos);
                    var colliderPos = arrayOfSomeVectorsIntersect2[minIndex2] + (someOtherDir * highExtent);

                    var intersectorValue = arrayOfSomeVectorsIntersect2[minIndex2] - new Vector2(object1.currentObject.transform.position.x, object1.currentObject.transform.position.y);


                    float currentOffsetX = (-intersectorValue.x);
                    float currentOffsetY = (-intersectorValue.y);

                    object1.lerpPos = colliderPos;

                    Vector2 oriLerp = object1.lerpPos.Value;

                    oriLerp.X += currentOffsetX;
                    oriLerp.Y += currentOffsetY;

                    object1.lerpPos = oriLerp;

                    Vector2 normalOfCollision = arrayOfSomeVectorsObject3[minIndex2].End - arrayOfSomeVectorsObject3[minIndex2].Start;
                    Vector2 normalCol = normalOfCollision;
                    normalOfCollision.X = -normalCol.Y;
                    normalOfCollision.Y = normalCol.X;

                    lastNormVec = normalOfCollision;
                    object1.collidePoint = -intersectorValue;

                    Vector2 reflectedCollision = Vector2.Reflect(object1.velocity.normalized, lastNormVec.Value.normalized);
                    object1.collideNorm = reflectedCollision;

                    //UnityEngine.Debug.DrawRay(arrayOfSomeVectorsIntersect2[minIndex2], reflectedCollision, UnityEngine.Color.yellow, 10);
                    //UnityEngine.Debug.DrawRay(arrayOfSomeVectorsIntersect2[minIndex2], lastNormVec.Value, Color.blue, 10);

                    //UnityEngine.Debug.Log("collision 04");
                    return 4;
                }
            }
            return null;
        }

        int isIntersecting(out SC_RigidInfo object1, out SC_RigidInfo object2, int checkOne, SC_RigidInfo singleObject, out Vector2? intersectPoint, float theTime)
        {
            object1 = singleObject;
            object2 = null;

            if (singleObject.isStatic == 0)
            {
                hasGotCol = 0;
                intersector0 = null;


                //Vector2 diff = singleObject.position - new Vector2(singleObject.currentObject.transform.position.x, singleObject.currentObject.transform.position.y);

                Vector2 currentPos = new Vector2(singleObject.currentObject.transform.position.x, singleObject.currentObject.transform.position.y);
                Vector3 diff = (currentPos + (singleObject.velocity * theTime * lowExtent)) - currentPos;

                /*float offsetOfDir = 0;

                if (Mathf.Abs(lowestX.x - highestX.x) < Mathf.Abs(lowestY.y - highestY.y))
                {
                    offsetOfDir = Mathf.Abs(lowestY.y - highestY.y);
                }
                else
                {
                    offsetOfDir = Mathf.Abs(lowestX.x - highestX.x);
                }

                Vector2 supposedVelo = singleObject.velocity;

                if (singleObject.velocity.magnitude < Vector2.one.magnitude)
                {
                    supposedVelo = singleObject.velocity.normalized;
                }


                if (Mathf.Abs(singleObject.velocity.x) < Mathf.Abs(singleObject.velocity.y))
                {
                    offsetOfDir = Mathf.Abs(lowestX.x - highestX.x);
                }
                else
                {
                    offsetOfDir = Mathf.Abs(lowestY.y - highestY.y);
                }


                Vector3 dir = (supposedVelo * (offsetOfDir * 1.5f));
                singleObject.extendedVelocity = supposedVelo * (offsetOfDir * 1.5f);
                */



                Segment one1 = new Segment();
                one1.Start = singleObject.currentObject.transform.TransformPoint(singleObject.corner1) + new Vector3(diff.X, diff.Y, 0);
                one1.End = singleObject.currentObject.transform.TransformPoint(singleObject.corner2) + new Vector3(diff.X, diff.Y, 0);

                Segment two1 = new Segment();
                two1.Start = singleObject.currentObject.transform.TransformPoint(singleObject.corner2) + new Vector3(diff.X, diff.Y, 0);
                two1.End = singleObject.currentObject.transform.TransformPoint(singleObject.corner3) + new Vector3(diff.X, diff.Y, 0);

                Segment three1 = new Segment();
                three1.Start = singleObject.currentObject.transform.TransformPoint(singleObject.corner3) + new Vector3(diff.X, diff.Y, 0);
                three1.End = singleObject.currentObject.transform.TransformPoint(singleObject.corner4) + new Vector3(diff.X, diff.Y, 0);

                Segment four1 = new Segment();
                four1.Start = singleObject.currentObject.transform.TransformPoint(singleObject.corner4) + new Vector3(diff.x, diff.y, 0);
                four1.End = singleObject.currentObject.transform.TransformPoint(singleObject.corner1) + new Vector3(diff.x, diff.y, 0);

                /*
                Segment one1 = new Segment();
                one1.Start = (new Vector2(singleObject.corner1.x, singleObject.corner1.y) - singleObject.lerpPos.Value) + singleObject.lerpPos.Value;
                one1.End = (new Vector2(singleObject.corner2.x, singleObject.corner2.y) - singleObject.lerpPos.Value) + singleObject.lerpPos.Value;

                Segment two1 = new Segment();
                two1.Start = (new Vector2(singleObject.corner2.x, singleObject.corner2.y) - singleObject.lerpPos.Value) + singleObject.lerpPos.Value;
                two1.End = (new Vector2(singleObject.corner3.x, singleObject.corner3.y) - singleObject.lerpPos.Value) + singleObject.lerpPos.Value;

                Segment three1 = new Segment();
                three1.Start = (new Vector2(singleObject.corner3.x, singleObject.corner3.y) - singleObject.lerpPos.Value) + singleObject.lerpPos.Value;
                three1.End = (new Vector2(singleObject.corner4.x, singleObject.corner4.y) - singleObject.lerpPos.Value) + singleObject.lerpPos.Value;

                Segment four1 = new Segment();
                four1.Start = (new Vector2(singleObject.corner4.x, singleObject.corner4.y) - singleObject.lerpPos.Value) + singleObject.lerpPos.Value;
                four1.End = (new Vector2(singleObject.corner1.x, singleObject.corner1.y) - singleObject.lerpPos.Value) + singleObject.lerpPos.Value;
                */


                //Instantiate(pointer1, one1.Start, Quaternion.identity);
                //Instantiate(pointer1, one1.End, Quaternion.identity);


                //singleObject.isStaticTemp = 1;





                Segment[] arrayOfBoundsSegments00 = new Segment[4];
                arrayOfBoundsSegments00[0] = one1;
                arrayOfBoundsSegments00[1] = two1;
                arrayOfBoundsSegments00[2] = three1;
                arrayOfBoundsSegments00[3] = four1;

                for (int ii = 0; ii < arrayOfGameObjects.Length; ii++)
                {
                    if (arrayOfGameObjects[ii].currentObject != singleObject.currentObject)
                    {
                        if (arrayOfGameObjects[ii].velocity != new Vector2(0, 0) || singleObject.velocity != new Vector2(0, 0))
                        {
                            var one11 = new Segment();
                            one11.Start = arrayOfGameObjects[ii].currentObject.transform.TransformPoint(arrayOfGameObjects[ii].corner1);
                            one11.End = arrayOfGameObjects[ii].currentObject.transform.TransformPoint(arrayOfGameObjects[ii].corner2);

                            var two11 = new Segment();
                            two11.Start = arrayOfGameObjects[ii].currentObject.transform.TransformPoint(arrayOfGameObjects[ii].corner2);
                            two11.End = arrayOfGameObjects[ii].currentObject.transform.TransformPoint(arrayOfGameObjects[ii].corner3);

                            var three11 = new Segment();
                            three11.Start = arrayOfGameObjects[ii].currentObject.transform.TransformPoint(arrayOfGameObjects[ii].corner3);
                            three11.End = arrayOfGameObjects[ii].currentObject.transform.TransformPoint(arrayOfGameObjects[ii].corner4);

                            var four11 = new Segment();
                            four11.Start = arrayOfGameObjects[ii].currentObject.transform.TransformPoint(arrayOfGameObjects[ii].corner4);
                            four11.End = arrayOfGameObjects[ii].currentObject.transform.TransformPoint(arrayOfGameObjects[ii].corner1);


                            /*Segment one11 = new Segment();
                            one11.Start = singleObject.lerpPos.Value + new Vector2(arrayOfGameObjects[ii].corner1.x, arrayOfGameObjects[ii].corner1.y);
                            one11.End = singleObject.lerpPos.Value + new Vector2(arrayOfGameObjects[ii].corner2.x, arrayOfGameObjects[ii].corner2.y);

                            Segment two11 = new Segment();
                            two11.Start = singleObject.lerpPos.Value + new Vector2(arrayOfGameObjects[ii].corner2.x, arrayOfGameObjects[ii].corner2.y);
                            two11.End = singleObject.lerpPos.Value + new Vector2(arrayOfGameObjects[ii].corner3.x, arrayOfGameObjects[ii].corner1.y);

                            Segment three11 = new Segment();
                            three11.Start = singleObject.lerpPos.Value + new Vector2(arrayOfGameObjects[ii].corner3.x, arrayOfGameObjects[ii].corner3.y);
                            three11.End = singleObject.lerpPos.Value + new Vector2(arrayOfGameObjects[ii].corner4.x, arrayOfGameObjects[ii].corner4.y);

                            Segment four11 = new Segment();
                            four11.Start = singleObject.lerpPos.Value + new Vector2(arrayOfGameObjects[ii].corner4.x, arrayOfGameObjects[ii].corner4.y);
                            four11.End = singleObject.lerpPos.Value + new Vector2(arrayOfGameObjects[ii].corner1.x, arrayOfGameObjects[ii].corner1.y);
                            */

                            Segment[] arrayOfBoundsSegments01 = new Segment[4];
                            arrayOfBoundsSegments01[0] = one11;
                            arrayOfBoundsSegments01[1] = two11;
                            arrayOfBoundsSegments01[2] = three11;
                            arrayOfBoundsSegments01[3] = four11;

                            for (int c0 = 0; c0 < arrayOfBoundsSegments00.Length; c0++)
                            {
                                for (int c1 = 0; c1 < arrayOfBoundsSegments01.Length; c1++)
                                {
                                    intersector0 = SC_Segment.Intersects(arrayOfBoundsSegments01[c1], arrayOfBoundsSegments00[c0]);

                                    if (intersector0.HasValue && !double.IsNaN(intersector0.Value.X) && !double.IsNaN(intersector0.Value.Y))
                                    {

                                        object2 = arrayOfGameObjects[ii];
                                        //Vector2 currentCollisionPos = intersector0.Value;
                                        //arrayOfSomeVectorsIntersect.Add(currentCollisionPos);
                                        intersectPoint = intersector0.Value;
                                        return 1;
                                    }
                                }
                            }
                        }
                    }
                }
            }
            intersectPoint = null;
            return 0;
        }

        float Dot(float aX, float aY, float bX, float bY)
        {
            return (aX * bX) + (aY * bY);
        }
        public static void UpdateObject(Vector3 currentPosition, Vector3 currentVelocity, float gravity_negative, float deltaTime, SC_RigidInfo someObject)
        {
            someObject.position += new Vector2(currentVelocity.X, currentVelocity.Y) * deltaTime;
            someObject.velocity.Y += gravity_negative * deltaTime;

            /*someObject.angle += someObject.angularVelocity;
            someObject.angularVelocity += someObject.angularAcceleration;
            someObject.angularAcceleration = someObject.torque / someObject.inertia;
            someObject.torque = 0;*/


            //someObject.currentObject.transform.RotateAround(someObject.,);

            /*angle += angularVelocity;
            angularVelocity += angularAcceleration;
            angularAcceleration = torque / inertia;

            force.set(0, 0);
            torque = 0;*/
        }

        /*public void update()
        {
            position.add(velocity);
            velocity.add(acceleration);
            // force.add(View.gravity);
            force.scale(1 / mass);
            acceleration.set(force);

            angle += angularVelocity;
            angularVelocity += angularAcceleration;
            angularAcceleration = torque / inertia;

            force.set(0, 0);
            torque = 0;
        }*/

        bool NSEW(Vector2 a, Vector2 b, Vector2 c)
        {
            return ((b.X - a.X) * (c.Y - a.Y) - (b.Y - a.Y) * (c.X - a.X)) > 0;
        }

        /*private void OnDrawGizmos()
        {
            if (arrayOfGameObjects != null)
            {
                for (int i = 0; i < arrayOfGameObjects.Length; i++)
                {
                    if (arrayOfGameObjects[i] != null)
                    {
                        Gizmos.color = UnityEngine.Color.white;
                        Gizmos.DrawLine(arrayOfGameObjects[i].currentObject.transform.TransformPoint(arrayOfGameObjects[i].corner1), arrayOfGameObjects[i].currentObject.transform.TransformPoint(arrayOfGameObjects[i].corner2));
                        Gizmos.DrawLine(arrayOfGameObjects[i].currentObject.transform.TransformPoint(arrayOfGameObjects[i].corner2), arrayOfGameObjects[i].currentObject.transform.TransformPoint(arrayOfGameObjects[i].corner3));
                        Gizmos.DrawLine(arrayOfGameObjects[i].currentObject.transform.TransformPoint(arrayOfGameObjects[i].corner3), arrayOfGameObjects[i].currentObject.transform.TransformPoint(arrayOfGameObjects[i].corner4));
                        Gizmos.DrawLine(arrayOfGameObjects[i].currentObject.transform.TransformPoint(arrayOfGameObjects[i].corner4), arrayOfGameObjects[i].currentObject.transform.TransformPoint(arrayOfGameObjects[i].corner1));

                    }
                }
            }
        }*/
    }
}