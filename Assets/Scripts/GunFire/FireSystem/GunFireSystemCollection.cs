using System.Collections.Generic;
using System.Linq;

namespace ShootingGames.Gun {
    public static class GunFireSystemCollection {
        private static Dictionary<GunFireFactory, GunFireSystem> projectileSystems = new Dictionary<GunFireFactory, GunFireSystem>();

        internal static bool TryResolveSystem(this GunFireFactory fireFactory, out GunFireSystem system) {
            if (projectileSystems.TryGetValue(fireFactory, out GunFireSystem systemObject)) {
                system = systemObject;
                return true;
            }

            system = null;
            return false;
        }

        internal static void RegisterSystem(GunFireFactory key, GunFireSystem system) {
            projectileSystems[key] = system;
        }

        public static void ClearGunSystem() {
            GunFireSystem[] gunFireSystems = projectileSystems.Values.ToArray();

            for (int i = 0; i < gunFireSystems.Length; i++) {
                gunFireSystems[i]?.Disable();
            }
        }
    }
}