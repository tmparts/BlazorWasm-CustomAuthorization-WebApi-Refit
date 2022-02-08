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
        public NodeModelAnalog(int id, string name) : base(name) { Id = id; }
        public NodeModelAnalog(string name, int parent_id) : base(name) { ParentId = parent_id; }
    }
}
