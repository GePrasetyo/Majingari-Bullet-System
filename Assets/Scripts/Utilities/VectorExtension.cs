using UnityEngine;

namespace ShootingGames.Utils {
    public static class VectorExtension {
        private enum Axis { X, Y, Z, XY, XZ, YZ }

        public static Vector3 ExcludingX(this Vector3 self) => Excluding(self, Axis.X);
        public static Vector3 ExcludingY(this Vector3 self) => Excluding(self, Axis.Y);
        public static Vector3 ExcludingZ(this Vector3 self) => Excluding(self, Axis.Z);
        public static Vector3 ExcludingXY(this Vector3 self) => Excluding(self, Axis.XY);
        public static Vector3 ExcludingXZ(this Vector3 self) => Excluding(self, Axis.XZ);
        public static Vector3 ExcludingYZ(this Vector3 self) => Excluding(self, Axis.YZ);

        private static Vector3 Excluding(this Vector3 self, Axis axis) {
            switch (axis) {
                case Axis.X:
                    return new Vector3(0, self.y, self.z);
                case Axis.Y:
                    return new Vector3(self.x, 0, self.z);
                case Axis.Z:
                    return new Vector3(self.x, self.y, 0);
                case Axis.XY:
                    return new Vector3(0, 0, self.z);
                case Axis.XZ:
                    return new Vector3(0, self.y, 0);
                case Axis.YZ:
                    return new Vector3(self.x, 0, 0);
                default:
                    return self;
            }

        }
    }
}