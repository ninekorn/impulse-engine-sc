using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;

using SharpDX;

using Segment = SCCoreSystems.SC_Segment.Segment;


namespace SCCoreSystems
{

    public class SC_RigidInfo
    {
        public Segment[] edgeSegments;
        public Vector2[] vertices;
        public Vector2 position;
        public Vector2 velocity;

        public Vector2 boundsLowX;
        public Vector2 boundsHighX;
        public Vector2 boundsLowY;
        public Vector2 boundsHighY;

        public Vector2 corner1;
        public Vector2 corner2;
        public Vector2 corner3;
        public Vector2 corner4;

        public Vector2? collideNorm;
        public Vector2? collidePoint;
        public Vector2? extendedVelocity;
        public Vector2? lerpPos;
        public Vector2 lastFramePos;
        public Vector2 lastFrameVelo;
        public Vector2 lastLerpPos;

        public Vector2 angularVel;

        public DModelClass_Triangle Model_Triangle;
        public DModel Model;
        public Body body;

        public Vector2 supposedPos;
        public Vector2 supposedVelo;


        public float orientation;

        public float mass;
        public float restitution;
        public float angularVelocity;
        public float angularAcceleration;
        public float torque;
        public float angle;
        public float inertia;
        public float timeToHit;

        public float airFriction;
        public float rotZ;
        public float lastFrameAngle;

        public int lerp;
        public int indexToCollideWith;
        public int mainIndex;
        public int isStaticTemp;
        public int isStatic;
        public float invMass;
        public float invInertia;

        public Matrix? someRotMatrix;
        public Matrix? someWorldMatrix;


        /*public Vector2[] vertices;
        public Segment[] edgeSegments;

        public GameObject currentObject;
        public Vector2 position;
        public int height;
        public int width;
        public Vector2 velocity;
        public float mass;
        public Vector3 boundsLowX;
        public Vector3 boundsHighX;
        public Vector3 boundsLowY;
        public Vector3 boundsHighY;

        public Vector3 corner1;
        public Vector3 corner2;
        public Vector3 corner3;
        public Vector3 corner4;

        public Vector2 lastFramePos;

        public Vector3 boundMinX;
        public Vector3 boundMaxX;
        public int lerp;
        public int lerpTwo;
        public Vector2? lerpPos;
        public float timeToHit;

        public int isStatic;

        public int isBusy;
        public int indexToCollideWith;
        public int indexToCollideWithTwo;

        public Segment? seg;

        public Vector2? collideNorm;
        public int applyImpulse;
        public int mainIndex;
        public Stopwatch fallingWatch;
        public int hasWatchStarted;
        public Vector2? collidePoint;
        public Vector2? otherCollidePoint;

        public Vector2 lastFrameVelo;
        public float lastFrameTime;
        public Vector2? extendedVelocity;
        public Vector2? lastImpulseVelocity;
        public Vector2 originalPosition;
        public int hasCollided = 0;

        public Vector2? perpRight;
        public Vector2? perpLeft;

        public int hasWork = 0;
        public Vector2 lastLerpPos;

        public Vector2 positionAtLerpStart;
        public Vector2 lastPositionAtLerpStart;
        public float lastTimeToHit;
        public float restitution;
        public int isStaticTemp;
        public Vector2? lastImpulse;

        public Segment colSegment;

        public int isColliding;

        public float angularVelocity;
        public float angularAcceleration;
        public float torque;
        public float angle;
        public float inertia;

        public Vector2 impulse;*/
    }
}



