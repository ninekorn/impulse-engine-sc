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
    public class Collisions//: Manifold
    {
        Action<object> detectCallback;
        public Collisions instance;

        //public static CollisionCircleCircle colHandleCC = new CollisionCircleCircle();
        //public static CollisionPolygonCircle colHandlePC = new CollisionPolygonCircle();
        //public static CollisionCirclePolygon colHandleCP = new CollisionCirclePolygon();
        //public static CollisionPolygonPolygon colHandlePP = new CollisionPolygonPolygon();



        public Collisions()
        {
            //instance = this;
            //detectCallback = new Action<object>(DetectCallback);
        }

        //public CollisionCircleCircle colCircCirc;


        //public virt void handleCollision(Manifold m, Body a, Body b);


 

        /*public interface CollisionCallback
        {
            void handleCollision(Manifold m, Body a, Body b);
        }*/

        /*private void DetectCallback(object obj)
        {
            //BroadphasePair pair = obj as BroadphasePair;
            //base.Detect(pair.Entity1, pair.Entity2);
            //BroadphasePair.Pool.GiveBack(pair);
        }*/


        //public static CollisionCircleCircle instanceCircCirc = new CollisionCircleCircle();


        //CollisionCallback[][] obj = { new CollisionCircleCircle() };


        /*public static CollisionCallback[][] dispatch =
        {
            new
            {
                CollisionPolygonPolygon.instance,
                CollisionPolygonPolygon.instance,
            }
        };*/



        //CollisionCircleCircle,
        //CollisionCirclePolygon



        //instanceCircCirc,


        /*new
        {
            CollisionCircleCircle.instance,
        },
        new
        {
            CollisionPolygonCircle.instance,
            CollisionPolygonPolygon.instance
        }*/

        /*CollisionCircleCircle.instance,
        CollisionCirclePolygon.instance,
        CollisionPolygonCircle.instance,
        CollisionPolygonPolygon.instance*/




        /*new {
            CollisionCircleCircle.instance, CollisionCirclePolygon.instance },

        {
            CollisionPolygonCircle.instance, CollisionPolygonPolygon.instance }*/

        //new CollisionCircleCircle(),

        /*new
        {
            CollisionCircleCircle.instance, CollisionCirclePolygon.instance
        },*/

        /*new
        {
            CollisionCircleCircle.instance,CollisionCirclePolygon.instance
        }
        new
        {
            CollisionCircleCircle.instance,
            CollisionCirclePolygon.instance
        },
        new
        {
            CollisionPolygonCircle.instance,
            CollisionPolygonPolygon.instance
        }*/
    };
}
