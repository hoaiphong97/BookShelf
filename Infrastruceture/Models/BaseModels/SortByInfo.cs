using System.ComponentModel.DataAnnotations;

namespace Infrastructure.Models.BaseModels
{
    public class SortByInfo
    {
        #region Properties

        [Required]
        public string FieldName { get; set; }

        public bool Ascending { get; set; }

        #endregion Properties

        #region Constructors
        public SortByInfo() { }

        public SortByInfo(string fieldName, bool ascending)
        {
            FieldName = fieldName;
            Ascending = ascending;
        }

        #endregion Constructors
    }
}
