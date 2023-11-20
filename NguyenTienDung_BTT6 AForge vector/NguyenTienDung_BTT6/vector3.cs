using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NguyenTienDung_BTT6
{

    public class Vector3D
    {
        /// <summary>
        /// Thuộc tính độ dài của vector
        /// </summary>
        public float Length
        {
            get
            {
                return this.Norm;
            }
        }
        /// <summary>
        /// Hàm trừ 2 vector
        /// </summary>
        public static Vector3D Tru(Vector3D v1,Vector3D v2)
        {
            return v2 - v1;
        }

        /// <summary>
        /// Phương thức lấy độ dài của vector
        /// </summary>
        public float getLength()
        {
            return this.Length;
        }
        /// <summary>
        /// Phương thức tính góc giữa 2 Vector
        /// </summary>
        public static double getAngle(Vector3D v1, Vector3D v2)
        {
            double tich = v1 * v2;
            double tichDoDai = v1.Length * v2.Length;
            double t = Math.Round(tich / tichDoDai, 9);
            return Math.Acos(t);
            //return (float)Math.Acos((Vector3D.Dot(v1, v2)) / (v1.Norm * v2.Norm));
        }

        /// <summary>
        /// Xây dựng phương thức tìm véc tơ phân giác của 2 véc tơ a và b, biết công thức là c = a |b| + b |a|. 
        /// </summary>

        public static Vector3D PhanGiac(Vector3D vector1, Vector3D vector2)
        {
            return vector1 * vector2.Length + vector2 * vector1.Length;
        }


        //
        // Summary:
        //     X coordinate of the vector.
        public float X;

        //
        // Summary:
        //     Y coordinate of the vector.
        public float Y;

        //
        // Summary:
        //     Z coordinate of the vector.
        public float Z;

        //
        // Summary:
        //     Returns maximum value of the vector.
        //
        // Remarks:
        //     Returns maximum value of all 3 vector's coordinates.
        public float Max
        {
            get
            {
                if (!(X > Y))
                {
                    if (!(Y > Z))
                    {
                        return Z;
                    }

                    return Y;
                }

                if (!(X > Z))
                {
                    return Z;
                }

                return X;
            }
        }

        //
        // Summary:
        //     Returns minimum value of the vector.
        //
        // Remarks:
        //     Returns minimum value of all 3 vector's coordinates.
        public float Min
        {
            get
            {
                if (!(X < Y))
                {
                    if (!(Y < Z))
                    {
                        return Z;
                    }

                    return Y;
                }

                if (!(X < Z))
                {
                    return Z;
                }

                return X;
            }
        }

        //
        // Summary:
        //     Returns index of the coordinate with maximum value.
        //
        // Remarks:
        //     Returns index of the coordinate, which has the maximum value - 0 for X, 1 for
        //     Y or 2 for Z.
        //     If there are multiple coordinates which have the same maximum value, the property
        //     returns smallest index.
        public int MaxIndex
        {
            get
            {
                if (!(X >= Y))
                {
                    if (!(Y >= Z))
                    {
                        return 2;
                    }

                    return 1;
                }

                if (!(X >= Z))
                {
                    return 2;
                }

                return 0;
            }
        }

        //
        // Summary:
        //     Returns index of the coordinate with minimum value.
        //
        // Remarks:
        //     Returns index of the coordinate, which has the minimum value - 0 for X, 1 for
        //     Y or 2 for Z.
        //     If there are multiple coordinates which have the same minimum value, the property
        //     returns smallest index.
        public int MinIndex
        {
            get
            {
                if (!(X <= Y))
                {
                    if (!(Y <= Z))
                    {
                        return 2;
                    }

                    return 1;
                }

                if (!(X <= Z))
                {
                    return 2;
                }

                return 0;
            }
        }

        //
        // Summary:
        //     Returns vector's norm.
        //
        // Remarks:
        //     Returns Euclidean norm of the vector, which is a square root of the sum: X2+Y2+Z2.
        public float Norm => (float)System.Math.Sqrt(X * X + Y * Y + Z * Z);

        //
        // Summary:
        //     Returns square of the vector's norm.
        //
        // Remarks:
        //     Return X2+Y2+Z2, which is a square of vector's norm or a dot product of this
        //     vector with itself.
        public float Square => X * X + Y * Y + Z * Z;

        //
        // Summary:
        //     Initializes a new instance of the AForge.Math.Vector3D structure.
        //
        // Parameters:
        //   x:
        //     X coordinate of the vector.
        //
        //   y:
        //     Y coordinate of the vector.
        //
        //   z:
        //     Z coordinate of the vector.
        public Vector3D(float x, float y, float z)
        {
            X = x;
            Y = y;
            Z = z;
        }

        //
        // Summary:
        //     Initializes a new instance of the AForge.Math.Vector3D structure.
        //
        // Parameters:
        //   value:
        //     Value, which is set to all 3 coordinates of the vector.
        public Vector3D(float value)
        {
            X = (Y = (Z = value));
        }

        //
        // Summary:
        //     Returns a string representation of this object.
        //
        // Returns:
        //     A string representation of this object.
        public override string ToString()
        {
            return string.Format(CultureInfo.InvariantCulture, "{0}, {1}, {2}", new object[3] { X, Y, Z });
        }

        //
        // Summary:
        //     Returns array representation of the vector.
        //
        // Returns:
        //     Array with 3 values containing X/Y/Z coordinates.
        public float[] ToArray()
        {
            return new float[3] { X, Y, Z };
        }

        //
        // Summary:
        //     Adds corresponding coordinates of two vectors.
        //
        // Parameters:
        //   vector1:
        //     The vector to add to.
        //
        //   vector2:
        //     The vector to add to the first vector.
        //
        // Returns:
        //     Returns a vector which coordinates are equal to sum of corresponding coordinates
        //     of the two specified vectors.
        public static Vector3D operator +(Vector3D vector1, Vector3D vector2)
        {
            return new Vector3D(vector1.X + vector2.X, vector1.Y + vector2.Y, vector1.Z + vector2.Z);
        }

        //
        // Summary:
        //     Adds corresponding coordinates of two vectors.
        //
        // Parameters:
        //   vector1:
        //     The vector to add to.
        //
        //   vector2:
        //     The vector to add to the first vector.
        //
        // Returns:
        //     Returns a vector which coordinates are equal to sum of corresponding coordinates
        //     of the two specified vectors.
        public static Vector3D Add(Vector3D vector1, Vector3D vector2)
        {
            return vector1 + vector2;
        }

        //
        // Summary:
        //     Adds a value to all coordinates of the specified vector.
        //
        // Parameters:
        //   vector:
        //     Vector to add the specified value to.
        //
        //   value:
        //     Value to add to all coordinates of the vector.
        //
        // Returns:
        //     Returns new vector with all coordinates increased by the specified value.
        public static Vector3D operator +(Vector3D vector, float value)
        {
            return new Vector3D(vector.X + value, vector.Y + value, vector.Z + value);
        }

        //
        // Summary:
        //     Adds a value to all coordinates of the specified vector.
        //
        // Parameters:
        //   vector:
        //     Vector to add the specified value to.
        //
        //   value:
        //     Value to add to all coordinates of the vector.
        //
        // Returns:
        //     Returns new vector with all coordinates increased by the specified value.
        public static Vector3D Add(Vector3D vector, float value)
        {
            return vector + value;
        }

        //
        // Summary:
        //     Subtracts corresponding coordinates of two vectors.
        //
        // Parameters:
        //   vector1:
        //     The vector to subtract from.
        //
        //   vector2:
        //     The vector to subtract from the first vector.
        //
        // Returns:
        //     Returns a vector which coordinates are equal to difference of corresponding coordinates
        //     of the two specified vectors.
        public static Vector3D operator -(Vector3D vector1, Vector3D vector2)
        {
            return new Vector3D(vector1.X - vector2.X, vector1.Y - vector2.Y, vector1.Z - vector2.Z);
        }

        //
        // Summary:
        //     Subtracts corresponding coordinates of two vectors.
        //
        // Parameters:
        //   vector1:
        //     The vector to subtract from.
        //
        //   vector2:
        //     The vector to subtract from the first vector.
        //
        // Returns:
        //     Returns a vector which coordinates are equal to difference of corresponding coordinates
        //     of the two specified vectors.
        public static Vector3D Subtract(Vector3D vector1, Vector3D vector2)
        {
            return vector1 - vector2;
        }

        //
        // Summary:
        //     Subtracts a value from all coordinates of the specified vector.
        //
        // Parameters:
        //   vector:
        //     Vector to subtract the specified value from.
        //
        //   value:
        //     Value to subtract from all coordinates of the vector.
        //
        // Returns:
        //     Returns new vector with all coordinates decreased by the specified value.
        public static Vector3D operator -(Vector3D vector, float value)
        {
            return new Vector3D(vector.X - value, vector.Y - value, vector.Z - value);
        }

        //
        // Summary:
        //     Subtracts a value from all coordinates of the specified vector.
        //
        // Parameters:
        //   vector:
        //     Vector to subtract the specified value from.
        //
        //   value:
        //     Value to subtract from all coordinates of the vector.
        //
        // Returns:
        //     Returns new vector with all coordinates decreased by the specified value.
        public static Vector3D Subtract(Vector3D vector, float value)
        {
            return vector - value;
        }

        //
        // Summary:
        //     Multiplies corresponding coordinates of two vectors.
        //
        // Parameters:
        //   vector1:
        //     The first vector to multiply.
        //
        //   vector2:
        //     The second vector to multiply.
        //
        // Returns:
        //     Returns a vector which coordinates are equal to multiplication of corresponding
        //     coordinates of the two specified vectors.
        public static float operator *(Vector3D vector1, Vector3D vector2)
        {
            return Dot(vector1,vector2);
            //return new Vector3D(vector1.X * vector2.X, vector1.Y * vector2.Y, vector1.Z * vector2.Z);
        }

        //
        // Summary:
        //     Multiplies corresponding coordinates of two vectors.
        //
        // Parameters:
        //   vector1:
        //     The first vector to multiply.
        //
        //   vector2:
        //     The second vector to multiply.
        //
        // Returns:
        //     Returns a vector which coordinates are equal to multiplication of corresponding
        //     coordinates of the two specified vectors.
        public static Vector3D Multiply(Vector3D vector1, Vector3D vector2)
        {
            return new Vector3D(vector1.X * vector2.X, vector1.Y * vector2.Y, vector1.Z * vector2.Z);
        }

        //
        // Summary:
        //     Multiplies coordinates of the specified vector by the specified factor.
        //
        // Parameters:
        //   vector:
        //     Vector to multiply coordinates of.
        //
        //   factor:
        //     Factor to multiple coordinates of the specified vector by.
        //
        // Returns:
        //     Returns new vector with all coordinates multiplied by the specified factor.
        public static Vector3D operator *(Vector3D vector, float factor)
        {
            return new Vector3D(vector.X * factor, vector.Y * factor, vector.Z * factor);
        }

        //
        // Summary:
        //     Multiplies coordinates of the specified vector by the specified factor.
        //
        // Parameters:
        //   vector:
        //     Vector to multiply coordinates of.
        //
        //   factor:
        //     Factor to multiple coordinates of the specified vector by.
        //
        // Returns:
        //     Returns new vector with all coordinates multiplied by the specified factor.
        public static Vector3D Multiply(Vector3D vector, float factor)
        {
            return vector * factor;
        }

        //
        // Summary:
        //     Divides corresponding coordinates of two vectors.
        //
        // Parameters:
        //   vector1:
        //     The first vector to divide.
        //
        //   vector2:
        //     The second vector to devide.
        //
        // Returns:
        //     Returns a vector which coordinates are equal to coordinates of the first vector
        //     divided by corresponding coordinates of the second vector.
        public static Vector3D operator /(Vector3D vector1, Vector3D vector2)
        {
            return new Vector3D(vector1.X / vector2.X, vector1.Y / vector2.Y, vector1.Z / vector2.Z);
        }

        //
        // Summary:
        //     Divides corresponding coordinates of two vectors.
        //
        // Parameters:
        //   vector1:
        //     The first vector to divide.
        //
        //   vector2:
        //     The second vector to devide.
        //
        // Returns:
        //     Returns a vector which coordinates are equal to coordinates of the first vector
        //     divided by corresponding coordinates of the second vector.
        public static Vector3D Divide(Vector3D vector1, Vector3D vector2)
        {
            return vector1 / vector2;
        }

        //
        // Summary:
        //     Divides coordinates of the specified vector by the specified factor.
        //
        // Parameters:
        //   vector:
        //     Vector to divide coordinates of.
        //
        //   factor:
        //     Factor to divide coordinates of the specified vector by.
        //
        // Returns:
        //     Returns new vector with all coordinates divided by the specified factor.
        public static Vector3D operator /(Vector3D vector, float factor)
        {
            return new Vector3D(vector.X / factor, vector.Y / factor, vector.Z / factor);
        }

        //
        // Summary:
        //     Divides coordinates of the specified vector by the specified factor.
        //
        // Parameters:
        //   vector:
        //     Vector to divide coordinates of.
        //
        //   factor:
        //     Factor to divide coordinates of the specified vector by.
        //
        // Returns:
        //     Returns new vector with all coordinates divided by the specified factor.
        public static Vector3D Divide(Vector3D vector, float factor)
        {
            return vector / factor;
        }

        //
        // Summary:
        //     Tests whether two specified vectors are equal.
        //
        // Parameters:
        //   vector1:
        //     The left-hand vector.
        //
        //   vector2:
        //     The right-hand vector.
        //
        // Returns:
        //     Returns true if the two vectors are equal or false otherwise.
        public static bool operator ==(Vector3D vector1, Vector3D vector2)
        {
            if (vector1.X == vector2.X && vector1.Y == vector2.Y)
            {
                return vector1.Z == vector2.Z;
            }

            return false;
        }

        //
        // Summary:
        //     Tests whether two specified vectors are not equal.
        //
        // Parameters:
        //   vector1:
        //     The left-hand vector.
        //
        //   vector2:
        //     The right-hand vector.
        //
        // Returns:
        //     Returns true if the two vectors are not equal or false otherwise.
        public static bool operator !=(Vector3D vector1, Vector3D vector2)
        {
            if (vector1.X == vector2.X && vector1.Y == vector2.Y)
            {
                return vector1.Z != vector2.Z;
            }

            return true;
        }

        //
        // Summary:
        //     Tests whether the vector equals to the specified one.
        //
        // Parameters:
        //   vector:
        //     The vector to test equality with.
        //
        // Returns:
        //     Returns true if the two vectors are equal or false otherwise.
        public bool Equals(Vector3D vector)
        {
            if (vector.X == X && vector.Y == Y)
            {
                return vector.Z == Z;
            }

            return false;
        }

        //
        // Summary:
        //     Tests whether the vector equals to the specified object.
        //
        // Parameters:
        //   obj:
        //     The object to test equality with.
        //
        // Returns:
        //     Returns true if the vector equals to the specified object or false otherwise.
        public override bool Equals(object obj)
        {
            if (obj is Vector3D)
            {
                return Equals((Vector3D)obj);
            }

            return false;
        }

        //
        // Summary:
        //     Returns the hashcode for this instance.
        //
        // Returns:
        //     A 32-bit signed integer hash code.
        public override int GetHashCode()
        {
            return X.GetHashCode() + Y.GetHashCode() + Z.GetHashCode();
        }

        //
        // Summary:
        //     Normalizes the vector by dividing it’s all coordinates with the vector's norm.
        //
        // Returns:
        //     Returns the value of vectors’ norm before normalization.
        public float Normalize()
        {
            float num = (float)System.Math.Sqrt(X * X + Y * Y + Z * Z);
            float num2 = 1f / num;
            X *= num2;
            Y *= num2;
            Z *= num2;
            return num;
        }

        //
        // Summary:
        //     Inverse the vector.
        //
        // Returns:
        //     Returns a vector with all coordinates equal to 1.0 divided by the value of corresponding
        //     coordinate in this vector (or equal to 0.0 if this vector has corresponding coordinate
        //     also set to 0.0).
        public Vector3D Inverse()
        {
            return new Vector3D((X == 0f) ? 0f : (1f / X), (Y == 0f) ? 0f : (1f / Y), (Z == 0f) ? 0f : (1f / Z));
        }

        //
        // Summary:
        //     Calculate absolute values of the vector.
        //
        // Returns:
        //     Returns a vector with all coordinates equal to absolute values of this vector's
        //     coordinates.
        public Vector3D Abs()
        {
            return new Vector3D(System.Math.Abs(X), System.Math.Abs(Y), System.Math.Abs(Z));
        }

        //
        // Summary:
        //     Calculates cross product of two vectors.
        //
        // Parameters:
        //   vector1:
        //     First vector to use for cross product calculation.
        //
        //   vector2:
        //     Second vector to use for cross product calculation.
        //
        // Returns:
        //     Returns cross product of the two specified vectors.
        public static Vector3D Cross(Vector3D vector1, Vector3D vector2)
        {
            return new Vector3D(vector1.Y * vector2.Z - vector1.Z * vector2.Y, vector1.Z * vector2.X - vector1.X * vector2.Z, vector1.X * vector2.Y - vector1.Y * vector2.X);
        }

        //
        // Summary:
        //     Calculates dot product of two vectors.
        //
        // Parameters:
        //   vector1:
        //     First vector to use for dot product calculation.
        //
        //   vector2:
        //     Second vector to use for dot product calculation.
        //
        // Returns:
        //     Returns dot product of the two specified vectors.
        public static float Dot(Vector3D vector1, Vector3D vector2)
        {
            return vector1.X * vector2.X + vector1.Y * vector2.Y + vector1.Z * vector2.Z;
        }

        //
        // Summary:
        //     Converts the vector to a 4D vector.
        //
        // Returns:
        //     Returns 4D vector which is an extension of the 3D vector.
        //
        // Remarks:
        //     The method returns a 4D vector which has X, Y and Z coordinates equal to the
        //     coordinates of this 3D vector and W coordinate set to 1.0.
        
    }
}
