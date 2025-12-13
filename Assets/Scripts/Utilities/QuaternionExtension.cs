using UnityEngine;

namespace ShootingGames.Utils {
    public static class QuaternionExtension {
        public static Quaternion IsolateXRotation(this Quaternion q) {
            float theta_x = Mathf.Atan2(q.x, q.w);

            Quaternion xRotation = new Quaternion(Mathf.Sin(theta_x), 0, 0, Mathf.Cos(theta_x));

            return xRotation;
        }

        public static Quaternion IsolateYRotation(this Quaternion q) {
            float theta_y = Mathf.Atan2(q.y, q.w);

            Quaternion yRotation = new Quaternion(0, Mathf.Sin(theta_y), 0, Mathf.Cos(theta_y));

            return yRotation;
        }

        public static Quaternion IsolateZRotation(this Quaternion q) {
            float theta_z = Mathf.Atan2(q.z, q.w);

            Quaternion zRotation = new Quaternion(0, 0, Mathf.Sin(theta_z), Mathf.Cos(theta_z));

            return zRotation;
        }
    }
}