using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace QHLand
{
    public static class FadeMenusManager
    {
        private static List<FadeMenus> fademenus = new List<FadeMenus>();

        public static void Add(FadeMenus menu, eFadeMenus type)
        {
            menu.SetID(type);
            menu.SetManager(AbstractManagerEditor.manager);
            fademenus.Add(menu);
        }

        public static void SetMenusSkin(GUISkin skin)
        {
            foreach (FadeMenus menu in fademenus)
            {
                menu.SetSkin(skin);
            }
        }

        public static FadeMenus GetFadeMenu(eFadeMenus type)
        {
            foreach (FadeMenus menu in fademenus)
            {
                if (menu.GetID() == type)
                    return menu;
            }
            return null;
        }

        public static void Clear()
        {
            fademenus.Clear();
        }
    }
}