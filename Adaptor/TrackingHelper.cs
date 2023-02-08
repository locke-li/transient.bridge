using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

namespace Transient.Bridge {
    [Flags]
    public enum TrackingPlatform {
        None,
    }

    public class TrackingHelper {
        public void UserUpdate(TrackingPlatform target, string json) {
            //TODO
        }

        public void UserSet(TrackingPlatform target, string json) {
            //TODO
        }

        public void Trace(TrackingPlatform target, string name, string json) {
            //TODO
        }
    }
}