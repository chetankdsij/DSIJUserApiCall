/*
' DotNetNuke® - http://www.dotnetnuke.com
' Copyright (c) 2002-2006
' by Perpetual Motion Interactive Systems Inc. ( http://www.perpetualmotion.ca )
'
' Permission is hereby granted, free of charge, to any person obtaining a copy of this software and associated 
' documentation files (the "Software"), to deal in the Software without restriction, including without limitation 
' the rights to use, copy, modify, merge, publish, distribute, sublicense, and/or sell copies of the Software, and 
' to permit persons to whom the Software is furnished to do so, subject to the following conditions:
'
' The above copyright notice and this permission notice shall be included in all copies or substantial portions 
' of the Software.
'
' THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR IMPLIED, INCLUDING BUT NOT LIMITED 
' TO THE WARRANTIES OF MERCHANTABILITY, FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL 
' THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER LIABILITY, WHETHER IN AN ACTION OF 
' CONTRACT, TORT OR OTHERWISE, ARISING FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER 
' DEALINGS IN THE SOFTWARE.
 */

using System;
using System.Data;
using System.Data.SqlClient;

using Microsoft.ApplicationBlocks.Data;

namespace YourCompany.Modules.DSJUserSubscription
{
    /// -----------------------------------------------------------------------------
    /// <summary>
    /// The SqlDataProvider class is a SQL Server implementation of the abstract DataProvider
    /// </summary>
    /// <remarks>
    /// </remarks>
    /// <history>
    /// </history>
    /// -----------------------------------------------------------------------------
    public class SqlDataProvider : DataProvider
    {

        #region Private Members
        private string _connectionString;
        #endregion

        #region Constructors

        /// <summary>
        /// Constructs new SqlDataProvider instance
        /// </summary>
        public SqlDataProvider()
        {
            //Read the configuration specific information for this provider

            //Read the attributes for this provider
            //Get Connection string from web.config
            _connectionString = System.Configuration.ConfigurationManager.ConnectionStrings["SiteSqlServer"].ConnectionString; 
        }

        #endregion

        #region Properties

        /// <summary>
        /// Gets and sets the connection string
        /// </summary>
        public string ConnectionString
        {
            get { return _connectionString; }
        }

        /// <summary>
        /// Gets and sets the Provider path
        /// </summary>
        
        #endregion


        public override DataSet GetUserInfoData()
        {
            try
            {
                return SqlHelper.ExecuteDataset(ConnectionString, CommandType.StoredProcedure, "GetUserInfoData", null);
            }
            catch (Exception ex)
            {
                return null;
            }
        }
    }
}