namespace KPS272_gateway.DBGateWay
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class vfd_node_registry
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int id { get; set; }

        public DateTime? date_time { get; set; }

        [Required]
        public string coordinates { get; set; }

        public long? imei { get; set; }

        public int? port { get; set; }

        public string serial { get; set; }

        public string tmeic { get; set; }

        public int id_model { get; set; }

        public int id_input_voltage { get; set; }

        public int id_output_current { get; set; }

        public int id_output_power { get; set; }

        public int? id_comany { get; set; }

        public int? id_site { get; set; }

        [Required]
        public string config { get; set; }

        [Required]
        public string table_name { get; set; }

        [Required]
        public string files { get; set; }

        public bool is_delete { get; set; }
    }
}
