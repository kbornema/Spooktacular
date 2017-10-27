using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

[System.Serializable]
public struct Vec2i : IEquatable<Vec2i>
{
    public static Vec2i one     { get { return new Vec2i(1, 1); } }
    public static Vec2i zero    { get { return new Vec2i(0, 0); } }
    public static Vec2i unitX   { get { return new Vec2i(1, 0); } }
    public static Vec2i unitY   { get { return new Vec2i(0, 1); } }

    public static Vec2i left    { get { return new Vec2i(-1,  0); } }
    public static Vec2i right   { get { return new Vec2i( 1,  0); } }
    public static Vec2i up      { get { return new Vec2i( 0,  1); } }
    public static Vec2i down    { get { return new Vec2i( 0, -1); } }

    public int x;
    public int y;

    public Vec2i(int x, int y)
    {
        this.x = x;
        this.y = y;
    }

    public Vec2i(int w)
        : this(w, w)
    {
    }

    public Vec2i(Vec2i other)
        : this(other.x, other.y)
    {
    }

    public Vec2i(Vector2 floatVec)
        : this((int)floatVec.x, (int)floatVec.y)
    {
    }

    public Vector2 ToVector2()
    {
        return new Vector2(this.x, this.y);
    }

    public override string ToString()
    {
        return string.Concat(x.ToString(), ", ", y.ToString());
    }
    
    public static bool operator !=(Vec2i left, Vec2i right)
    {
        return left.x != right.x || left.y != right.y;
    }

    public static bool operator ==(Vec2i left, Vec2i right)
    {
        return left.x == right.x && left.y == right.y;
    }
    
    public static Vec2i operator -(Vec2i left, Vec2i right)
    {
        return new Vec2i(left.x - right.x, left.y - right.y);
    }

    public static Vec2i operator +(Vec2i left, Vec2i right)
    {
        return new Vec2i(left.x + right.x, left.y + right.y);
    }

    public override bool Equals(object obj)
    {
        if(obj is Vec2i)  
            return this.Equals((Vec2i)obj);

        return false;
    }

    public bool Equals(Vec2i other)
    {
        return this.x == other.x && this.y == other.y;
    }

    public override int GetHashCode()
    {
        return base.GetHashCode();
    }

    public static Vec2i operator*(Vec2i lhs, int rhs)
    {
        return new Vec2i(lhs.x * rhs, lhs.y * rhs);
    }
    
    public static Vec2i operator *(int lhs, Vec2i rhs)
    {
        return rhs * lhs;
    }
	
	public static Vec2i operator /(Vec2i lhs, int rhs)
    {
        return new Vec2i(lhs.x / rhs, lhs.y / rhs);
    }

    public static Vec2i Random(int minX, int maxX, int minY, int maxY)
    {
        int x = UnityEngine.Random.Range(minX, maxX);
        int y = UnityEngine.Random.Range(minY, maxY);

        return new Vec2i(x, y);
    }

    public static int Dot(Vec2i lhs, Vec2i rhs)
    {
        return lhs.x * rhs.x + lhs.y * rhs.y;
    }

    public Vec2i LeftOrtho()
    {
        return new Vec2i(-this.y, this.x);
    }

    public Vec2i RightOrtho()
    {
        return new Vec2i(this.y, -this.x);
    }

    public bool IsOrthogonal(Vec2i other)
    {
        return Vec2i.Dot(this, other) == 0;
    }

    public bool IsOpposite(Vec2i other)
    {
        return (this * -1) == other;
    }

    public static void MinMax(Vec2i a, Vec2i b, out Vec2i min, out Vec2i max)
    {
        min.x = Mathf.Min(a.x, b.x);
        max.x = Mathf.Max(a.x, b.x);

        min.y = Mathf.Min(a.y, b.y);
        max.y = Mathf.Max(a.y, b.y);
    }
}
