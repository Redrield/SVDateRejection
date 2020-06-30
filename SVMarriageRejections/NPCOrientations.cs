using System.Collections.Generic;
using System.Linq;
using System.Threading;

namespace SVMarriageRejections {
    public enum Orientation {
        Straight,
        Gay,
        Bi
    }
    
    public class NPCOrientation {
        public string Name { get; set; }
        public Orientation Orientation { get; set; }

        public NPCOrientation(string name, Orientation orientation) {
            this.Name = name;
            this.Orientation = orientation;
        }

        public override string ToString() {
            return $"NPCOrientation {{ Name = {Name}, Orientation = {Orientation} }}";
        }
    }
    
    public class NPCOrientations {
        public List<NPCOrientation> Orientations { get; set; }

        public NPCOrientations() {
            this.Orientations = new List<NPCOrientation>();
        }

        public NPCOrientations(List<NPCOrientation> orientations) {
            this.Orientations = orientations;
        }

        public NPCOrientation FindByName(string name) {
            return Orientations
                .First(orient => orient.Name == name);
        }

        public override string ToString() {
            return $"NPCOrientations {{ Orientations = {Orientations} }}";
        }
    }
}
