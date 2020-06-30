using System;
using System.Linq;
using Harmony;
using Microsoft.Xna.Framework;
using StardewModdingAPI;
using StardewModdingAPI.Events;
using StardewModdingAPI.Utilities;
using StardewValley;

namespace SVMarriageRejections {
    public class ModEntry : Mod {
        public override void Entry(IModHelper helper) {
            helper.Events.GameLoop.SaveCreating += OnNewSaveCreating;

            // Set up required parts for the patch class
            NPCPatches.Initialize(Monitor, helper);
            var harmony = HarmonyInstance.Create(ModManifest.UniqueID);
            // Patch gift handling method
            harmony.Patch(AccessTools.Method(typeof(NPC), nameof(NPC.tryToReceiveActiveObject)),
                new HarmonyMethod(typeof(NPCPatches), nameof(NPCPatches.tryToReceiveActiveObject_Prefix)));
        }
        private void OnNewSaveCreating(object sender, SaveCreatingEventArgs args) {
            var model = new NPCOrientations();
            // Game is being created; hard-code one straight lass for now and figure out the rest later
            foreach (var name in Constants.EligibleNpcs) {
                var sample = Game1.random.NextDouble();
                if(sample < 0.3) {
                    Monitor.Log($"Adding {name} as gay");
                    model.Orientations.Add(new NPCOrientation(name, Orientation.Gay));
                } else if (sample < 0.6) {
                    Monitor.Log($"Adding {name} as straight");
                    model.Orientations.Add(new NPCOrientation(name, Orientation.Straight));
                } else {
                    Monitor.Log($"Adding {name} as bi");
                    model.Orientations.Add(new NPCOrientation(name, Orientation.Bi));
                }
            }
            
            Helper.Data.WriteSaveData(Constants.SaveDataKey, model);
        }
    }
}