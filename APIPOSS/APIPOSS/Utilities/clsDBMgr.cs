using System;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using Microsoft.VisualBasic; // Install-Package Microsoft.VisualBasic
using Microsoft.VisualBasic.CompilerServices; // Install-Package Microsoft.VisualBasic

public  class clsDBMgr
{
    private SqlConnection p_DBConn = new SqlConnection();

    public SqlConnection DBConn
    {
        get
        {
            return p_DBConn;
        }
    }

    public clsDBMgr()
    {
        // Open DB Connection
        p_DBConn.ConnectionString = ConfigurationManager.ConnectionStrings["DBConn"].ConnectionString;
        p_DBConn.Open();
    }

    ~clsDBMgr()
    {
        // Try to Close the Connection
        try
        {
            p_DBConn.Close();
        }
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
        // Dim LocalDataSet As New DataSet
        // Dim startTrans As String
        // Dim endTrans As String
        // ' Define Command to Execute
        // Dim SQLAdap As New SqlDataAdapter(QueryToRun, p_DBConn)
        // Try
        // startTrans = Now
        // SQLAdap.Fill(LocalDataSet)
        // Catch ex As Exception
        // endTrans = Now
        // tag:
        // End Try


        // Return LocalDataSet

        var LocalDataSet = new DataSet();
        string startTrans;
        string endTrans;
        // Define Command to Execute
        // Dim SQLAdap As New SqlDataAdapter(QueryToRun, p_DBConn)
        try
        {
            startTrans = Conversions.ToString(DateAndTime.Now);

            // SQLAdap.Fill(LocalDataSet)
            var selectCMD = new SqlCommand(QueryToRun, p_DBConn);
            selectCMD.CommandTimeout = 0;
            var SQLAdap = new SqlDataAdapter();
            SQLAdap.SelectCommand = selectCMD;
            SQLAdap.Fill(LocalDataSet);
        }
        catch (Exception ex)
        {
            endTrans = Conversions.ToString(DateAndTime.Now);
        tag:
            ;
        }

        return LocalDataSet;
    }

    public DataSet GetQuerydts2(string QueryToRun)
    {
        var LocalDataSet = new DataSet();
        string startTrans;
        string endTrans;
        // Define Command to Execute
        // Dim SQLAdap As New SqlDataAdapter(QueryToRun, p_DBConn)
        try
        {
            startTrans = Conversions.ToString(DateAndTime.Now);

            // SQLAdap.Fill(LocalDataSet)
            var selectCMD = new SqlCommand(QueryToRun, p_DBConn);
            selectCMD.CommandTimeout = 0;
            var SQLAdap = new SqlDataAdapter();
            SQLAdap.SelectCommand = selectCMD;
            SQLAdap.Fill(LocalDataSet);
        }
        catch (Exception ex)
        {
            endTrans = Conversions.ToString(DateAndTime.Now);
        tag:
            ;
        }

        return LocalDataSet;
    }

    public void ExecuteCMD(string CMDToRun)
    {

        // Define Command to Execute
        var SQLComm = new SqlCommand(CMDToRun, p_DBConn);
        SQLComm.CommandTimeout = 0;

        // Execute, With out Returning Dataset
        SQLComm.ExecuteNonQuery();
    }

    public void ExecuteCMD(string CMDToRun, SqlParameter[] Parameters)
    {

        // Define Command to Execute
        var SQLComm = new SqlCommand(CMDToRun, p_DBConn);
        SQLComm.CommandType = CommandType.StoredProcedure;

        // Set Passed Parameters
        SQLComm.CommandTimeout = 0;
        foreach (var Parameter in Parameters)
            SQLComm.Parameters.Add(Parameter);

        // Execute, With out Returning Dataset
        SQLComm.ExecuteNonQuery();
    }

    public void ExecuteCMDReturn(string CMDToRun, ref SqlParameter[] Parameters)
    {

        // Define Command to Execute
        var SQLComm = new SqlCommand(CMDToRun, p_DBConn);
        SQLComm.CommandType = CommandType.StoredProcedure;

        // Set Passed Parameters
        SQLComm.CommandTimeout = 0;
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