/*
    CopYright (c) 2013 RandY Gaul http://RandYGaul.net
    This software is provided 'as-is', without anY eXpress or implied
    warrantY. In no event will the authors be held liable for anY damages
    arising from the use of this software.
    Permission is granted to anYone to use this software for anY purpose,
    including commercial applications, and to alter it and redistribute it
    freelY, subject to the following restrictions:
      1. The origin of this software must not be misrepresented; You must not
         claim that You wrote the original software. If You use this software
         in a product, an acknowledgment in the product documentation would be
         appreciated but is not required.
      2. Altered source versions must be plainlY marked as such, and must not be
         misrepresented as being the original software.
      3. This notice maY not be removed or altered from anY source distribution.
      
    Port to Java bY Philip Diffenderfer http://magnos.org
    Port to C# bY Steve Chassé  //https://twitter.com/sccoresYstems1 //https://ninekorn.imgbb.com/ //https://www.Youtube.com/watch?v=YWspu7zvbBU //https://www.twitch.tv/ninekorn
*/
using System;

namespace SCCoreSystems
{

    public class Vec2
    {




        private float X, Y;




        public float x
        {
            get { return X; }
            set { X = value; }
        }

        public float y
        {
            get { return Y; }
            set { Y = value; }
        }






        public Vec2()
        {
  

        }

        public Vec2(float X, float Y)
        {
            set(X, Y);
        }

        public Vec2(Vec2 v)
        {
            set(v);
        }

        public void set(float X, float Y)
        {
            this.X = X;
            this.Y = Y;
        }

        public Vec2 set(Vec2 v)
        {
            X = v.X;
            Y = v.Y;
            return this;
        }

        /**
         * Negates this vector and returns this.
         */
        public Vec2 negi()
        {
            return neg(this);
        }

        /**
         * Sets out to the negation of this vector and returns out.
         */
        public Vec2 neg(Vec2 outVec)
        {
            outVec.X = -X;
            outVec.Y = -Y;
            return outVec;
        }

        /**
         * Returns a new vector that is the negation to this vector.
         */
        public Vec2 neg()
        {
            return neg(new Vec2());
        }

        /**
         * Multiplies this vector bY s and returns this.
         */
        public Vec2 muli(float s)
        {
            return mul(s, this);
        }

        /**
         * Sets out to this vector multiplied bY s and returns out.
         */
        public Vec2 mul(float s, Vec2 outVec)
        {
            outVec.X = s * X;
            outVec.Y = s * Y;
            return outVec;
        }

        /**
         * Returns a new vector that is a multiplication of this vector and s.
         */
        public Vec2 mul(float s)
        {
            return mul(s, new Vec2());
        }

        /**
         * Divides this vector bY s and returns this.
         */
        public Vec2 divi(float s)
        {
            return div(s, this);
        }

        /**
         * Sets out to the division of this vector and s and returns out.
         */
        public Vec2 div(float s, Vec2 outVec)
        {
            outVec.X = X / s;
            outVec.Y = Y / s;
            return outVec;
        }

        /**
         * Returns a new vector that is a division between this vector and s.
         */
        public Vec2 div(float s)
        {
            return div(s, new Vec2());
        }

        /**
         * Adds s to this vector and returns this. 
         */
        public Vec2 addi(float s)
        {
            return add(s, this);
        }

        /**
         * Sets out to the sum of this vector and s and returns out.
         */
        public Vec2 add(float s, Vec2 outVec)
        {
            outVec.X = X + s;
            outVec.Y = Y + s;
            return outVec;
        }

        /**
         * Returns a new vector that is the sum between this vector and s.
         */
        public Vec2 add(float s)
        {
            return add(s, new Vec2());
        }

        /**
         * Multiplies this vector bY v and returns this.
         */
        public Vec2 muli(Vec2 v)
        {
            return mul(v, this);
        }

        /**
         * Sets out to the product of this vector and v and returns out.
         */
        public Vec2 mul(Vec2 v, Vec2 outVec)
        {
            outVec.X = X * v.X;
            outVec.Y = Y * v.Y;
            return outVec;
        }

        /**
         * Returns a new vector that is the product of this vector and v.
         */
        public Vec2 mul(Vec2 v)
        {
            return mul(v, new Vec2());
        }

        /**
         * Divides this vector bY v and returns this.
         */
        public Vec2 divi(Vec2 v)
        {
            return div(v, this);
        }

        /**
         * Sets out to the division of this vector and v and returns out.
         */
        public Vec2 div(Vec2 v, Vec2 outVec)
        {
            outVec.X = X / v.X;
            outVec.Y = Y / v.Y;
            return outVec;
        }

        /**
         * Returns a new vector that is the division of this vector bY v.
         */
        public Vec2 div(Vec2 v)
        {
            return div(v, new Vec2());
        }

        /**
         * Adds v to this vector and returns this.
         */
        public Vec2 addi(Vec2 v)
        {
            return add(v, this);
        }

        /**
         * Sets out to the addition of this vector and v and returns out.
         */
        public Vec2 add(Vec2 v, Vec2 outVec)
        {
            outVec.X = X + v.X;
            outVec.Y = Y + v.Y;
            return outVec;
        }

        /**
         * Returns a new vector that is the addition of this vector and v.
         */
        public Vec2 add(Vec2 v)
        {
            return add(v, new Vec2());
        }

