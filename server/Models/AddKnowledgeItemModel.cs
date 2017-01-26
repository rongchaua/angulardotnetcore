using System.Collections.Generic;

namespace WebApplication.Models
{
    public class AddKnowledgeItemModel {
        public string Content { get; set; }
        public LinkModel Link { get; set; }
        public IEnumerable<DocumentModel> Documents { get; set; }
    }
}