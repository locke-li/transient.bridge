using System.Collections;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading;
using System.Threading.Tasks;
using System.IO;
using System.Text;
using Google.Protobuf;
using UnityEngine;
using UnityEngine.Networking;
using Transient;
using System;

namespace Transient.Bridge {
    public class NetworkHttpBatch : NetworkHttp {
        public Action<int, string, string, byte[]> WhenBatchPacketReceived { get; set; }
        public Action<int> WhenBatchDone { get; set; }
        public string AccessToken {
            set {
                var key = "x-session-token";
                DefaultHeader.Remove(key);
                DefaultHeader.TryAddWithoutValidation(key, value);
            }
        }
        
        public override void Init() {
            base.Init();
            WhenReceived = WhenReceivedBatch;
            WhenCompositeContent = WhenComposite;
            DefaultHeader.TryAddWithoutValidation("Connection", "keep-alive");
        }

        private void WhenComposite(HttpContent content_) {
            content_.Headers.TryAddWithoutValidation("x-request-id", "0");
            content_.Headers.TryAddWithoutValidation("Content-Type", "application/x-protobuf");
        }

        private void WhenReceivedBatch(byte[] data_) {
            try {
                //TODO
                // var batch = ParseFrom(data_);
                // foreach (var packet in batch.Content) {
                //     WhenBatchPacketReceived(batch.SequenceID,
                //         packet.Info,
                //         packet.Uri,
                //         packet.Data.ToByteArray());
                // }
                // WhenBatchDone?.Invoke(batch.SequenceID);
                WhenBatchDone?.Invoke(0);
            }
            catch (System.Exception e) {
                Debug.LogErrorFormat("Exception throw :{0} on parsing data", e.Message);
            }
        }
    }
}