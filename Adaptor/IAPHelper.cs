using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

namespace Transient.Bridge {
    public struct IAPItem {
        public string id;
        public string price;
        public string tag;
    }

    public class IAPHelper {
        public void SetupIAP() {
            //TODO
            var item = new List<IAPItem>();
            SetupIAP(item, PurchaseError);
        }

        //WhenPurchaseDone(id, receipt, error)
        public void SetupIAP(List<IAPItem> item, Action<string, string, string> WhenPurchaseDone) {
            //TODO
        }

        public void IAPPurchase(string id) {
            //TODO
        }

        private void PurchaseError(string id, string receipt, string error) {
            //TODO
            //popup error
        }
    }
}