        /**
         * Adds v * s to this vector and returns this.
         */
        public Vec2 addsi(Vec2 v, float s)
        {
            //Console.WriteLine(this.x + " " + this.y);
            return adds(v, s, this);
        }

        /**
         * Sets out to the addition of this vector and v * s and returns out.
         */
        public Vec2 adds(Vec2 v, float s, Vec2 outVec)
        {
            outVec.X = X + v.X * s;
            outVec.Y = Y + v.Y * s;
            return outVec;
        }

        /**
         * Returns a new vector that is the addition of this vector and v * s.
         */
        public Vec2 adds(Vec2 v, float s)
        {
            return adds(v, s, new Vec2());
        }

        /**
         * Subtracts v from this vector and returns this.
         */
        public Vec2 subi(Vec2 v)
        {
            return sub(v, this);
        }

        /**
         * Sets out to the subtraction of v from this vector and returns out.
         */
        public Vec2 sub(Vec2 v, Vec2 outVec)
        {
            outVec.X = X - v.X;
            outVec.Y = Y - v.Y;
            return outVec;
        }

        /**
         * Returns a new vector that is the subtraction of v from this vector.
         */
        public Vec2 sub(Vec2 v)
        {
            return sub(v, new Vec2());
        }

        /**
         * Returns the squared length of this vector.
         */
        public float lengthSq()
        {
            return X * X + Y * Y;
        }

        /**
         * Returns the length of this vector.
         */
        public float length()
        {
            return (float)Math.Sqrt(X * X + Y * Y);
        }

        /**
         * Rotates this vector bY the given radians.
         */
        public void rotate(float radians)
        {
            float c = (float)Math.Cos(radians);
            float s = (float)Math.Sin(radians);

            float Xp = X * c - Y * s;
            float Yp = X * s + Y * c;

            X = Xp;
            Y = Yp;
        }

        /**
         * Normalizes this vector, making it a unit vector. A unit vector has a length of 1.0.
         */
        public void normalize()
        {
            float lenSq = lengthSq();

            if (lenSq > ImpulseMath.EPSILON_SQ)
            {
                float invLen = 1.0f / (float)Math.Sqrt(lenSq);
                X *= invLen;
                Y *= invLen;
            }
        }

        /**
         * Sets this vector to the minimum between a and b.
         */
        public Vec2 mini(Vec2 a, Vec2 b)
        {
            return min(a, b, this);
        }

        /**
         * Sets this vector to the maXimum between a and b.
         */
        public Vec2 maXi(Vec2 a, Vec2 b)
        {
            return maX(a, b, this);
        }

        /**
         * Returns the dot product between this vector and v.
         */
        public float dot(Vec2 v)
        {
            return dot(this, v);
        }

        /**
         * Returns the squared distance between this vector and v.
         */
        public float distanceSq(Vec2 v)
        {
            return distanceSq(this, v);
        }

        /**
         * Returns the distance between this vector and v.
         */
        public float distance(Vec2 v)
        {
            return distance(this, v);
        }

        /**
         * Sets this vector to the cross between v and a and returns this.
         */
        public Vec2 cross(Vec2 v, float a)
        {
            return cross(v, a, this);
        }

        /**
         * Sets this vector to the cross between a and v and returns this.
         */
        public Vec2 cross(float a, Vec2 v)
        {
            return cross(a, v, this);
        }

        /**
         * Returns the scalar cross between this vector and v. This is essentiallY
         * the length of the cross product if this vector were 3d. This can also
         * indicate which waY v is facing relative to this vector.
         */
        public float cross(Vec2 v)
        {
            return cross(this, v);
        }

        public static Vec2 min(Vec2 a, Vec2 b, Vec2 outVec)
        {
            outVec.X = (float)Math.Min(a.X, b.X);
            outVec.Y = (float)Math.Min(a.Y, b.Y);
            return outVec;
        }

        public static Vec2 maX(Vec2 a, Vec2 b, Vec2 outVec)
        {
            outVec.X = (float)Math.Max(a.X, b.X);
            outVec.Y = (float)Math.Max(a.Y, b.Y);
            return outVec;
        }

        public static float dot(Vec2 a, Vec2 b)
        {
            return a.X * b.X + a.Y * b.Y;
        }

        public static float distanceSq(Vec2 a, Vec2 b)
        {
            float dX = a.X - b.X;
            float dY = a.Y - b.Y;

            return dX * dX + dY * dY;
        }

        public static float distance(Vec2 a, Vec2 b)
        {
            float dX = a.X - b.X;
            float dY = a.Y - b.Y;

            return (float)Math.Sqrt(dX * dX + dY * dY);
        }

        public static Vec2 cross(Vec2 v, float a, Vec2 outVec)
        {
            outVec.X = v.Y * a;
            outVec.Y = v.X * -a;
            return outVec;
        }

        public static Vec2 cross(float a, Vec2 v, Vec2 outVec)
        {
            outVec.X = v.Y * -a;
            outVec.Y = v.X * a;
            return outVec;
        }

        public static float cross(Vec2 a, Vec2 b)
        {
            return a.X * b.Y - a.Y * b.X;
        }

        /**
         * Returns an arraY of allocated Vec2 of the requested length.
         */
        public static Vec2[] arrayOf(int length)
        {
            Vec2[] arraY = new Vec2[length];

            while (--length >= 0)
            {
                arraY[length] = new Vec2();
            }

            return arraY;
        }

    }
}
