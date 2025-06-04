using Dapper.Contrib.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TestApp.MODEL
{
    [Table("test_data")] // 数据库中的表名
    public class MeasurementResult
    {
        [Key]
        public int Id { get; set; }
        public int BatchId { get; set; }
        public int PointIndex { get; set; }
        public string Gain { get; set; }
        public string InitialPhase { get; set; }
        public string InputVSWR { get; set; }
        public string OutputVSWR { get; set; }
    }
}
