namespace ThongKe.Data.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("account")]
    public partial class account
    {
        [Key]
        [StringLength(50)]
        public string username { get; set; }

        [StringLength(50)]
        public string password { get; set; }

        [StringLength(50)]
        public string hoten { get; set; }

        [StringLength(50)]
        public string daily { get; set; }

        [StringLength(3)]
        public string chinhanh { get; set; }

        [StringLength(50)]
        public string role { get; set; }

        public bool doimatkhau { get; set; }

        [Column(TypeName = "date")]
        public DateTime ngaydoimk { get; set; }

        public bool trangthai { get; set; }

        [Required]
        [StringLength(5)]
        public string khoi { get; set; }

        [StringLength(50)]
        public string nguoitao { get; set; }

        public DateTime ngaytao { get; set; }

        [StringLength(50)]
        public string nguoicapnhat { get; set; }

        public DateTime? ngaycapnhat { get; set; }
    }
}
