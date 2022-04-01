////////////////////////////////////////////////
// © https://github.com/badhitman - @fakegov 
////////////////////////////////////////////////

namespace MetaLib.Models
{
    public class NodeModel : EntryModel
    {
        /// <summary>
        /// 
        /// </summary>
        public int ParentId { get; set; }

        public NodeModel() { }
        public NodeModel(int id, string name) : base(name) { Id = id; }
        public NodeModel(string name, int parent_id) : base(name) { ParentId = parent_id; }
    }
}
