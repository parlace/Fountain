﻿using UnityEngine;
using SilverCat;

public sealed class PressAnyKeyView : UIView {

    protected override void OnCreate() {
        var obj = Resources.Load<GameObject>("View/Opening/PressAnyKeyView");
        AttachHost(obj);
    }

    public override void Update() {
        if (Input.anyKey) {
            UIManager.Close(this);
            UIManager.Open<MainMenuView>();
        }
    }
}
