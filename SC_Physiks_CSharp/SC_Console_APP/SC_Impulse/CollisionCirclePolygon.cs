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

namespace SCCoreSystems
{

    public class CollisionCirclePolygon : CollisionCallback//CollisionCallback //implements CollisionCallback
    {
        public static CollisionCirclePolygon instance;// = new CollisionCirclePolygon();

        public CollisionCirclePolygon()
        {
            instance = this;
        }



        public void handleCollision(Manifold m, Body a, Body b)
        {
            Circle A = (Circle)a.shape;
            Polygon B = (Polygon)b.shape;

            m.contactCount = 0;

            Vec2 center = B.u.transpose().muli(a.position.sub(b.position));

            float separation = -float.MaxValue;
            int faceNormal = 0;
            for (int i = 0; i < B.vertexCount; ++i)
            {
                float s = Vec2.dot(B.normals[i], center.sub(B.vertices[i]));

                if (s > A.radius)
                {
                    return;
                }

                if (s > separation)
                {
                    separation = s;
                    faceNormal = i;
                }
            }

            // Grab face's vertices
            Vec2 v1 = B.vertices[faceNormal];
            int i2 = faceNormal + 1 < B.vertexCount ? faceNormal + 1 : 0;
            Vec2 v2 = B.vertices[i2];

            // Check to see if center is within polygon
            if (separation < ImpulseMath.EPSILON)
            {
                m.contactCount = 1;
                B.u.mul(B.normals[faceNormal], m.normal).negi();
                m.contacts[0].set(m.normal).muli(A.radius).addi(a.position);
                m.penetration = A.radius;
                return;
            }

            float dot1 = Vec2.dot(center.sub(v1), v2.sub(v1));
            float dot2 = Vec2.dot(center.sub(v2), v1.sub(v2));
            m.penetration = A.radius - separation;
            //m.penetration = (A.radius - Vec2.distance(v2, center));
            // Closest to v1
            if (dot1 <= 0.0f)
            {
                if (Vec2.distanceSq(center, v1) > A.radius * A.radius)
                {
                    return;
                }

                m.contactCount = 1;
                B.u.muli(m.normal.set(v1).subi(center)).normalize();
                B.u.mul(v1, m.contacts[0]).addi(b.position);
            }
            //m.penetration = (A.radius - Vector2.distance(v2, center));
            // Closest to v2
            else if (dot2 <= 0.0f)
            {
                if (Vec2.distanceSq(center, v2) > A.radius * A.radius)
                {
                    return;
                }

                m.contactCount = 1;
                B.u.muli(m.normal.set(v2).subi(center)).normalize();
                B.u.mul(v2, m.contacts[0]).addi(b.position);
            }

            // Closest to face
            else
            {
                Vec2 n = B.normals[faceNormal];

                if (Vec2.dot(center.sub(v1), n) > A.radius)
                {
                    return;
                }
                m.contactCount = 1;
                B.u.mul(n, m.normal).negi();
                m.contacts[0].set(a.position).addsi(m.normal, A.radius);
            }
        }
    }
}
