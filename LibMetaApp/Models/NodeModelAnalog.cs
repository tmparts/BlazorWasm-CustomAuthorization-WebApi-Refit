////////////////////////////////////////////////
// © https://github.com/badhitman - @fakegov 
////////////////////////////////////////////////

namespace LibMetaApp.Models
{
    public class NodeModelAnalog : EntryModel
    {
        /// <summary>
        /// 
        /// </summary>
        public int ParentId { get; set; }

        public NodeModelAnalog() { }
        public NodeModelAnalog(int id, string name) : base(id, name) { }
        public NodeModelAnalog(int id, string name, int parent_id) : base(id, name) { ParentId = parent_id; }
    }
}
