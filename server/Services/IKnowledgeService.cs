using WebApplication.Models;

namespace WebApplication.Services
{
    public interface IKnowledgeService {
        TimelineModel LoadKnowledgeTimeline(int page, int pageSize);
        void AddKnowledgeItem(AddKnowledgeItemModel model);
        
    }
}