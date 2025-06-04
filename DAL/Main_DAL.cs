using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TestApp.MODEL;

namespace TestApp.DAL
{
    public class Main_DAL
    {
        public int InsertTestData_DT(MeasurementResult model)
        {
            string sql = $@"insert test_data
	                ([batch_id],[point_index],[gain_db],[phase_deg],[input_vswr],[output_vswr])  
                values('{model.BatchId}','{model.PointIndex}','{model.Gain}','{model.InitialPhase}','{model.InputVSWR}','{model.OutputVSWR}')";
            return Dapper.DbHelper.UpdateBySql(sql);
        }
        public int InsertTestBatch_DT(MeasurementBatch model)
        {
            string sql = $@"insert test_batch
	                ([operator],[description],[update_time])  
                values('{model.Operator}','{model.Description}','{model.UpdateTime}')";
            return Dapper.DbHelper.UpdateBySql(sql);
        }
        public int GetBatchId()
        {
            string sql = $@"select id from test_batch order by id desc";
            Dapper.SqlUtil Sqlutil = new Dapper.SqlUtil();
            var result = Sqlutil.Query<int>(sql).FirstOrDefault();
            return result;
        }
    }
}
