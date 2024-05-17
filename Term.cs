using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SQLite;
using ColumnAttribute = SQLite.ColumnAttribute;
using TableAttribute = System.ComponentModel.DataAnnotations.Schema.TableAttribute;

namespace WGUAppV2
{

    [Table("term")]
    public class Term
    {
        [PrimaryKey]
        [AutoIncrement]
        [Column("termid")]
        public int TermId { get; set; }

        [Column("termname")]
        public string TermName { get; set; } = string.Empty;

        [Column("start")]
        public DateTime Start { get; set; } = DateTime.MinValue;

        [Column("end")]
        public DateTime End { get; set; } = DateTime.MinValue;
    }

}
