using UnityEngine;
using UnityEngine.UI;
using EasyUI.Tabs;

public class TabsUIHorizontal : TabsUI
{
    #if UNITY_EDITOR
    private void OnValidate() {
        base.Validate(TabsType.Horizontal);
    }
    #endif
}
