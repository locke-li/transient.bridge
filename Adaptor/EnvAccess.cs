using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Transient.Bridge {
    public static class EnvAccess  {
        public static VersionEntry EnvVersion;
        public static NetworkHttpBatch EnvHttp;
        public static AccountHelper EnvAccount;
        public static IAPHelper EnvIAP;
        public static TrackingHelper EnvTracking;
    }
}