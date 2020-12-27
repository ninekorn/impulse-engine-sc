using System.Collections;
using System.Collections.Generic;
using SharpDX;

namespace SCCoreSystems
{

    //found somewhere on internet on stackoverflow or the unity3d forums. i'll provide the reference when i find it again.
    public struct SC_Segment
    {
        public struct Segment
        {
            public Vector2 Start;
            public Vector2 End;
            public Vector2 Normal;
        }

        public static Vector2? Intersects(Segment AB, Segment CD)
        {
            double deltaACy = AB.Start.Y - CD.Start.Y;
            double deltaDCx = CD.End.X - CD.Start.X;
            double deltaACx = AB.Start.X - CD.Start.X;
            double deltaDCy = CD.End.Y - CD.Start.Y;
            double deltaBAx = AB.End.X - AB.Start.X;
            double deltaBAy = AB.End.Y - AB.Start.Y;

            double denominator = deltaBAx * deltaDCy - deltaBAy * deltaDCx;
            double numerator = deltaACy * deltaDCx - deltaACx * deltaDCy;

            if (denominator == 0)
            {
                if (numerator == 0)
                {
                    // collinear. Potentially infinite intersection points.
                    // Check and return one of them.
                    if (AB.Start.X >= CD.Start.X && AB.Start.X <= CD.End.X)
                    {
                        return AB.Start;
                    }
                    else if (CD.Start.X >= AB.Start.X && CD.Start.X <= AB.End.X)
                    {
                        return CD.Start;
                    }
                    else
                    {
                        return null;
                    }
                }
                else
                { // parallel
                    return null;
                }
            }

            double r = numerator / denominator;
            if (r < 0 || r > 1)
            {
                return null;
            }

            double s = (deltaACy * deltaBAx - deltaACx * deltaBAy) / denominator;
            if (s < 0 || s > 1)
            {
                return null;
            }

            return new Vector2((float)(AB.Start.X + r * deltaBAx), (float)(AB.Start.Y + r * deltaBAy));
        }
    }
}







//https://www.gamedev.net/forums/topic/545650-c-collision-of-two-aabbs--how-do-i-find-the-normal-of-the-collision-point/
/*bool axisIntersect(int axis, const AABB& a, const AABB& b, float& mtd, int& mtd_axis)
{
    // the absolute intersect intervals on the 'left' and 'right'
    float d0 = (b.max.coord(axis) - a.min.coord(axis));
    float d1 = (a.max.coord(axis) - b.min.coord(axis));

    // the signed minimum intersection between 'left' and 'right'
    // if the intervals are negative, boxes dont intersect
    if (d0 < 0.0f || d1 < 0.0f) return false;
    float d = (d0 < d1) ? -d0 : d1;

    // the signed minimum intersection between all three axes
    if ((mtd_axis == -1) || (fabs(d) < fabs(mtd)))
    {
        mtd = d;
        mtd_axis = axis;
    }
    return true;
}

bool intersect(const AABB& a, const AABB& b, Vector& mtd)
{
    // the mtd found between the three axes
    float mtd_val = 0.0f;
    int mtd_axis = -1;

    // test x, y, z axes
    if (!axisIntersect(0, a, b, mtd_val, mtd_axis)) return false;
    if (!axisIntersect(1, a, b, mtd_val, mtd_axis)) return false;
    if (!axisIntersect(2, a, b, mtd_val, mtd_axis)) return false;

    // sanity check
    if (mtd_axis < 0)
    {
        assert(!"humbug. we should havea valid mtd here.");
        return false;
    }

    // final result. return the 'push' vector, in magnitude and direction.
    mtd = Vector(0, 0, 0);
    mtd.coord(mtd_axis) = mtd_val;
    return true;
}*/
