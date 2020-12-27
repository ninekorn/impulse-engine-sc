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

    public class Mat2
    {

        private float M00, M01;
        private float M10, M11;



        public float m00
        {
            get { return M00; }
            set { M00 = value; }
        }

        public float m01
        {
            get { return M01; }
            set { M01 = value; }
        }



        public float m10
        {
            get { return M10; }
            set { M10 = value; }
        }

        public float m11
        {
            get { return M11; }
            set { M11 = value; }
        }





        public Mat2()
        {
        }

        public Mat2(float radians)
        {
            set(radians);
        }

        public Mat2(float a, float b, float c, float d)
        {
            set(a, b, c, d);
        }

        /**
         * Sets this matrix to a rotation matrix with the given radians.
         */
        public void set(float radians)
        {
            float c = (float)Math.Cos(radians);
            float s = (float)Math.Sin(radians);

            M00 = c;
            M01 = -s;
            M10 = s;
            M11 = c;
        }

        /**
         * Sets the values of this matrix.
         */
        public void set(float a, float b, float c, float d)
        {
            M00 = a;
            M01 = b;
            M10 = c;
            M11 = d;
        }

        /**
         * Sets this matrix to have the same values as the given matrix.
         */
        public void set(Mat2 m)
        {
            M00 = m.M00;
            M01 = m.M01;
            M10 = m.M10;
            M11 = m.M11;
        }

        /**
         * Sets the values of this matrix to their absolute value.
         */
        public void absi()
        {
            abs(this);
        }

        /**
         * Returns a new matrix that is the absolute value of this matrix.
         */
        public Mat2 abs()
        {
            return abs(new Mat2());
        }

        /**
         * Sets out to the absolute value of this matrix.
         */
        public Mat2 abs(Mat2 outMat)
        {
            outMat.M00 = Math.Abs(M00);
            outMat.M01 = Math.Abs(M01);
            outMat.M10 = Math.Abs(M10);
            outMat.M11 = Math.Abs(M11);
            return outMat;
        }

        /**
         * Sets out to the x-axis (1st column) of this matrix.
         */
        public Vec2 getAxisX(Vec2 outVec)
        {
            outVec.x = M00;
            outVec.y = M10;
            return outVec;
        }

        /**
         * Returns a new vector that is the x-axis (1st column) of this matrix.
         */
        public Vec2 getAxisX()
        {
            return getAxisX(new Vec2());
        }

        /**
         * Sets out to the y-axis (2nd column) of this matrix.
         */
        public Vec2 getAxisY(Vec2 outVec)
        {
            outVec.x = M01;
            outVec.y = M11;
            return outVec;
        }

        /**
         * Returns a new vector that is the y-axis (2nd column) of this matrix.
         */
        public Vec2 getAxisY()
        {
            return getAxisY(new Vec2());
        }

        /**
         * Sets the matrix to it's transpose.
         */
        public void transposei()
        {
            float t = M01;
            M01 = M10;
            M10 = t;
        }

        /**
         * Sets out to the transpose of this matrix.
         */
        public Mat2 transpose(Mat2 outMat)
        {
            outMat.M00 = M00;
            outMat.M01 = M10;
            outMat.M10 = M01;
            outMat.M11 = M11;
            return outMat;
        }

        /**
         * Returns a new matrix that is the transpose of this matrix.
         */
        public Mat2 transpose()
        {
            return transpose(new Mat2());
        }

        /**
         * Transforms v by this matrix.
         */
        public Vec2 muli(Vec2 v)
        {
            return mul(v.x, v.y, v);
        }

        /**
         * Sets out to the transformation of v by this matrix.
         */
        public Vec2 mul(Vec2 v, Vec2 outMat)
        {
            return mul(v.x, v.y, outMat);
        }

        /**
         * Returns a new vector that is the transformation of v by this matrix.
         */
        public Vec2 mul(Vec2 v)
        {
            return mul(v.x, v.y, new Vec2());
        }

        /**
         * Sets out the to transformation of {x,y} by this matrix.
         */
        public Vec2 mul(float x, float y, Vec2 outMat)
        {
            outMat.x = M00 * x + M01 * y;
            outMat.y = M10 * x + M11 * y;
            return outMat;
        }

        /**
         * Multiplies this matrix by x.
         */
        public void muli(Mat2 x)
        {
            set(
                M00 * x.M00 + M01 * x.M10,
                M00 * x.M01 + M01 * x.M11,
                M10 * x.M00 + M11 * x.M10,
                M10 * x.M01 + M11 * x.M11);
        }

        /**
         * Sets out to the multiplication of this matrix and x.
         */
        public Mat2 mul(Mat2 x, Mat2 outMat)
        {
            outMat.M00 = M00 * x.M00 + M01 * x.M10;
            outMat.M01 = M00 * x.M01 + M01 * x.M11;
            outMat.M10 = M10 * x.M00 + M11 * x.M10;
            outMat.M11 = M10 * x.M01 + M11 * x.M11;
            return outMat;
        }

        /**
         * Returns a new matrix that is the multiplication of this and x.
         */
        public Mat2 mul(Mat2 x)
        {
            return mul(x, new Mat2());
        }
    }

}