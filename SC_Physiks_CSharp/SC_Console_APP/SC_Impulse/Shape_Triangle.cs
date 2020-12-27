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
using SC_Console_APP;

namespace SCCoreSystems
{

    public abstract class Shape_Triangle
{

    public enum Type
    {
        Circle, Poly, Count
    }

    public Body body;
    public float radius;
    public Mat2 u = new Mat2();
    public DModelClass_Triangle gameObject;
    public SC_RigidInfo rigidInfo;
    public Mat2 orU = new Mat2();
    public Mat2 lU = new Mat2();

    public DModelClassCircle gameObjectCirc;

    public Shape_Triangle()
    {

    }

    public abstract Shape clone();

    public abstract void initialize();

    public abstract void computeMass(float density);

    public abstract void setOrient(float radians);

    public abstract Type getType();

    public abstract void setOriginOrient(float radians);
    public abstract void setLastOrient(float radians);
}

}