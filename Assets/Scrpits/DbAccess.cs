using UnityEngine;
using System;
using System.Collections;
using Mono.Data.Sqlite;

/// <summary>
/// Db access.
/// </summary>
public class DbAccess {
    private SqliteConnection dbConnection;
    private SqliteCommand dbCommand;
    private SqliteDataReader reader;

	/// <summary>
	/// Initializes a new instance of the <see cref="DbAccess"/> class.
	/// </summary>
	/// <param name="connectionString">Connection string.</param>
    public DbAccess (string connectionString) {
        OpenDB (connectionString);
    }

	/// <summary>
	/// Initializes a new instance of the <see cref="DbAccess"/> class.
	/// </summary>
    public DbAccess () {
		
	}

	/// <summary>
	/// Opens the DB
	/// </summary>
	/// <param name="connectionString">Connection string.</param>
    public void OpenDB (string connectionString) {
		try
   		{
			dbConnection = new SqliteConnection (connectionString);
       	 	dbConnection.Open ();
       		Debug.Log ("Connected to db");
		}
    	catch(Exception e)
    	{
       		string temp1 = e.ToString();
       		Debug.Log(temp1);
    	}
    }

	/// <summary>
	/// Closes the sql connection.
	/// </summary>
    public void CloseSqlConnection () {
        if (dbCommand != null) {
            dbCommand.Dispose ();
        }

        dbCommand = null;
        if (reader != null) {
            reader.Dispose ();
        }

        reader = null;
        if (dbConnection != null) {
            dbConnection.Close ();
        }

        dbConnection = null;
        Debug.Log ("Disconnected from db.");
    }

	/// <summary>
	/// Executes the query.
	/// </summary>
	/// <returns>The query.</returns>
	/// <param name="sqlQuery">Sql query.</param>
    public SqliteDataReader ExecuteQuery (string sqlQuery) {
        dbCommand = dbConnection.CreateCommand ();
        dbCommand.CommandText = sqlQuery;
        reader = dbCommand.ExecuteReader ();
        return reader;
    }

	/// <summary>
	/// Reads the full table.
	/// </summary>
	/// <returns>The full table.</returns>
	/// <param name="tableName">Table name.</param>
    public SqliteDataReader ReadFullTable (string tableName) {
        string query = "SELECT * FROM " + tableName;
        return ExecuteQuery (query);
    }

	/// <summary>
	/// Inserts the into.
	/// </summary>
	/// <returns>The into.</returns>
	/// <param name="tableName">Table name.</param>
	/// <param name="values">Values.</param>
    public SqliteDataReader InsertInto (string tableName, string[] values) {
        string query = "INSERT INTO " + tableName + " VALUES (" + values[0];
        for (int i = 1; i < values.Length; ++i) {
            query += ", " + values[i];
        }
        query += ")";
        return ExecuteQuery (query);
    }

	/// <summary>
	/// Updates the into.
	/// </summary>
	/// <returns>The into.</returns>
	/// <param name="tableName">Table name.</param>
	/// <param name="cols">Cols.</param>
	/// <param name="colsvalues">Colsvalues.</param>
	/// <param name="selectkey">Selectkey.</param>
	/// <param name="selectvalue">Selectvalue.</param>
	public SqliteDataReader UpdateInto (string tableName, string []cols,string []colsvalues,string selectkey,string selectvalue) {
		string query = "UPDATE "+tableName+" SET "+cols[0]+" = "+colsvalues[0];
		for (int i = 1; i < colsvalues.Length; ++i) {
		 	 query += ", " +cols[i]+" ="+ colsvalues[i];
		}
		query += " WHERE "+selectkey+" = "+selectvalue+" ";
		return ExecuteQuery (query);
	}

	/// <summary>
	/// Delete the specified tableName, cols and colsvalues.
	/// </summary>
	/// <param name="tableName">Table name.</param>
	/// <param name="cols">Cols.</param>
	/// <param name="colsvalues">Colsvalues.</param>
	public SqliteDataReader Delete(string tableName,string []cols,string []colsvalues) {
		string query = "DELETE FROM "+tableName + " WHERE " +cols[0] +" = " + colsvalues[0];
		for (int i = 1; i < colsvalues.Length; ++i) {
			query += " or " +cols[i]+" = "+ colsvalues[i];
		}
		return ExecuteQuery (query);
	}

	/// <summary>
	/// Inserts the into specific.
	/// </summary>
	/// <returns>The into specific.</returns>
	/// <param name="tableName">Table name.</param>
	/// <param name="cols">Cols.</param>
	/// <param name="values">Values.</param>
    public SqliteDataReader InsertIntoSpecific (string tableName, string[] cols, string[] values) {
        if (cols.Length != values.Length) {
            throw new SqliteException ("columns.Length != values.Length");
        }

        string query = "INSERT INTO " + tableName + "(" + cols[0];
        for (int i = 1; i < cols.Length; ++i) {
            query += ", " + cols[i];
        }

        query += ") VALUES (" + values[0];
        for (int i = 1; i < values.Length; ++i) {
            query += ", " + values[i];
        }
        query += ")";
        return ExecuteQuery (query);
    }

	/// <summary>
	/// Deletes the contents.
	/// </summary>
	/// <returns>The contents.</returns>
	/// <param name="tableName">Table name.</param>
    public SqliteDataReader DeleteContents (string tableName) {
        string query = "DELETE FROM " + tableName;
        return ExecuteQuery (query);
    }

	/// <summary>
	/// Creates the table.
	/// </summary>
	/// <returns>The table.</returns>
	/// <param name="name">Name.</param>
	/// <param name="col">Col.</param>
	/// <param name="colType">Col type.</param>
    public SqliteDataReader CreateTable (string name, string[] col, string[] colType) {
        if (col.Length != colType.Length) {
            throw new SqliteException ("columns.Length != colType.Length");
        }

		string query = "CREATE TABLE IF NOT EXISTS " + name + " (" + col[0] + " " + colType[0];

        for (int i = 1; i < col.Length; ++i) {
            query += ", " + col[i] + " " + colType[i];
        }
        query += ")";
        return ExecuteQuery (query);
    }

	/// <summary>
	/// Selects the where.
	/// </summary>
	/// <returns>The where.</returns>
	/// <param name="tableName">Table name.</param>
	/// <param name="items">Items.</param>
	/// <param name="col">Col.</param>
	/// <param name="operation">Operation.</param>
	/// <param name="values">Values.</param>
    public SqliteDataReader SelectWhere (string tableName, string[] items, string[] col, string[] operation, string[] values) {
        if (col.Length != operation.Length || operation.Length != values.Length) {
            throw new SqliteException ("col.Length != operation.Length != values.Length");
        }

        string query = "SELECT " + items[0];
        for (int i = 1; i < items.Length; ++i) {
            query += ", " + items[i];
        }

        query += " FROM " + tableName + " WHERE " + col[0] + operation[0] + "'" + values[0] + "' ";

        for (int i = 1; i < col.Length; ++i) {
            query += " AND " + col[i] + operation[i] + "'" + values[0] + "' ";
        }

        return ExecuteQuery (query);
    }
}