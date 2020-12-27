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


using System.Collections;
using System.Collections.Generic;
using System;

namespace SCCoreSystems
{

    public class ImpulseMath
    {
        public static System.Random rand = new System.Random();

        public static float PI = (float)Math.PI;
        public static double EPSILON = 0.0001f;// Double.Epsilon;//0.0001f;// Mathf.Epsilon;
        public static double EPSILON_SQ = EPSILON * EPSILON;
        public static float BIAS_RELATIVE = 0.95f;
        public static float BIAS_ABSOLUTE = 0.01f;
        public static float DT = 1.0f / 60.0f;
        public static Vec2 GRAVITY = new Vec2(0.0f, -9.81f);
        public static double RESTING = GRAVITY.mul(1.0f / 60.0f).lengthSq() + EPSILON;
        public static float PENETRATION_ALLOWANCE = 0.05f; //0.5f
        public static float PENETRATION_CORRECTION = 0.4f; //0.4f

        /*public static final float PI = (float)StrictMath.PI;
        public static final float EPSILON = 0.0001f;
        public static final float EPSILON_SQ = EPSILON * EPSILON;
        public static final float BIAS_RELATIVE = 0.95f;
        public static final float BIAS_ABSOLUTE = 0.01f;
        public static final float DT = 1.0f / 60.0f;
        public static final Vec2 GRAVITY = new Vec2( 0.0f, 50.0f );
        public static final float RESTING = GRAVITY.mul(DT).lengthSq() + EPSILON;
        public static final float PENETRATION_ALLOWANCE = 0.05f;
        public static final float PENETRATION_CORRETION = 0.4f;*/

        public static bool equal(float a, float b)
        {
            return Math.Abs(a - b) <= EPSILON;
        }

        public static float clamp(float min, float max, float a)
        {
            return (a < min ? min : (a > max ? max : a));
        }

        /*public static int round(float a)
        {
            return (int)(a + 0.5f);
        }*/

        public static float random(float min, float max)
        {
            return (float)((max - min) * GetRandomNumber(0, 1.0f) + min);
        }

        public static int random(int min, int max)
        {
            return (int)((max - min + 1) * GetRandomNumber(0, 1.0f) + min);
        }

        public static bool gt(float a, float b)
        {
            return a >= b * BIAS_RELATIVE + a * BIAS_ABSOLUTE;
        }

        public static double GetRandomNumber(double minimum, double maximum)
        {
            //rand = new Random();

            double randomNum = rand.NextDouble() * (maximum - minimum) + minimum;

            //Console.WriteLine(randomNum);
            return randomNum;// rand.NextDouble() * (maximum - minimum) + minimum;
        }

    }
}