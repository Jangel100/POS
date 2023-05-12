using System;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;

public partial class clsDBMgrSAP
{
    private SqlConnection p_DBConn = new SqlConnection();

    public SqlConnection DBConn
    {
        get
        {
            return p_DBConn;
        }
    }

    public clsDBMgrSAP()
    {
        // Open DB Connection
        p_DBConn.ConnectionString = ConfigurationManager.ConnectionStrings["DBConnSAP"].ConnectionString;
        p_DBConn.Open();
    }

    ~clsDBMgrSAP()
    {
        // Try to Close the Connection
        try
        {
        }
        // p_DBConn.Close()
        catch (Exception ex)
        {
        }
    }

    public void GetQuerydtr(string QueryToRun, ref SqlDataReader LocalReader)
    {

        // Define Command to Execute
        var SQLComm = new SqlCommand(QueryToRun, p_DBConn);

        // Return Resultset
        LocalReader = SQLComm.ExecuteReader();
    }

    public void GetQuerydtr(string QueryToRun, SqlParameter[] Parameters, ref SqlDataReader LocalReader)
    {
        // Define Command to Execute
        var SQLComm = new SqlCommand(QueryToRun, p_DBConn);

        // Set Passed Parameters
        SQLComm.CommandType = CommandType.StoredProcedure;
        foreach (var Parameter in Parameters)
            SQLComm.Parameters.Add(Parameter);

        // Return Resultset
        LocalReader = SQLComm.ExecuteReader();
    }

    public DataSet GetQuerydts(string QueryToRun)
    {
        var LocalDataSet = new DataSet();

        // Define Command to Execute
        var SQLAdap = new SqlDataAdapter(QueryToRun, p_DBConn);
        SQLAdap.Fill(LocalDataSet);
        return LocalDataSet;
    }

    public void ExecuteCMD(string CMDToRun)
    {

        // Define Command to Execute
        var SQLComm = new SqlCommand(CMDToRun, p_DBConn);

        // Execute, With out Returning Dataset
        SQLComm.ExecuteNonQuery();
    }

    public void ExecuteCMD(string CMDToRun, SqlParameter[] Parameters)
    {

        // Define Command to Execute
        var SQLComm = new SqlCommand(CMDToRun, p_DBConn);

        // Set Passed Parameters
        SQLComm.CommandType = CommandType.StoredProcedure;
        foreach (var Parameter in Parameters)
            SQLComm.Parameters.Add(Parameter);

        // Execute, With out Returning Dataset
        SQLComm.ExecuteNonQuery();
    }

    public void ExecuteCMDReturn(string CMDToRun, ref SqlParameter[] Parameters)
    {

        // Define Command to Execute
        var SQLComm = new SqlCommand(CMDToRun, p_DBConn);

        // Set Passed Parameters
        SQLComm.CommandType = CommandType.StoredProcedure;
        foreach (var Parameter in Parameters)
            SQLComm.Parameters.Add(Parameter);

        // Execute, With out Returning Dataset
        SQLComm.ExecuteNonQuery();

        // Return Parameters
        int Index;
        var loopTo = Parameters.Length - 1;
        for (Index = 0; Index <= loopTo; Index++)
            Parameters[Index] = SQLComm.Parameters[Index];
    }
}