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
        //WhenPurchaseDone(id, receipt, error)
        public Action<string, string, string> WhenPurchaseDone;

        public void SetupIAP() {
            //TODO
            var item = new List<IAPItem>();
            SetupIAP(item);
        }

        public void SetupIAP(List<IAPItem> item) {
            //TODO
        }

        public void CompleteTransaction(string id, string receipt) {
            //TODO
        }

        private void PurchaseError(string id, string receipt, string error) {
            //TODO
            //popup error
        }
    }
}