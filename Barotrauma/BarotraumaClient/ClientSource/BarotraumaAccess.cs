using Barotrauma;
using Microsoft.Xna.Framework;
using Prismatoid;
using Steamworks.ServerList;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace Barotrauma {
    public static class BAcc { //C# DOESN'T HAVE NON-MEMBER FUNCTIONS!?!?!?!?!?!?!? ??????????!?!?!?!?? WHAT ARE NAMESPACES FOR????
        private static PrismContext context = new PrismContext();
        static bool NoScreenReader = true;
        static byte LogLevel = 2; //0 = Basic, 1 = Development, 2 = Debug
        static int Index = 0;
        static int ItemIndex = 0;

        public static PrismContext Context { get => context; set => context = value; }

        public static bool Say (string text, bool binter = false) {
            using var backend = Context.AcquireBestBackend();
            backend.Output(text, interrupt: binter);
            if (binter) {
                DebugConsole.NewMessage("Said \"" + text + "\" with interrupt");
            } else {
                DebugConsole.NewMessage("Said \"" + text + '\"');
            }
            return true;
        }

        public static bool readCur() {
            return Say(GUI.MouseOn.ToString());
            //return Say(x => updateList.Filter(GUIComponent.Selected.get());
        }

        public static bool nextSibling() { //I will probably need an exclusion list
            if (GUI.updateList == null) {
                Say("No GUI present.", false);
                return false;
            }
            int nElement = BAcc.Index;
            while(true){
                if ((nElement) >= GUI.updateList.Count) {
                    nElement = 0;
                }
                nElement = nElement + 1;
                if ((GUI.updateList[nElement].Parent == GUI.updateList[BAcc.Index].Parent) || (nElement == BAcc.Index)) { break; }
            }
            if (nElement == BAcc.Index) { Say("No other elements.", false); }
            if (nElement < BAcc.Index) { Say("Wrapped around.", false); }
            BAcc.Index = nElement;
            GUI.ForceMouseOn(GUI.updateList[BAcc.Index]);
            return readCur();
        }
        public static bool lastSibling(){
            if (GUI.updateList == null) {
                Say("No GUI present.", false);
                return false;
            }
            int lElement = BAcc.Index;
            while (true) {
                if (lElement <= 0) {
                    lElement = GUI.updateList.Count;
                }
                lElement = lElement - 1;
                if ((GUI.updateList[lElement].Parent == GUI.updateList[BAcc.Index].Parent) || (lElement == BAcc.Index)) {break;}
            }
            if (lElement == BAcc.Index) {Say("No other elements.", false);}
            if (lElement < BAcc.Index) {Say("Wrapped around.", false);}
            BAcc.Index = lElement;
            GUI.ForceMouseOn(GUI.updateList[BAcc.Index]);
            return readCur();
        }
        public static bool nextItem(Character player) {  //TODO: Help. How do I get which character I am?
            if (player.interactablesInRange == null) {
                Say("No items nearby.", false);
                return false;
            }
            int nItem = BAcc.ItemIndex;
            do {
                if (nItem >= player.interactablesInRange.Count) {
                    nItem = 0;
                }
                nItem = nItem + 1;
            } while (nItem == BAcc.ItemIndex);
            if (nItem == BAcc.ItemIndex) { Say("No other elements.", false); }
            if (nItem < BAcc.ItemIndex) { Say("Wrapped around.", false); }
            BAcc.ItemIndex = nItem;
            //GUI.ForceMouseOn(player.interactablesInRange[BAcc.ItemIndex]);//How do I get the boxes that pop up?
            return readCur();
        }

        public static bool lastItem(Character player) {
            if (player.interactablesInRange == null) {
                Say("No items nearby.", false);
                return false;
            }
            int lItem = BAcc.ItemIndex;
            do {
                if (lItem <= 0) {
                    lItem = player.interactablesInRange.Count;
                }
                lItem = lItem - 1;
            } while (lItem == BAcc.ItemIndex);
            if (lItem == BAcc.ItemIndex) { Say("No other elements.", false); }
            if (lItem < BAcc.ItemIndex) { Say("Wrapped around.", false); }
            BAcc.ItemIndex = lItem;
            //GUI.ForceMouseOn(player.interactablesInRange[BAcc.ItemIndex]);//How do I get the boxes that pop up?
            return readCur();
        }



        public static bool goToParent() {
            if (GUI.updateList == null) {
                Say("No GUI present.", false);
                return false;
            }
            if (GUI.updateList[BAcc.Index].Parent == null) {
                Say("No parent GUI Element.");
                return false;
            }
            int nElement = BAcc.Index;
            for (int i = 0; i == GUI.updateList.Count; i+=1) {
                if (GUI.updateList[i] == GUI.updateList[BAcc.Index].Parent) {
                    nElement = i;
                }
            }
            if (nElement == BAcc.Index) {
                Say("GUI List changed during search. Please retry.");
                return false;
            } else {
                BAcc.Index = nElement;
            }
            GUI.ForceMouseOn(GUI.updateList[BAcc.Index]);
            return readCur();
        }

        public static bool goToChild() {
            if (GUI.updateList == null) {
                Say("No GUI present.", false);
                return false;
            }
            if (GUI.updateList[BAcc.Index].Children == null) {
                Say("No child GUI Elements.");
                return false;
            }
            int lElement = BAcc.Index;
            do {
                if (lElement <= 0) {
                    lElement = GUI.updateList.Count;
                }
                lElement = lElement - 1;
            } while ((GUI.updateList[lElement].Parent == GUI.updateList[BAcc.Index].Children.ElementAt(0)) || (lElement == BAcc.Index));
            if (lElement == BAcc.Index) {
                Say("GUI List changed during search. Please retry.");
                return false;
            } else {
                BAcc.Index = lElement;
            }
            GUI.ForceMouseOn(GUI.updateList[BAcc.Index]);
            return readCur();
        }
    }
}
