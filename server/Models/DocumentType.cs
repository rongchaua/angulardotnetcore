using System;

namespace WebApplication.Models
{
    [Flags]
    public enum DocumentType {
        Audio,
        Video,
        Image,
        Document
    }
}