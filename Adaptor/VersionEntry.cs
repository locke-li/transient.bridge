using Transient;
using System.Collections;
using UnityEngine;
using System;

namespace Transient.Bridge {
    public sealed class VersionEntry {
        public bool WillUpdate { get; private set; }
        public float Progress { get; private set; }
        private bool cancel;

        public IEnumerator StartSync(Action<string> WhenVersionInfoReceived) {
            cancel = false;
            WillUpdate = false;
            var url = AppEnv.Option["EntryUrl"];
            //TODO
            return WaitSync();
        }

        private IEnumerator WaitSync() {
            var wait = new WaitForSecondsRealtime(0.1f);
            while (Progress < 1) {
                if (cancel) yield break;
                Progress += 0.1f;//TODO
                yield return wait;
            }
        }

        public IEnumerator StartUpdate(Action<float> UpdateProgress) {
            cancel = false;
            //TODO
            //first start
            //or
            //start again
            return WaitUpdate(UpdateProgress);
        }

        public IEnumerator WaitUpdate(Action<float> UpdateProgress) {
            var wait = new WaitForSecondsRealtime(0.5f);
            while (Progress < 1) {
                if (cancel) yield break;
                Progress += 0.5f;//TODO
                UpdateProgress(Progress);
                yield return wait;
            }
        }

        public void Stop() {
            cancel = true;
        }
    }
}
