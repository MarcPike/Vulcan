namespace DAL.IntegrationDb
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("MessageLog")]
    public partial class MessageLog
    {
        [Key]
        [StringLength(64)]
        public string ConversationHandle { get; set; }

        [StringLength(64)]
        public string MessageTypeName { get; set; }

        public DateTime? LoggedTime { get; set; }

        public DateTime? ImportTime { get; set; }

        [Column(TypeName = "xml")]
        public string MessageContent { get; set; }

        public string ErrorMessage { get; set; }
    }
}
