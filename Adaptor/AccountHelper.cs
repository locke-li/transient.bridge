using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

namespace Transient.Bridge {
    public enum AccountType {
        None,
        Guest,//local user
        User,//registered user
        Bind,//bind with third party
    }

    public class AccountHelper {
        public string id;
        public AccountType type;
        public string info;
        public string token;

        public void LoginAuto(AccountType typeAuto, Action<bool> WhenLoginDone) {
            //TODO try local cache
            var cacheValid = false;
            if (cacheValid) {
                //id =
                //type =
                //info =
                Login(type, WhenLoginDone);
                return;
            }
            if (typeAuto == AccountType.None) WhenLoginDone(false);
            else Login(typeAuto, WhenLoginDone);
        }

        public void Login(AccountType type, Action<bool> WhenLoginDone) {
            //TODO
            WhenLoginDone(true);
        }

        public void LoginCurrent(Action<bool> WhenLoginDone) {
            //TODO
            WhenLoginDone(true);
        }

        public void Logout() {
            //TODO
        }

        public void Bind(string info, Action<bool> WhenBind) {
            //TODO
            WhenBind(true);
        }

        public void UpdateUser(string info) {
            //TODO
        }
    }
}