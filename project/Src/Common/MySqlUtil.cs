using System;
using System.Data;
using MySql.Data;
using MySql.Data.MySqlClient;
using System.Diagnostics;

namespace Smrf.AppLib
{
//*****************************************************************************
//  Class: MySqlUtil
//
/// <summary>
/// Utility methods for working with MySql databases.
/// </summary>
///
/// <remarks>
/// All methods are static.
/// </remarks>
//*****************************************************************************

public static class MySqlUtil
{
    //*************************************************************************
    //  Method: ExecuteStoredProcedureNonQuery()
    //
    /// <summary>
    /// Executes a MySql stored procedure that does not return results.
    /// </summary>
    ///
    /// <param name="connectionString">
    /// Database connection string.
    /// </param>
    ///
    /// <param name="storedProcedureName">
    /// Name of the stored procedure.
    /// </param>
    ///
    /// <param name="commandTimeoutSeconds">
    /// Time to wait for the command to complete before timing out, in seconds.
    /// Can't be zero.
    /// </param>
    ///
    /// <param name="nameValuePairs">
    /// Zero or stored procedure parameter name/value pairs.  Each parameter
    /// name must start with "@".  A parameter value can be null.
    /// </param>
    //*************************************************************************

    public static void
    ExecuteStoredProcedureNonQuery
    (
        String connectionString,
        String storedProcedureName,
        Int32 commandTimeoutSeconds,
        params Object [] nameValuePairs
    )
    {
        Debug.Assert( !String.IsNullOrEmpty(connectionString) );
        Debug.Assert( !String.IsNullOrEmpty(storedProcedureName) );
        Debug.Assert(commandTimeoutSeconds > 0);
        Debug.Assert(nameValuePairs.Length % 2 == 0);

        MySqlConnection mySqlConnection;
        MySqlCommand mySqlCommand;

        GetMySqlConnectionAndCommand(connectionString, storedProcedureName,
            commandTimeoutSeconds, out mySqlConnection, out mySqlCommand,
            nameValuePairs);

        using (mySqlConnection)
        {
            mySqlConnection.Open();
            mySqlCommand.ExecuteNonQuery();
        }
    }

    //*************************************************************************
    //  Method: GetMySqlConnectionAndCommand()
    //
    /// <summary>
    /// Creates new MySqlConnection and MySqlCommand objects for executing a
    /// stored procedure.
    /// </summary>
    ///
    /// <param name="connectionString">
    /// Database connection string.
    /// </param>
    ///
    /// <param name="storedProcedureName">
    /// Name of the stored procedure.
    /// </param>
    ///
    /// <param name="commandTimeoutSeconds">
    /// Time to wait for the command to complete before timing out, in seconds.
    /// Can't be zero.
    /// </param>
    ///
    /// <param name="mySqlConnection">
    /// Where the new SqlConnection object gets stored.  The connection is not
    /// opened by this method.
    /// </param>
    ///
    /// <param name="mySqlCommand">
    /// Where the new SqlCommand object gets stored.
    /// </param>
    ///
    /// <param name="nameValuePairs">
    /// Zero or stored procedure parameter name/value pairs.  Each parameter
    /// name must start with "@".  A parameter value can be null.
    /// </param>
    //*************************************************************************

    public static void
    GetMySqlConnectionAndCommand
    (
        String connectionString,
        String storedProcedureName,
        Int32 commandTimeoutSeconds,
        out MySqlConnection mySqlConnection,
        out MySqlCommand mySqlCommand,
        params Object [] nameValuePairs
    )
    {
        Debug.Assert( !String.IsNullOrEmpty(connectionString) );
        Debug.Assert( !String.IsNullOrEmpty(storedProcedureName) );
        Debug.Assert(commandTimeoutSeconds > 0);
        Debug.Assert(nameValuePairs.Length % 2 == 0);

        mySqlConnection = new MySqlConnection(connectionString);
        mySqlCommand = new MySqlCommand(storedProcedureName, mySqlConnection);
        mySqlCommand.CommandType = CommandType.StoredProcedure;
        mySqlCommand.CommandTimeout = commandTimeoutSeconds;
        MySqlParameterCollection parameters = mySqlCommand.Parameters;

        for (Int32 i = 0; i < nameValuePairs.Length; i += 2)
        {
            Debug.Assert(nameValuePairs[i + 0] is String);

            String name = (String)nameValuePairs[i + 0];

            Debug.Assert( !String.IsNullOrEmpty(name) );
            Debug.Assert(name[0] == '@');

            Object value = nameValuePairs[i + 1];

            if (value == null)
            {
                // You cannot set a MySqlParameter to null.  It must be
                // DBNull.Value.

                value = DBNull.Value;
            }

            parameters.AddWithValue(name, value);
        }
    }

    //*************************************************************************
    //  Method: GetNullableString()
    //
    /// <summary>
    /// Gets a column value that can be either DBNull or a string.
    /// </summary>
    ///
    /// <param name="mySqlDataReader">
    /// The MySqlDataReader object to use, already positioned to a record.
    /// </param>
    ///
    /// <param name="columnIndex">
    /// Index of the column to get the value from.
    /// </param>
    ///
    /// <returns>
    /// The string in the column, or null if the column contains DBNull.
    /// </returns>
    //*************************************************************************

    static public String
    GetNullableString
    (
        MySqlDataReader mySqlDataReader,
        Int32 columnIndex
    )
    {
        Debug.Assert(mySqlDataReader != null);
        Debug.Assert(mySqlDataReader.HasRows);
        Debug.Assert(columnIndex >= 0);

        if ( mySqlDataReader.IsDBNull(columnIndex) )
        {
            return (null);
        }

        return ( mySqlDataReader.GetString(columnIndex) );
    }
}

}
