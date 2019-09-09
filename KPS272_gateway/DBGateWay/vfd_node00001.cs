namespace KPS272_gateway.DBGateWay
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class vfd_node00001
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int id { get; set; }

        public DateTime? date_time { get; set; }

        public double? ai_0 { get; set; }

        public double? ai_1 { get; set; }

        public double? ai_2 { get; set; }

        public double? ai_3 { get; set; }

        public double? ai_4 { get; set; }

        public double? ai_5 { get; set; }

        public double? di_0 { get; set; }

        public double? di_1 { get; set; }

        public double? di_2 { get; set; }

        public double? di_3 { get; set; }

        public double? di_4 { get; set; }

        public double? di_5 { get; set; }

        public double? di_6 { get; set; }

        public double? di_7 { get; set; }

        public double? temperature { get; set; }

        public double? humidity { get; set; }

        public bool is_delete { get; set; }
    }
}
