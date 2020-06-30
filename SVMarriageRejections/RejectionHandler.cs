using StardewValley;

namespace SVMarriageRejections {
    public static class RejectionHandler {
        public static bool HandleFemale(NPC me, Farmer them, Orientation orientation) {
            // Allow to progress as normal if the NPCs orientation matches the PC
            if (!them.IsMale && orientation == Orientation.Gay) return true;
            if (them.IsMale && orientation == Orientation.Straight) return true;

            // Since the mixin completely overrides the game function, do what the game function would do to start off an interaction
            them.Halt();
            them.faceGeneralDirection(me.getStandingPosition());
            if (!them.IsMale) {
                me.CurrentDialogue.Push(new Dialogue(ExtraDialogues.StraightGirlRejection, me));
                Game1.drawDialogue(me);
                me.doEmote(Constants.EmoteSad);
            } else {
                me.CurrentDialogue.Push(new Dialogue(ExtraDialogues.StraightGuyRejection, me));
                Game1.drawDialogue(me);
                me.doEmote(Constants.EmoteSad);
            }
            // Never need to change any friendship data since it's a rejection, fun!

            return false;
        }

        public static bool HandleMale(NPC me, Farmer them, Orientation orientation) {
            // Allow to progress as normal if the NPCs orientation matches the PC
            if (them.IsMale && orientation == Orientation.Gay) return true;
            if (!them.IsMale && orientation == Orientation.Straight) return true;

            them.Halt();
            them.faceGeneralDirection(me.getStandingPosition());
            if (them.IsMale) {
                me.CurrentDialogue.Push(new Dialogue(ExtraDialogues.StraightGuyRejection, me));
                Game1.drawDialogue(me);
                me.doEmote(Constants.EmoteSad);
            } else {
                me.CurrentDialogue.Push(new Dialogue(ExtraDialogues.StraightGirlRejection, me));
                Game1.drawDialogue(me);
                me.doEmote(Constants.EmoteSad);
            }

            return false;
        }
    }
}