using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SQLite;
using ColumnAttribute = SQLite.ColumnAttribute;
using TableAttribute = System.ComponentModel.DataAnnotations.Schema.TableAttribute;

namespace WGUAppV2
{

    [Table("Course")]
    public class Course
    {
        [PrimaryKey, AutoIncrement, Column("courseId")]
        public int CourseId { get; set; }

        [Column("courseName")]
        public string CourseName { get; set; } = string.Empty;

        [Column("termId")]
        public int TermId { get; set; } // This is the foreign key

        [Column("courseStatus")]
        public string CourseStatus { get; set; } = string.Empty;

        [Column("instructor")]
        public string Instructor { get; set; } = string.Empty;

        [Column("phone")]
        public string Phone { get; set; } = string.Empty;

        [Column("email")]
        public string Email { get; set; } = string.Empty;

        [Column("start")]
        public DateTime Start { get; set; } = DateTime.MinValue;

        [Column("end")]
        public DateTime End { get; set; } = DateTime.MinValue;

        [Column("paName")]
        public string PaName { get; set; } = string.Empty;

        [Column("paStart")]
        public DateTime PaStart { get; set; } = DateTime.MinValue;

        [Column("paEnd")]
        public DateTime PaEnd { get; set; } = DateTime.MinValue;

        [Column("oaName")]
        public string OaName { get; set; } = string.Empty;

        [Column("oaStart")]
        public DateTime OaStart { get; set; } = DateTime.MinValue;

        [Column("oaEnd")]
        public DateTime OaEnd { get; set; } = DateTime.MinValue;

        [System.ComponentModel.DataAnnotations.MaxLength(500), Column("notes")]
        public string Notes { get; set; } = string.Empty;

    }

}
