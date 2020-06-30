using System;
using System.IO;
using StardewModdingAPI;
using StardewValley;

namespace SVMarriageRejections {
    public class NPCPatches {
        private static IMonitor Monitor;
        private static IModHelper Helper;

        public static void Initialize(IMonitor monitor, IModHelper helper) {
            Monitor = monitor;
            Helper = helper;
        }

        public static bool tryToReceiveActiveObject_Prefix(NPC __instance, Farmer who) {
            try {

                Monitor.Log($"Interaction with {__instance.Name}", LogLevel.Info);
                // If this NPC isn't eligible then abort immediately.
                if (!Constants.EligibleNpcs.Contains(__instance.Name)) return true;

                // Read the list of NPC orientations and find the orientation of the NPC in question
                var orientationData = Helper.Data.ReadSaveData<NPCOrientations>(Constants.SaveDataKey);
                var myOrientation = orientationData.FindByName(__instance.Name);
                Monitor.Log($"Found orientation for {__instance.Name}, they are {myOrientation.Orientation}");

                // Default for NPCs anyways, so let the game go
                if(myOrientation.Orientation == Orientation.Bi) return true; 
                
                var item = who.ActiveObject;
                if (item.parentSheetIndex != 458) return true; // Item is a bouquet
                
                var friendshipData = who.friendshipData[__instance.Name];
                // All checks that would happen prior to accepting a bouquet. If any of these fail then let the game do its own thing
                if (friendshipData.IsDating() || friendshipData.IsDivorced() || friendshipData.Points < 2000)
                    return true;

                // Delegate to the handler functions. Return value dictates whether anything has happened/game code should run
                switch (__instance.Gender) {
                    case NPC.female:
                        return RejectionHandler.HandleFemale(__instance, who, myOrientation.Orientation);
                    case NPC.male:
                        return RejectionHandler.HandleMale(__instance, who, myOrientation.Orientation);
                    default: throw new InvalidDataException($"Unexpected gender for dateable NPC: {__instance.Gender}");
                }
            }
            catch (Exception ex) {
                Monitor.Log($"Failed to run patch: {ex}", LogLevel.Error);
                return true;
            }
        }
    }
}