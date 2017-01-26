using System.Linq;
using System;
using System.Collections.Generic;
using WebApplication.Data;
using WebApplication.Models;
using Microsoft.EntityFrameworkCore;

namespace WebApplication.Services
{
    public class KnowledgeService: IKnowledgeService{
        private readonly ApplicationDbContext dbContext;

        public KnowledgeService(ApplicationDbContext dbContext)
        {
            this.dbContext = dbContext;
        }

        public void AddKnowledgeItem(AddKnowledgeItemModel model)
        {
            var entry = new KnowledgeEntry(){
                Content = model.Content,
                Date = DateTime.Now,
                User = "Juergen",
                Link = ToLink(model.Link),
                // Documents = ToDocuments(model.Documents)
            };
            dbContext.KnowledgeEntries.Add(entry);
            dbContext.SaveChanges();

        }

        public TimelineModel LoadKnowledgeTimeline(int page, int pageSize)
        {
            // var items = dbContext.KnowledgeEntries.Include(x=>x.Documents).ToList();
            // var count = items.Count();
            // var result = items
            //                 .OrderByDescending(x=>x.Date)
            //                 .Skip(page*pageSize)
            //                 .Take(pageSize)
            //                 .Select(x=> new TimelineItemModel(){
            //                     Id =x.Id,
            //                     Content = x.Content,
            //                     Date = x.Date,
            //                     User = x.User,
            //                     Link = ToLinkModel(x.Link),
            //                     Documents = ToDocumentModels(x.Documents)
            //                 });

            // var model = new TimelineModel(){
            //     PageNumber = page,
            //     PageSize = pageSize,
            //     Items = result,
            //     OverallLength = count
            // };
            // return model;
            return null;

        }


        private ICollection<Document> ToDocuments(IEnumerable<DocumentModel> models){
            if (models == null || !models.Any())
                return null;

            return models.Select(model => new Document(){
                Name = model.Name,
                Source = model.Source,
                Type = (DocumentType)Enum.Parse(typeof(DocumentType),model.Type)
            }).ToList();
        }

        private IEnumerable<DocumentModel> ToDocumentModels(ICollection<Document> documents){
            if (documents == null || !documents.Any())
                return new List<DocumentModel>();

            return documents.Select(x=> new DocumentModel(){
               Id = x.Id,
               Name = x.Name,
               Source = x.Source,
               Type = x.Type.ToString() 
            });
        }

        private Link ToLink(LinkModel model){
            if (model == null){
                return null;
            }

            return new Link(){
                Title = model.Title,
                Description = model.Description,
                Url = model.Url
            };
        }

        private LinkModel ToLinkModel(Link link){
            if (link ==null)
                return null;

            return new LinkModel(){
                Id = link.Id,
                Title = link.Title,
                Description = link.Description,
                Url = link.Url
            };
        }
    }
}