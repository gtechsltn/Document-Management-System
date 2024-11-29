using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DMS.Models
{
    public class Document : Item
    {
        public int Id { get; set; }

        public bool IsComposite()
        {
            return false;
        }

        [Required]
        public string? Name { get; set; }

        [DataType(DataType.DateTime)]
        public DateTime DateCreated { get; set; }

        [DataType(DataType.DateTime)]
        public DateTime DateModified { get; set; }

        public string? Extension { get; set; }

        public double? Size { get; set; }

        public string? FilePath { get; set; }

        public string? FileType { get; set; }

        public string? AuthorName { get; set; }

        public string? SupervisorName { get; set; }

        public Level Level { get; set; }

        public Department Department { get; set; }

        public Faculty Faculty { get; set; }

        [DataType(DataType.DateTime)]
        public DateTime PublicationDate { get; set; }

        public int ParentId { get; set; }

        [NotMapped]
        public string ParentFolder { get; set; }
    }

    //public class Folder
    //{
    //    public int ParentId { get; set; }
    //    public string FolderName { get; set; }
    //}


    public enum Level { NewProject, Legacy, Production }
    public enum Department { Development, QA, UAT, Business, APP, SSA, DBA,  }

    public enum Faculty { BusienssIT, SecurityIT, InfraIT, Others }
}